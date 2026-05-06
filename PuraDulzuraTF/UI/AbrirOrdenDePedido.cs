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
    public partial class AbrirOrdenDePedido : Form
    {
        private List<DetalleOrdenDePedido> detalles = new List<DetalleOrdenDePedido>();
        private List<Producto> productosDisponibles = new List<Producto>();
        bool validar = false;
        UI.Menu Principal;

        public AbrirOrdenDePedido()
        {
            InitializeComponent();
        }

        private void AbrirOrdenDePedido_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            var bll = new BLL.ProductoBLL();
            productosDisponibles = bll.ObtenerTodosLosProductos()
                                      .Where(p => p.ProductoActivo)
                                      .OrderBy(p => p.Nombre)
                                      .ToList();

            cmbProducto.DataSource = productosDisponibles;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ID";

            CargarClientesYVendedores();
            dgvDetallePedido.EnableHeadersVisualStyles = false; // Importante
            dgvDetallePedido.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvDetallePedido.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetallePedido.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvDetallePedido.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetallePedido.EnableHeadersVisualStyles = false;
            dgvDetallePedido.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void CargarClientesYVendedores()
        {
            var clienteBLL = new ClienteBLL();
            var listaClientes = clienteBLL.Buscar_Todos_Lista(false);

            cmbDniCliente.DataSource = listaClientes;
            cmbDniCliente.DisplayMember = "NombreCompleto";
            cmbDniCliente.ValueMember = "Dni"; 
            cmbDniCliente.SelectedIndex = -1;

            var usuarioBLL = new UsuariosBLL();
            var listaVendedores = usuarioBLL.BuscarTodos()
                                            .Where(u => u.Permiso.Id == 4)
                                            .ToList();

            cmbDniVendedor.DataSource = listaVendedores;
            cmbDniVendedor.DisplayMember = "NombreCompleto"; // muestra el nombre de usuario
            cmbDniVendedor.ValueMember = "DNI";
            cmbDniVendedor.SelectedIndex = -1;
        }


        private void CalcularPrecioTotal()
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad) &&
                decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario))
            {
                decimal total = cantidad * precioUnitario;
                txtPrecioTotal.Text = total.ToString("0.00");
            }
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            CalcularPrecioTotal();
        }

        private void txtPrecioTotal_TextChanged(object sender, EventArgs e)
        {
            CalcularPrecioTotal();
        }

        private void txtPrecioUnitario_TextChanged(object sender, EventArgs e)
        {
            CalcularPrecioTotal();
        }

        private void btnAgregarPedido_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem == null ||
                !int.TryParse(txtCantidad.Text, out int cantidad) ||
                !decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario))                
            {
                MessageBox.Show("Datos inválidos del producto.");
                return;
            }
            else
            {
                validar = true;
            }

                int idProducto = (int)cmbProducto.SelectedValue;

            var detalle = new DetalleOrdenDePedido
            {
                IdProducto = idProducto,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario
            };

            detalles.Add(detalle);
            ActualizarGrilla();
            LimpiarCamposDetalle();
        }

        private void LimpiarCamposDetalle()
        {
            cmbProducto.SelectedIndex = -1;
            txtCantidad.Clear();
            txtPrecioUnitario.Clear();
            txtPrecioTotal.Clear();
        }

        private void ActualizarGrilla()
        {
            dgvDetallePedido.DataSource = null;
            dgvDetallePedido.DataSource = detalles.Select(d => new
            {
                Producto = productosDisponibles.FirstOrDefault(p => p.ID == d.IdProducto)?.Nombre ?? "N/D",
                d.Cantidad,
                d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList();
        }

        private void btnConfirmarPedido_Click(object sender, EventArgs e)
        {
            if (cmbDniCliente.SelectedIndex == -1 || cmbDniVendedor.SelectedIndex == -1 || detalles.Count == 0)
            {
                MessageBox.Show("Faltan datos para guardar el pedido.");
                return;
            }

            var pedido = new ENTITY.OrdenDePedido
            {
                DNI_Cliente = int.Parse(cmbDniCliente.SelectedValue.ToString()),
                DNI_Vendedor = Convert.ToInt32(cmbDniVendedor.SelectedValue),
                FechaDeVenta = dtpFechaVenta.Value.ToShortDateString(),
                Total = detalles.Sum(d => d.Subtotal),
                Eliminado = false,
                Detalles = detalles
            };

            var bll = new BLL.OrdenDePedidoBLL();
            if (bll.Agregar(pedido))
            {
                string resumen = $"Pedido guardado correctamente.\n\n" +
                                 $"Fecha: {pedido.FechaDeVenta}\n" +
                                 $"Cantidad de productos: {detalles.Count}\n" +
                                 $"Total de la venta: ${pedido.Total:0.00}";

                MessageBox.Show(resumen, "Resumen del Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Principal.AgregarALaBitacora("Se ha agregado un nuevo pedido.");
                validar = false;
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("Error al guardar el pedido.");
            }
        }

        private void LimpiarFormulario()
        {
            dtpFechaVenta.Value = DateTime.Today;
            cmbDniCliente.SelectedIndex = -1;
            cmbDniVendedor.SelectedIndex = -1;
            detalles.Clear();
            ActualizarGrilla();
            LimpiarCamposDetalle();
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem is Producto seleccionado)
            {
                txtPrecioUnitario.Text = seleccionado.PrecioDeVenta.ToString("0.00");
                CalcularPrecioTotal();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (validar == false)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Debe conirmar los pedidos agregados.","Atención",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void cmbDniVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
