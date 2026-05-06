using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public class CalculoDelCuil
    {
        public string CalcularCUIL(string dni, string sexo)
        {
            // 1. Limpieza y validación inicial del DNI.
            dni = dni.Trim().Replace(".", "").Replace("-", "").Replace(" ", "");

            if (dni.Length < 7 || dni.Length > 8 || !long.TryParse(dni, out _))
            {
                return "DNI INVÁLIDO";
            }

            // Asegurar 8 dígitos, rellenando con '0' a la izquierda si es DNI de 7 dígitos.
            dni = dni.PadLeft(8, '0');

            // 2. Determinar el Prefijo (XX) inicial basado en el Sexo.
            string prefijo = "";
            string sexoUpper = sexo.ToUpper();

            if (sexoUpper == "MASCULINO" || sexoUpper == "HOMBRE" || sexoUpper == "H")
            {
                prefijo = "20";
            }
            else if (sexoUpper == "FEMENINO" || sexoUpper == "MUJER" || sexoUpper == "M" || sexoUpper == "F")
            {
                prefijo = "27";
            }
            else
            {
                // Este caso podría ser para CUIT (Sociedad/Empresa), que típicamente usa 30.
                prefijo = "30";
            }

            // 3. Función auxiliar para el cálculo del dígito verificador.
            // Se realiza la multiplicación de la base (prefijo + dni) por la matriz de coeficientes.
            (string nuevoPrefijo, int digitoVerificador) CalcularDV(string p, string d)
            {
                string baseCuil = p + d;
                int[] multiplicadores = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                int suma = 0;

                for (int i = 0; i < 10; i++)
                {
                    // Se convierte el char a int. BaseCuil[i] - '0' es un truco rápido para char a int.
                    int digito = baseCuil[i] - '0';
                    suma += digito * multiplicadores[i];
                }

                int resto = suma % 11;
                int provisional = 11 - resto;
                int dv = provisional;
                string prefijoActualizado = p;

                // Reglas de excepción (Resto = 1 o provisional = 10)
                if (resto == 0)
                {
                    dv = 0;
                }
                else if (resto == 1)
                {
                    // Si el resto es 1, se produce un cambio de prefijo.
                    if (p == "20")
                    {
                        prefijoActualizado = "23";
                        dv = 9;
                    }
                    else if (p == "27")
                    {
                        prefijoActualizado = "23";
                        dv = 4;
                    }
                    else // Caso CUIT (30)
                    {
                        prefijoActualizado = "33";
                        dv = 0;
                    }
                }
                else
                {
                    dv = provisional;
                }

                return (prefijoActualizado, dv);
            }

            // 4. Primer intento de cálculo del CUIL.
            var resultado = CalcularDV(prefijo, dni);

            // 5. Se verifica si hubo cambio de prefijo y recalcular si es necesario.
            if (resultado.nuevoPrefijo != prefijo)
            {
                // Si el prefijo cambió (ej. de 20 a 23), se debe recalcular el dígito verificador 
                // con el nuevo prefijo para el 99.9% de los casos.
                var resultadoRecalculo = CalcularDV(resultado.nuevoPrefijo, dni);

                // En el recalculo, el DV no puede volver a ser 10, si lo fuera, se ajusta a 9 o 0.
                if (resultadoRecalculo.digitoVerificador == 10)
                {
                    // Este es un caso de doble excepción, casi siempre se ajusta a 9 o 0.
                    // Para CUIL (personas), casi siempre es 9 si viene de 23.
                    if (resultadoRecalculo.nuevoPrefijo == "23")
                    {
                        resultadoRecalculo.digitoVerificador = 9;
                    }
                    else if (resultadoRecalculo.nuevoPrefijo == "33")
                    {
                        resultadoRecalculo.digitoVerificador = 0;
                    }

                }

                return $"{resultadoRecalculo.nuevoPrefijo}-{dni}-{resultadoRecalculo.digitoVerificador}";
            }

            // 6. Si no hubo cambio de prefijo, usar el resultado del primer cálculo.
            return $"{resultado.nuevoPrefijo}-{dni}-{resultado.digitoVerificador}";
        }
    }
}

