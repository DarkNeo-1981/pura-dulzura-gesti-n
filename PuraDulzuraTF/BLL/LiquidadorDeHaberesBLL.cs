using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class LiquidadorDeHaberesBLL
    {
        private const decimal TASA_JUBILACION = 0.11m;
        private const decimal TASA_PAMI = 0.03m;
        private const decimal TASA_OBRA_SOCIAL = 0.03m;
        private const decimal HORAS_MES = 200m;
        private const decimal RECARGO_HORA_EXTRA = 1.5m; // 50% de recargo

        public ReciboDeSueldo LiquidarMes(EmpleadoBase empleado, List<Novedad> novedades, DateTime periodo, string tipoLiquidacion)
        {
            var recibo = new ReciboDeSueldo
            {
                DNI_Empleado = empleado.DNI,
                Periodo = periodo,
                FechaEmision = DateTime.Now,
                Detalle = new List<ItemRecibo>()
            };

            // 1. CÁLCULO DE HABERES BASE
            decimal sueldoBaseCalculado = CalcularHaberBaseYSueldoBruto(recibo, empleado, tipoLiquidacion);

            // 2. APLICAR NOVEDADES (Haberes y Descuentos)
            ProcesarNovedades(recibo, novedades, sueldoBaseCalculado);

            // Suma de todos los Haberes (que es la base para los descuentos y el bruto)
            decimal sueldoBrutoTotal = recibo.Detalle.Where(i => i.Tipo == "Haber").Sum(i => i.Monto);

            // 3. APLICAR APORTES LEGALES
            AplicarAportesLegales(recibo, sueldoBrutoTotal, tipoLiquidacion);

            // 4. FINALIZACIÓN Y TOTALES 
            // Cálculo del total de descuentos sumando todos los ítems de tipo "Descuento"
            decimal totalDescuentos = recibo.Detalle.Where(i => i.Tipo == "Descuento").Sum(i => i.Monto);

            // Asignar a las propiedades que usa el PdfGenerator
            recibo.TotalHaberes = sueldoBrutoTotal;
            recibo.TotalDescuentos = totalDescuentos;

            // Otras propiedades
            recibo.Bruto = sueldoBrutoTotal;
            recibo.NetoAPagar = sueldoBrutoTotal - totalDescuentos;

            return recibo;
        }

        private decimal CalcularHaberBaseYSueldoBruto(ReciboDeSueldo recibo, EmpleadoBase empleado, string tipoLiquidacion)
        {
            decimal montoBase;
            string conceptoBase;

            if (tipoLiquidacion.Contains("Normal"))
            {
                montoBase = empleado.SueldoBasico;
                conceptoBase = "Sueldo Básico";
            }
            else if (tipoLiquidacion.Contains("Aguinaldo"))
            {
                montoBase = empleado.SueldoBasico / 2;
                conceptoBase = "Sueldo Anual Complementario (SAC)";
            }
            else
            {
                montoBase = empleado.SueldoBasico;
                conceptoBase = "Sueldo Base (Liq. Específica)";
            }

            recibo.Detalle.Add(new ItemRecibo
            {
                Concepto = conceptoBase,
                Tipo = "Haber",
                Monto = montoBase
            });

            return montoBase;
        }

        private void ProcesarNovedades(ReciboDeSueldo recibo, List<Novedad> novedades, decimal sueldoBaseCalculado)
        {
            decimal valorHoraBase = sueldoBaseCalculado / HORAS_MES;

            foreach (var novedad in novedades)
            {
                decimal montoHaber = 0m;
                string conceptoHaber = novedad.TipoNovedad;

                if (novedad.EsDescuento)
                {
                    recibo.Detalle.Add(new ItemRecibo
                    {
                        Concepto = $"Descuento ({novedad.TipoNovedad})",
                        Tipo = "Descuento",
                        Monto = novedad.Valor
                    });
                }
                else
                {
                    switch (novedad.TipoNovedad.ToUpper())
                    {
                        case "HORAEXTRA":
                            decimal valorHoraExtra = valorHoraBase * RECARGO_HORA_EXTRA;
                            montoHaber = novedad.Valor * valorHoraExtra;
                            conceptoHaber = $"Horas Extra ({novedad.Valor} hs)";
                            break;
                        case "PREMIO":
                            montoHaber = novedad.Valor;
                            break;
                        default:
                            montoHaber = novedad.Valor;
                            break;
                    }

                    recibo.Detalle.Add(new ItemRecibo
                    {
                        Concepto = conceptoHaber,
                        Tipo = "Haber",
                        Monto = montoHaber
                    });
                }
            }
        }

        private void AplicarAportesLegales(ReciboDeSueldo recibo, decimal sueldoBrutoTotal, string tipoLiquidacion)
        {
            decimal baseCalculo = sueldoBrutoTotal;

            if (tipoLiquidacion.Contains("Normal") || tipoLiquidacion.Contains("Aguinaldo"))
            {
                decimal jubilacion = baseCalculo * TASA_JUBILACION;
                recibo.Detalle.Add(new ItemRecibo { Concepto = "Jubilación (11%)", Tipo = "Descuento", Monto = jubilacion });

                decimal obraSocial = baseCalculo * TASA_OBRA_SOCIAL;
                recibo.Detalle.Add(new ItemRecibo { Concepto = "Obra Social (3%)", Tipo = "Descuento", Monto = obraSocial });

                decimal ley19032 = baseCalculo * TASA_PAMI;
                recibo.Detalle.Add(new ItemRecibo { Concepto = "Ley 19.032 (3%)", Tipo = "Descuento", Monto = ley19032 });
            }
        }
    }
}