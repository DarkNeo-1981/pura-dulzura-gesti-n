using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using ENTITY;
using MAPPER;

namespace UI
{
    public partial class LiquidacionSueldos : Form
    {
        private LiquidacionBLL _liquidacionBLL;
        private EmpleadoServiceBLL _empleadoServiceBLL;
        private List<EmpleadoBase> _listaEmpleados;
        internal ENTITY.Usuarios usuario;

        public LiquidacionSueldos(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            this.usuario = pUsuario;
            _liquidacionBLL = new LiquidacionBLL();
            _empleadoServiceBLL = new EmpleadoServiceBLL();
            CargarEmpleados();
            CargarTiposLiquidacion();
            ConfigurarDGVRecibo();
            cmbProcesoTipo_SelectedIndexChanged(null, null);
        }

        // ==========================================================
        //  MÉTODOS DE CONFIGURACIÓN Y CARGA DE DATOS 
        // ==========================================================
        private void CargarEmpleados()
        {
            try
            {
                _listaEmpleados = _empleadoServiceBLL.ObtenerTodosLosEmpleados();

                if (_listaEmpleados != null && _listaEmpleados.Any())
                {
                    // ComboBox para Legajo (usado en Liquidación Individual)
                    cmbLegajo.DataSource = _listaEmpleados.OrderBy(e => e.Legajo).ToList();
                    cmbLegajo.DisplayMember = "Legajo";
                    cmbLegajo.ValueMember = "DNI";
                    cmbLegajo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista de empleados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTiposLiquidacion()
        {
            cmbProcesoTipo.Items.Add("Masiva (Nómina Completa)");
            cmbProcesoTipo.Items.Add("Individual (Un Empleado)");
            cmbProcesoTipo.SelectedIndex = 0;

            cmbTipoLiquidacion.Items.Add("Normal (Mensual)");
            cmbTipoLiquidacion.Items.Add("Aguinaldo (SAC)");
            cmbTipoLiquidacion.Items.Add("Liquidación Final");
            cmbTipoLiquidacion.SelectedIndex = 0;
        }

        private void ConfigurarDGVRecibo()
        {
            dgvReciboDetalle.AutoGenerateColumns = true;
            dgvReciboDetalle.ReadOnly = true;
            dgvReciboDetalle.AllowUserToAddRows = false;
            dgvReciboDetalle.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvReciboDetalle.DefaultCellStyle.ForeColor = Color.Black;
            dgvReciboDetalle.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 50, 20);
            dgvReciboDetalle.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReciboDetalle.EnableHeadersVisualStyles = false;
        }

        // ==========================================================
        //  MÉTODO CORREGIDO: Obtiene la Configuración de Correo
        // ==========================================================
        private ConfiguracionMailDTO ObtenerConfiguracionMail()
        {
            // 1. RECARGAR LA CONFIGURACIÓN:
            Properties.Settings.Default.Reload();

            // 2. Leer los valores recargados
            var settings = Properties.Settings.Default;

            // 3. Mapear y retornar            
            return new ConfiguracionMailDTO
            {
                Host = settings.SmtpHost,
                Puerto = settings.SmtpPort,
                EmailRemitente = settings.SmtpEmail,
                Password = settings.SmtpPasswordEncrypted,
                UsaSSL = settings.SmtpEnableSsl
            };
        }

        // ==========================================================
        //  MANEJADORES DE EVENTOS 
        // ==========================================================
        private void cmbProcesoTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool esIndividual = cmbProcesoTipo.Text.Contains("Individual");

            cmbLegajo.Enabled = esIndividual;
            lblLegajo.Enabled = esIndividual;

            if (esIndividual)
            {
                cmbLegajo.Focus();
            }
            else
            {
                dtpPeriodo.Focus();
            }

            dgvReciboDetalle.DataSource = null;
            lblEstadoProceso.Text = string.Empty;
        }

        private void btnLiquidar_Click(object sender, EventArgs e)
        {
            DateTime periodo = dtpPeriodo.Value;
            string tipoLiquidacion = cmbTipoLiquidacion.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tipoLiquidacion))
            {
                MessageBox.Show("Debe seleccionar el Tipo de Liquidación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbProcesoTipo.Text.Contains("Masiva"))
            {
                LiquidarMasivo(periodo, tipoLiquidacion);
            }
            else if (cmbProcesoTipo.Text.Contains("Individual"))
            {
                LiquidarIndividual(periodo, tipoLiquidacion);
            }
        }

        // ==========================================================
        //  MÉTODOS DE EJECUCIÓN (Pasando 'config')
        // ==========================================================

        // --- LIQUIDACIÓN MASIVA ---
        private void LiquidarMasivo(DateTime periodo, string tipoLiquidacion)
        {
            lblEstadoProceso.Text = $"Iniciando Liquidación Masiva ({tipoLiquidacion}) para {periodo:MM/yyyy}... Esto puede tardar.";
            dgvReciboDetalle.DataSource = null;

            btnLiquidar.Enabled = false;

            try
            {
                // 1. OBTENER CONFIGURACIÓN
                ConfiguracionMailDTO config = ObtenerConfiguracionMail();

                // 2. Llamada a BLL, pasando la configuración
                _liquidacionBLL.ProcesarLiquidacionMensual(
                    periodo,
                    tipoLiquidacion,
                    config
                );
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se realizó una liquidacion masiva de haberes({fecha})");
                lblEstadoProceso.Text = $"Liquidación Masiva finalizada con éxito para {periodo:MM/yyyy}. (Recibos ENVIADOS por email)";
                MessageBox.Show("Liquidación Masiva finalizada con éxito. Recibos enviados por email.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblEstadoProceso.Text = $"ERROR CRÍTICO: La liquidación no pudo finalizar. {ex.Message}";
                MessageBox.Show($"Error en la Liquidación Masiva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLiquidar.Enabled = true;
            }
        }

        // --- LIQUIDACIÓN INDIVIDUAL ---
        private void LiquidarIndividual(DateTime periodo, string tipoLiquidacion)
        {
            if (cmbLegajo.SelectedIndex == -1 || cmbLegajo.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un empleado para la liquidación individual.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLegajo.Focus();
                return;
            }

            int dniEmpleado = Convert.ToInt32(cmbLegajo.SelectedValue);

            // Normalizar el período al primer día del mes para coincidir con la DAL
            DateTime periodoNormalizado = new DateTime(periodo.Year, periodo.Month, 1);

            try
            {
                lblEstadoProceso.Text = $"Liquidando DNI {dniEmpleado} ({tipoLiquidacion}) para {periodoNormalizado:MM/yyyy}... (Se enviará email)";

                // 1. OBTENER CONFIGURACIÓN
                ConfiguracionMailDTO config = ObtenerConfiguracionMail();

                // 2. LLAMADA A LA BLL: Llama al nuevo método, pasando la configuración
                ReciboDeSueldo recibo = _liquidacionBLL.LiquidarYGuardarYEnviarRecibo(
                    dniEmpleado,
                    periodoNormalizado,
                    tipoLiquidacion,
                    config
                );

                CargarDetalleReciboEnGrilla(recibo);
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se realizó una liquidacion individual de haberes({fecha})");
                lblEstadoProceso.Text = $"Recibo generado, guardado y ENVIADO para DNI {dniEmpleado}. Neto: ${recibo.NetoAPagar:N2}";
            }
            catch (Exception ex)
            {
                lblEstadoProceso.Text = $"ERROR al liquidar el DNI {dniEmpleado}: {ex.Message}";
                MessageBox.Show($"Error al liquidar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvReciboDetalle.DataSource = null;
            }
        }

        // ==========================================================
        //  MÉTODO AUXILIAR DE LA GRILLA 
        // ==========================================================
        private void CargarDetalleReciboEnGrilla(ReciboDeSueldo recibo)
        {
            if (recibo.Detalle != null && recibo.Detalle.Any())
            {
                dgvReciboDetalle.DataSource = recibo.Detalle;

                if (dgvReciboDetalle.Columns.Contains("Monto"))
                {
                    dgvReciboDetalle.Columns["Monto"].DefaultCellStyle.Format = "C2";
                    dgvReciboDetalle.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            else
            {
                dgvReciboDetalle.DataSource = null;
            }
        }

        private void LiquidacionSueldos_Load(object sender, EventArgs e)
        {

        }
    }
}

