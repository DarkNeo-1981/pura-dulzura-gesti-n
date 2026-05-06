using DAL;
using ENTITY;
using Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace BLL
{
    public class LiquidacionBLL
    {
        private readonly EmpleadoServiceBLL _empleadoServiceBLL;
        private readonly NovedadDAL _novedadDAL;
        private readonly ReciboDeSueldoDAL _reciboDAL;
        private readonly LiquidadorDeHaberesBLL _liquidador;
        private readonly EmailManager _emailManager;

        public LiquidacionBLL()
        {
            _empleadoServiceBLL = new EmpleadoServiceBLL();
            _novedadDAL = new NovedadDAL();
            _reciboDAL = new ReciboDeSueldoDAL();
            _liquidador = new LiquidadorDeHaberesBLL();
            _emailManager = new EmailManager();
        }

        // =================================================================
        // MÉTODO DE PROCESO BATCH (Liquidación de toda la nómina)
        // =================================================================
        public void ProcesarLiquidacionMensual(DateTime periodo, string tipoLiquidacion, ConfiguracionMailDTO config)
        {
            DateTime periodoNormalizado = new DateTime(periodo.Year, periodo.Month, 1);
            var empleadosActivos = _empleadoServiceBLL.ObtenerTodosLosEmpleados();

            if (empleadosActivos == null || empleadosActivos.Count == 0)
                throw new Exception("No hay empleados activos para liquidar.");

            foreach (var empleado in empleadosActivos)
            {
                try
                {
                    // Se pasa la configuración al método individual
                    LiquidarYGuardarYEnviarRecibo(empleado.DNI, periodoNormalizado, tipoLiquidacion, config);
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("ya existe"))
                {
                    // Manejo de recibo duplicado.
                }
                catch (Exception ex)
                {
                    // Loguear error de liquidación individual y continuar.
                    Console.WriteLine($"Error liquidando DNI {empleado.DNI}: {ex.Message}");
                }
            }
        }

        // =================================================================
        // MÉTODO INDIVIDUAL (Liquidación, Guardado y Envío de Correo)
        // =================================================================
        public ReciboDeSueldo LiquidarYGuardarYEnviarRecibo(int dniEmpleado, DateTime periodo, string tipoLiquidacion, ConfiguracionMailDTO config)
        {
            string rutaPdfTemporal = string.Empty;
            DateTime periodoNormalizado = new DateTime(periodo.Year, periodo.Month, 1);

            // 1. Obtener el empleado
            var empleado = _empleadoServiceBLL.ObtenerEmpleadoPorDni(dniEmpleado);

            // Si el empleado no se encuentra, la excepción es correcta.
            if (empleado == null)
                throw new InvalidOperationException($"No se encontró un empleado con DNI: {dniEmpleado}.");

            // Si el empleado no tiene Email, se avisa.
            if (string.IsNullOrEmpty(empleado.Email))
                throw new Exception($"No se puede enviar el recibo. El empleado {dniEmpleado} no tiene un email registrado.");

            try
            {
                // 2. Verificar duplicados
                if (_reciboDAL.ExisteReciboParaPeriodo(dniEmpleado, periodoNormalizado))
                    throw new InvalidOperationException($"El recibo para el DNI {dniEmpleado} y período {periodoNormalizado:MM/yyyy} ya existe.");

                // 3. Obtener novedades (asegurando lista no nula)
                var novedadesDelMes = _novedadDAL.ObtenerNovedadesPorDniYPeriodo(dniEmpleado, periodoNormalizado)
                                                 ?? new List<Novedad>();

                // 4. Calcular liquidación
                var recibo = _liquidador.LiquidarMes(empleado, novedadesDelMes, periodoNormalizado, tipoLiquidacion)
                                         ?? new ReciboDeSueldo();

                // 5. Asignar empleado y DNI al recibo
                // Lógica de inicialización dummy
                recibo.Empleado = empleado ?? new EmpleadoBase
                {
                    DNI = dniEmpleado,
                    Categoria = string.Empty,
                    CUIL = string.Empty
                };
                recibo.DNI_Empleado = empleado.DNI;

                // 6. Asegurar campos obligatorios (incluyendo el blindaje de Detalle)
                recibo.FechaEmision = recibo.FechaEmision == default ? DateTime.Now : recibo.FechaEmision;
                recibo.Periodo = periodoNormalizado;
                if (recibo.Detalle == null)
                    recibo.Detalle = new List<ItemRecibo>();

                // Blindaje del Detalle
                foreach (var item in recibo.Detalle)
                {
                    if (item.Concepto == null)
                        item.Concepto = "SIN CONCEPTO";

                    if (item.Tipo == null)
                        item.Tipo = "Haber";
                }

                // 7. Generar PDF
                rutaPdfTemporal = PdfGenerator.GenerarReciboPDF(recibo);

                // 8. Guardar en DAL
                _reciboDAL.GuardarRecibo(recibo);

                // 9. Enviar por email. Se usa el objeto 'config' que fue pasado como parámetro.
                _emailManager.EnviarReciboPorEmail(config, recibo, rutaPdfTemporal, empleado.Email);

                return recibo;
            }
            catch (Exception ex)
            {
                // Limpiar archivo temporal si existe
                if (!string.IsNullOrEmpty(rutaPdfTemporal) && File.Exists(rutaPdfTemporal))
                    File.Delete(rutaPdfTemporal);

                // Propagamos el error
                throw new Exception($"Error en liquidación/envío para DNI {dniEmpleado}: {ex.Message}");
            }
        }

        // ===============================
        // MÉTODOS DE SOPORTE Y CONSULTA
        // ===============================
        public void AgregarNovedad(Novedad novedad)
        {
            _novedadDAL.AgregarNovedad(novedad);
        }

        public List<ReciboDeSueldo> ObtenerRecibosPorDni(int dniEmpleado)
        {
            return _reciboDAL.ObtenerRecibosPorDni(dniEmpleado);
        }
    }
}