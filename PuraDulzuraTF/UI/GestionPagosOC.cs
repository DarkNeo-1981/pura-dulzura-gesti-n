using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace UI
{
    public partial class GestionPagosOC : Form
    {
        UI.Menu Principal;
        private OrdenCompraBLL _ordenCompraBLL;

        public GestionPagosOC(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            _ordenCompraBLL = new OrdenCompraBLL();
            this.btnPagarOrden.Click += new System.EventHandler(this.btnPagarOrden_Click);
            this.dgvOrdenesCompras.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgvOrdenesCompras_DataBindingComplete);

        }

        private void GestionPagosOC_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            CargarTodasLasOrdenes();            
        }

        private void ConfigurarDGV()
        {
            // Asegura que las columnas existan antes de configurar
            if (dgvOrdenesCompras.Columns.Count == 0) return;

            // --- Formato de Fecha y Hora ---
            if (dgvOrdenesCompras.Columns.Contains("FechaEmision"))
            {
                dgvOrdenesCompras.Columns["FechaEmision"].HeaderText = "Fecha Emisión";
                // Muestra la fecha completa y la hora
                dgvOrdenesCompras.Columns["FechaEmision"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvOrdenesCompras.Columns["FechaEmision"].Width = 130;
            }

            // --- Formato de Moneda (Total) ---
            if (dgvOrdenesCompras.Columns.Contains("Total"))
            {
                dgvOrdenesCompras.Columns["Total"].HeaderText = "Total OC";
                // Formato de moneda (C2) con alineación a la derecha
                dgvOrdenesCompras.Columns["Total"].DefaultCellStyle.Format = "C2";
                dgvOrdenesCompras.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvOrdenesCompras.Columns["Total"].Width = 100;
            }

            // --- Ajustes de Columnas de Estado ---
            if (dgvOrdenesCompras.Columns.Contains("Estado"))
            {
                dgvOrdenesCompras.Columns["Estado"].Width = 80;
            }
            if (dgvOrdenesCompras.Columns.Contains("EstadoPago"))
            {
                dgvOrdenesCompras.Columns["EstadoPago"].HeaderText = "Estado Pago";
                dgvOrdenesCompras.Columns["EstadoPago"].Width = 120;
            }

            // Ocultar ID, si lo cargamos
            if (dgvOrdenesCompras.Columns.Contains("Id"))
                dgvOrdenesCompras.Columns["Id"].Visible = false;

            // Autoajuste de las filas para llenar el espacio
            dgvOrdenesCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenesCompras.Columns["FechaEmision"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void CargarTodasLasOrdenes()
        {
            try
            {
                // 1. Obtener TODAS las órdenes de la BLL, sin filtrar
                DataTable dtCompleta = _ordenCompraBLL.TraerTodasOrdenes();                

                // 2. Asignar los datos completos al DataGridView
                dgvOrdenesCompras.DataSource = dtCompleta;

                // Aplicar el formato
                ConfigurarDGV();               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las órdenes de compra: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilasPorEstadoPago()
        {
            // Asegura que hay datos
            if (dgvOrdenesCompras.Rows.Count == 0) return;

            // Itera sobre cada fila del DGV
            foreach (DataGridViewRow row in dgvOrdenesCompras.Rows)
            {
                // Asegúrate de que la fila no sea la de encabezado o una fila nueva vacía
                if (row.IsNewRow || row.Cells["EstadoPago"].Value == null) continue;

                string estadoPago = row.Cells["EstadoPago"].Value.ToString().ToUpper();

                // Define los colores según el estado
                if (estadoPago == "PAGADA")
                {
                    // Pagado: Fondo verde claro para indicar finalización
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    // Deshabilita la selección de esta fila para evitar pagar de nuevo
                    row.ReadOnly = true;
                }
                else if (estadoPago == "PENDIENTE DE PAGO")
                {
                    // Pendiente: Fondo amarillo claro para indicar acción requerida
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                    row.ReadOnly = false;
                }
                else if (estadoPago == "CANCELADA")
                {
                    // Cancelada: Fondo gris para indicar que no aplica
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.ReadOnly = true;
                }
                else if (estadoPago == "PAGO PARCIAL")
                {
                    // Cancelada: Fondo naranja para indicar que no aplica
                    row.DefaultCellStyle.BackColor = Color.Orange;
                    row.ReadOnly = true;
                }
                else if (estadoPago == "DEVOLUCION")
                {
                    // Cancelada: Fondo rojo para indicar que no aplica
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.ReadOnly = true;
                }
                
            }
        }

        private void dgvOrdenesCompras_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
            ColorearFilasPorEstadoPago();                      
            dgvOrdenesCompras.ClearSelection();            
        }

        private void btnPagarOrden_Click(object sender, EventArgs e)
        {
            // A. Validación: Verificar que hay una fila seleccionada
            if (dgvOrdenesCompras.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una orden de compra para procesar el pago.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }            

            // B. Obtener el ID de la fila seleccionada
            int idOrdenSeleccionada;

            // El DataBoundItem es un DataRowView si la fuente es un DataTable
            DataRowView drv = (DataRowView)dgvOrdenesCompras.SelectedRows[0].DataBoundItem;

            // Intentar parsear el ID de la columna "Id"
            if (drv == null || !int.TryParse(drv["Id"].ToString(), out idOrdenSeleccionada))
            {
                MessageBox.Show("Error al obtener el ID de la orden seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ⭐️ VALIDACION DEL ESTADO DE PAGO ACTUAL
            string estadoPagoActual = drv["EstadoPago"].ToString().ToUpper();

            // ⭐️ REGLA DE NEGOCIO: Solo se puede PAGAR si está PENDIENTE DE PAGO o PAGO PARCIAL
            if (estadoPagoActual != "PENDIENTE DE PAGO" && estadoPagoActual != "PAGO PARCIAL")
            {
                MessageBox.Show($"La Orden ID {idOrdenSeleccionada} no puede ser pagada. Su estado actual es: {estadoPagoActual}.", "Validación de Estado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; // Detiene la ejecución si el estado no es válido
            }

            // C. Confirmación del usuario
            DialogResult confirm = MessageBox.Show($"¿Confirmas el pago de la Orden ID {idOrdenSeleccionada} por un total de {drv["Total"]:C2}?", "Confirmación de Pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
            {
                return;
            }

            // D. Llamada a la Lógica de Negocio
            try
            {
                bool pagoExitoso = _ordenCompraBLL.PagarOrden(idOrdenSeleccionada);

                if (pagoExitoso)
                {
                    MessageBox.Show($"Orden ID {idOrdenSeleccionada} marcada como PAGADA exitosamente.", "Pago Procesado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Principal.AgregarALaBitacora($"Se procesó el pago de la orden: {idOrdenSeleccionada} por un total de {drv["Total"]:C2}?");
                    // ⭐️ LLAMAR AL NUEVO MÉTODO PARA RECARGAR TODA LA LISTA Y COLOREAR
                    CargarTodasLasOrdenes();
                    dgvOrdenesCompras.ClearSelection();
                    this.Focus();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error durante el pago: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmi_CambiarEstadoPago_Click(object sender, EventArgs e)
        {
            // A. Obtener el ítem del menú que fue clickeado
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (dgvOrdenesCompras.SelectedRows.Count == 0) return;

            // B. Obtener el ID de la orden seleccionada
            int idOrdenSeleccionada;
            DataRowView drv = (DataRowView)dgvOrdenesCompras.SelectedRows[0].DataBoundItem;

            if (drv == null || !int.TryParse(drv["Id"].ToString(), out idOrdenSeleccionada))
            {
                MessageBox.Show("Error al obtener el ID de la orden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ⭐️ 1. OBTENER EL ESTADO ACTUAL
            // Es vital saber en qué estado está antes de permitir cambiarlo
            string estadoActual = drv["EstadoPago"].ToString().ToUpper();

            // ⭐️ 2. VALIDACIÓN DE BLOQUEO            
            if (estadoActual == "CANCELADA" || estadoActual == "DEVOLUCION")
            {
                MessageBox.Show($"No es posible cambiar el estado de una orden que está {estadoActual}.\n\nEsta operación es definitiva.",
                                "Operación No Permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            // C. El estado se toma directamente del texto del ítem del menú
            string nuevoEstado = menuItem.Text.ToUpper();

            // Validación extra: Si intentan cambiar al MISMO estado que ya tiene
            if (estadoActual == nuevoEstado)
            {
                return; // No hacemos nada si el estado es el mismo
            }

            // D. Confirmación y Llamada a la BLL
            DialogResult confirm = MessageBox.Show($"¿Deseas cambiar el estado de la Orden ID {idOrdenSeleccionada}?\n\nEstado Actual: {estadoActual}\nNuevo Estado: {nuevoEstado}",
                                                   "Confirmar Cambio de Estado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (_ordenCompraBLL.ActualizarEstadoPago(idOrdenSeleccionada, nuevoEstado))
                    {
                        MessageBox.Show($"Orden ID {idOrdenSeleccionada} actualizada a {nuevoEstado}.", "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Principal.AgregarALaBitacora($"Se actualizó el estado de la orden: {idOrdenSeleccionada} al estado {nuevoEstado}.");
                        // ⭐️ Recargar los datos y colores
                        CargarTodasLasOrdenes();
                        dgvOrdenesCompras.ClearSelection();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
