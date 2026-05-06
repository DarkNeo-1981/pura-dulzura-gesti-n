using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class NovedadesLiquidacionSueldos : Form
    {
        private EmpleadoServiceBLL _empleadoServiceBLL;
        private List<EmpleadoBase> _listaEmpleados;
        private BindingList<Novedad> _novedadesPendientes = new BindingList<Novedad>();
        private LiquidacionBLL _liquidacionBLL = new LiquidacionBLL();
        internal ENTITY.Usuarios usuario;

        public NovedadesLiquidacionSueldos(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            this.usuario = pUsuario;
            dgvNovedades.DataSource = _novedadesPendientes;
            _empleadoServiceBLL = new EmpleadoServiceBLL(); 
            CargarEmpleadosEnCombo();
            CargarTiposNovedad();
            // 1. Configurar el estilo de la CELDA POR DEFECTO
            dgvNovedades.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // Gris muy claro, casi blanco
            dgvNovedades.DefaultCellStyle.ForeColor = Color.Black;                    // Texto negro para contraste

            // 2. Configurar el estilo de la CABECERA (Header)
            dgvNovedades.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 50, 20); // Un marrón más oscuro o un color contrastante
            dgvNovedades.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                // Texto blanco en la cabecera

            // 3. (Opcional) Quitar el color de selección azul
            dgvNovedades.DefaultCellStyle.SelectionBackColor = Color.Orange;
            dgvNovedades.DefaultCellStyle.SelectionForeColor = Color.White;

            // 4. (Importante) Forzar el uso del color de fondo de las cabeceras
            dgvNovedades.EnableHeadersVisualStyles = false;
        }

        private void CargarEmpleadosEnCombo()
        {
            try
            {
                // 1. Obtener todos los empleados activos
                _listaEmpleados = _empleadoServiceBLL.ObtenerTodosLosEmpleados();

                if (_listaEmpleados != null && _listaEmpleados.Any())
                {
                    var listaOrdenada = _listaEmpleados.OrderBy(e => e.Legajo).ToList();

                    // 2. Configurar el ComboBox
                    cmbLegajo.DataSource = listaOrdenada;
                    cmbLegajo.DisplayMember = "Legajo"; // Lo que se muestra al usuario
                    cmbLegajo.ValueMember = "Legajo";   // El valor interno que se usa
                    cmbLegajo.SelectedIndex = -1; // No seleccionar nada al inicio
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista de empleados: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTiposNovedad()
        {            
            cmbTipoNovedad.DataSource = TiposNovedad.ObtenerListaParaUI();
            cmbTipoNovedad.SelectedIndex = -1; // No seleccionar ninguno al inicio
        }

        private void NovedadesLiquidacionSueldos_Load(object sender, EventArgs e)
        {

        }

        private void cmbLegajo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLegajo.SelectedIndex > -1)
            {
                // El ComboBox tiene el objeto EmpleadoBase asociado (gracias a DataSource)
                EmpleadoBase empleadoSeleccionado = cmbLegajo.SelectedItem as EmpleadoBase;

                if (empleadoSeleccionado != null)
                {
                    // 1. Cargar los campos de solo lectura
                    txtDNI.Text = empleadoSeleccionado.DNI.ToString();
                    txtApellido.Text = empleadoSeleccionado.Apellido;
                    txtNombre.Text = empleadoSeleccionado.Nombre;

                    // 2. Enfocar el siguiente campo de entrada
                    cmbTipoNovedad.Focus();
                }
            }
            else // Si se deselecciona el Legajo, limpiar todo
            {
                txtDNI.Clear();
                txtApellido.Clear();
                txtNombre.Clear();
            }
        }

        private void cmbTipoNovedad_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cmbTipoNovedad.SelectedIndex == -1) return;

            string seleccionUI = cmbTipoNovedad.SelectedItem?.ToString();
            string tipoClave = MapearUIToClave(seleccionUI);          

            // Sincronizar el CheckBox
            if (tipoClave == TiposNovedad.Adelanto ||
                tipoClave == TiposNovedad.Ausencia ||
                tipoClave == TiposNovedad.OtrosDescuentos)
            {
                // Si seleccionó un tipo de descuento, marcar el CheckBox
                chkDescuento.Checked = true;
                chkDescuento.Enabled = true; 
            }
            else
            {
                // Si seleccionó un Haber, desmarcarlo
                chkDescuento.Checked = false;
                chkDescuento.Enabled = true; // Para que el usuario decida si es un descuento especial
            }

            txtValor.Clear();

            if (tipoClave == TiposNovedad.HoraExtra)
            {
                // Se esperan cantidades de horas
                lblValorTipo.Text = "Valor (Horas):";
            }
            else if (tipoClave == TiposNovedad.Ausencia)
            {
                // Se esperan cantidades de días
                lblValorTipo.Text = "Valor (Días):";
            }
            else if (tipoClave == TiposNovedad.Premio ||
                     tipoClave == TiposNovedad.Adelanto ||
                     tipoClave == TiposNovedad.OtrosHaberes ||
                     tipoClave == TiposNovedad.OtrosDescuentos)
            {
                // Se esperan montos monetarios
                lblValorTipo.Text = "Valor (Monto $):";
            }
            else
            {
                // Por defecto, si algo falla
                lblValorTipo.Text = "Valor:";
            }

            // Mover el foco al campo Valor, que es el siguiente a llenar
            txtValor.Focus();
        }

        private string MapearUIToClave(string uiText)
        {
            if (uiText == "Horas Extra")
            {
                return TiposNovedad.HoraExtra;
            }
            else if (uiText == "Premio")
            {
                return TiposNovedad.Premio;
            }
            else if (uiText == "Adelanto de Sueldo")
            {
                return TiposNovedad.Adelanto;
            }
            else if (uiText == "Ausencia / Días")
            {
                return TiposNovedad.Ausencia;
            }
            else if (uiText == "Otros Haberes")
            {
                return TiposNovedad.OtrosHaberes;
            }
            else if (uiText == "Otros Descuentos")
            {
                return TiposNovedad.OtrosDescuentos;
            }
            else
            {
                return string.Empty;
            }
        }

        private void chkDescuento_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDescuento.Checked == false)
            {                
                string seleccionUI = cmbTipoNovedad.SelectedItem?.ToString();
                string tipoClave = MapearUIToClave(seleccionUI);               
                if (tipoClave == TiposNovedad.Adelanto ||
                    tipoClave == TiposNovedad.Ausencia ||
                    tipoClave == TiposNovedad.OtrosDescuentos)
                {
                    MessageBox.Show($"La novedad '{seleccionUI}' DEBE ser un descuento. No puede desmarcar esta opción.",
                                    "Error de Novedad", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Forzar el CheckBox a estar tildado nuevamente
                    chkDescuento.Checked = true;
                }
            }
        }

        private void btnAgregarNovedad_Click(object sender, EventArgs e)
        {
            if (cmbLegajo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un empleado (Legajo).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLegajo.Focus();
                return;
            }

            if (cmbTipoNovedad.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar el Tipo de Novedad.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoNovedad.Focus();
                return;
            }

            if (!decimal.TryParse(txtValor.Text, out decimal valorNovedad) || valorNovedad <= 0)
            {
                MessageBox.Show("El campo Valor debe ser un número positivo válido.", "Error de Entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtValor.Focus();
                return;
            }

            string tipoClave = MapearUIToClave(cmbTipoNovedad.SelectedItem.ToString());
            int dniEmpleado = Convert.ToInt32(txtDNI.Text);

            Novedad nuevaNovedad = new Novedad
            {                
                DNI_Empleado = dniEmpleado,                
                Periodo = dtpPeriodo.Value,
                TipoNovedad = tipoClave,
                Valor = valorNovedad,
                EsDescuento = chkDescuento.Checked,
                Observacion = txtObservacion.Text
            };

            _novedadesPendientes.Add(nuevaNovedad);
            LimpiarCamposNovedad();
        }

        private void LimpiarCamposNovedad()
        {
            cmbTipoNovedad.SelectedIndex = -1;
            txtValor.Clear();
            chkDescuento.Checked = false;
            txtObservacion.Clear();
            lblValorTipo.Text = "Valor:";
            cmbTipoNovedad.Focus();
        }

        private void btnGuardarNovedades_Click(object sender, EventArgs e)
        {            
            if (_novedadesPendientes.Count == 0)
            {
                MessageBox.Show("No hay novedades pendientes en la lista para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int registrosGuardados = 0;
            int registrosFallidos = 0;

            /* IMPORTANTE: Se crea una lista auxiliar con los ítems a guardar para 
               evitar un error de enumeración, ya que se va a MODIFICAR (_novedadesPendientes) 
               mientras se itera
            .*/

            List<Novedad> novedadesAGuardar = _novedadesPendientes.ToList();

            foreach (var novedad in novedadesAGuardar)
            {
                try
                {                    
                    _liquidacionBLL.AgregarNovedad(novedad);

                    // Si el guardado fue exitoso, la elimino de la lista temporal
                    // Como es un BindingList, el DGV se refresca solo.
                    _novedadesPendientes.Remove(novedad);
                    registrosGuardados++;
                }
                catch (Exception ex)
                {
                    // Si falla, el registro queda en la lista para que el usuario pueda corregirlo
                    registrosFallidos++;
                                        
                    MessageBox.Show($"Error al guardar la novedad para DNI {novedad.DNI_Empleado} ({novedad.TipoNovedad}): {ex.Message}",
                                    "Error de Persistencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 3. Feedback al Usuario
            if (registrosGuardados > 0 || registrosFallidos > 0)
            {
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se actualizaron las novedades ({fecha})");
                MessageBox.Show($"Proceso de Guardado Finalizado:\n\n" +
                                $"{registrosGuardados} Novedades guardadas con éxito.\n" +
                                $"{registrosFallidos} Novedades con error (permanecen en la lista).",
                                "Guardado Completo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
