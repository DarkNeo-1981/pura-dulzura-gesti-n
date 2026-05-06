using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using ENTITY;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UI
{
    public partial class BuscarOrdenDePedido : Form
    {
        private OrdenDePedidoBLL ordenBLL = new OrdenDePedidoBLL();
        private List<OrdenDePedido> listaOriginal = new List<OrdenDePedido>();
        Menu Principal;

        public BuscarOrdenDePedido()
        {
            InitializeComponent();
        }

        private void BuscarOrdenDePedido_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            listaOriginal = ordenBLL.ObtenerTodos();
            dgvOrdenes.DataSource = listaOriginal;
            if (dgvOrdenes.Columns.Contains("Cobrada") && dgvOrdenes.Columns.Contains("Facturada"))
            {
                dgvOrdenes.Columns["Cobrada"].DisplayIndex = dgvOrdenes.Columns["Facturada"].DisplayIndex - 1;
            }            
            if (dgvOrdenes.Columns.Contains("Eliminado"))
                dgvOrdenes.Columns["Eliminado"].Visible = false;
            rbTodos.Checked = true;

            rbTodos.CheckedChanged += new EventHandler(rbFiltro_CheckedChanged);
            rbPorId.CheckedChanged += new EventHandler(rbFiltro_CheckedChanged);
            rbPorDNICliente.CheckedChanged += new EventHandler(rbFiltro_CheckedChanged);
            rbPorDNIVendedor.CheckedChanged += new EventHandler(rbFiltro_CheckedChanged);

            dgvOrdenes.EnableHeadersVisualStyles = false; // Importante
            dgvOrdenes.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrdenes.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvOrdenes.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrdenes.EnableHeadersVisualStyles = false;
            dgvOrdenes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void AplicarFiltro()
        {
            string filtro = txtFiltro.Text.Trim().ToLower();
            List<OrdenDePedido> listaFiltrada = new List<OrdenDePedido>(listaOriginal);

            if (rbPorId.Checked && int.TryParse(filtro, out int id))
            {
                listaFiltrada = listaFiltrada.Where(o => o.Id == id).ToList();
            }
            else if (rbPorDNICliente.Checked && int.TryParse(filtro, out int dniCliente))
            {
                listaFiltrada = listaFiltrada.Where(o => o.DNI_Cliente == dniCliente).ToList();
            }
            else if (rbPorDNIVendedor.Checked && int.TryParse(filtro, out int dniVendedor))
            {
                listaFiltrada = listaFiltrada.Where(o => o.DNI_Vendedor == dniVendedor).ToList();
            }
            // Si está seleccionado "Todos" o no hay filtro válido, lista completa

            dgvOrdenes.DataSource = null;
            dgvOrdenes.DataSource = listaFiltrada;

            
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void rbFiltro_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEmitirFactura_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden de pedido para emitir factura.", "Atención");
                return;
            }

            // Obtener la orden seleccionada de la grilla (asegúrate de que el DataBoundItem sea OrdenDePedido)
            var ordenSeleccionadaGrilla = (OrdenDePedido)dgvOrdenes.SelectedRows[0].DataBoundItem;

            OrdenDePedidoBLL ordenBLL = new OrdenDePedidoBLL();

            // Obtener la orden completa desde el DAL para asegurarnos de tener el estado 'Facturada' más reciente
            var ordenActual = ordenBLL.BuscarCompleto(ordenSeleccionadaGrilla.Id);

            if (ordenActual == null)
            {
                MessageBox.Show("No se pudo cargar la información completa de la orden.", "Error");
                return;
            }

            if (!ordenActual.Cobrada)
            {
                MessageBox.Show("Debe cobrar la orden antes de emitir la factura.", "Atención");
                return;
            }

            // *** CAMBIO CLAVE 14: Validar si la orden ya está facturada ***
            if (ordenActual.Facturada)
            {
                MessageBox.Show($"La orden de pedido Nro. {ordenActual.Id} ya ha sido facturada.", "Orden ya Facturada");
                Principal.AgregarALaBitacora($"Se emitio la facrura de la orden {ordenActual.Id}");
                return;
            }

            // Si la orden no está facturada, procedemos
            if (ordenActual.Detalles == null || ordenActual.Detalles.Count == 0)
            {
                MessageBox.Show("La orden seleccionada no tiene detalles cargados.", "Error");
                return;
            }

            FacturaHelper helper = new FacturaHelper();
            helper.GenerarFacturaPDF(ordenActual); // Usar la orden actual con todos los detalles

            // *** CAMBIO CLAVE 15: Marcar la orden como facturada después de emitir la factura ***
            bool marcado = ordenBLL.MarcarComoFacturada(ordenActual.Id);
            if (marcado)
            {
                MessageBox.Show($"Orden de pedido Nro. {ordenActual.Id} marcada como facturada.", "Información");                
                CargarOrdenesEnGrilla(); // Llama a tu método para actualizar la grilla
            }
            else
            {
                MessageBox.Show("Error al marcar la orden como facturada en el XML.", "Error");
            }
        }

        private void CargarOrdenesEnGrilla()
        {
            OrdenDePedidoBLL ordenBLL = new OrdenDePedidoBLL();
            dgvOrdenes.DataSource = ordenBLL.ObtenerTodos(); 
            if (dgvOrdenes.Columns.Contains("Cobrada") && dgvOrdenes.Columns.Contains("Facturada"))
            {
                dgvOrdenes.Columns["Cobrada"].DisplayIndex = dgvOrdenes.Columns["Facturada"].DisplayIndex - 1;
            }
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden de pedido para cobrar.", "Atención");
                return;
            }

            var ordenSeleccionada = (OrdenDePedido)dgvOrdenes.SelectedRows[0].DataBoundItem;

            // Traer la orden completa
            var ordenActual = ordenBLL.BuscarCompleto(ordenSeleccionada.Id);
            if (ordenActual == null)
            {
                MessageBox.Show("No se pudo cargar la información completa de la orden.", "Error");
                return;
            }

            if (ordenActual.Facturada)
            {
                MessageBox.Show($"La orden de pedido Nro. {ordenActual.Id} ya fue facturada, no puede cobrarse nuevamente.", "Atención");
                return;
            }

            if (ordenActual.Cobrada)
            {
                MessageBox.Show($"La orden de pedido Nro. {ordenActual.Id} ya está cobrada.", "Información");
                return;
            }
                        
            DialogResult dr = MessageBox.Show("¿Confirma el cobro de la orden?", "Confirmación", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                bool marcado = ordenBLL.MarcarComoCobrada(ordenActual.Id);
                if (marcado)
                {
                    MessageBox.Show($"Orden de pedido Nro. {ordenActual.Id} cobrada exitosamente.", "Información");
                    Principal.AgregarALaBitacora($"Se realizo el cobro de la orden {ordenActual.Id}");
                    CargarOrdenesEnGrilla();
                }
                else
                {
                    MessageBox.Show("Error al marcar la orden como cobrada en el XML.", "Error");
                }
            }
        }
    }
}
