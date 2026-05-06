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
    public partial class ProductoProveedor : Form
    {
        private ProductoProveedorBLL _productoBLL;        
        private ENTITY.ProductoProveedor _productoSeleccionado;

        public ProductoProveedor()
        {
            InitializeComponent();
            _productoBLL = new ProductoProveedorBLL();
        }

        private void ProductoProveedor_Load(object sender, EventArgs e)
        {
            ConfigurarDGV();
            CargarGrilla();
            LimpiarControles();
        }

        private void ConfigurarDGV()
        {
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
        }

        private void CargarGrilla()
        {
            try
            {
                DataTable dt = _productoBLL.TraerTodos(false);
                dgvProductos.DataSource = dt;

                if (dgvProductos.Columns.Contains("Id")) dgvProductos.Columns["Id"].Visible = false;
                if (dgvProductos.Columns.Contains("Eliminado")) dgvProductos.Columns["Eliminado"].Visible = false;

                if (dgvProductos.Columns.Contains("PrecioReferencia"))
                {
                    dgvProductos.Columns["PrecioReferencia"].HeaderText = "Precio Ref.";
                    dgvProductos.Columns["PrecioReferencia"].DefaultCellStyle.Format = "C2";
                }

                if (dgvProductos.Columns.Contains("EstaActivo"))
                {
                    dgvProductos.Columns["EstaActivo"].HeaderText = "Activo";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la grilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarControles()
        {
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            chkActivo.Checked = true;
            _productoSeleccionado = null;

            btnGuardar.Text = "Guardar";
            btnEliminar.Enabled = false;
            dgvProductos.ClearSelection();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal precio = 0;
                if (!string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    if (!decimal.TryParse(txtPrecio.Text, out precio))
                    {
                        MessageBox.Show("El precio debe ser un número válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                ENTITY.ProductoProveedor prod = new ENTITY.ProductoProveedor();
                prod.Nombre = txtNombre.Text.Trim();
                prod.PrecioReferencia = precio;
                prod.EstaActivo = chkActivo.Checked;

                if (_productoSeleccionado == null)
                {
                    _productoBLL.Agregar(prod);
                    MessageBox.Show("Materia prima agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    prod.Id = _productoSeleccionado.Id;
                    _productoBLL.Modificar(prod);
                    MessageBox.Show("Materia prima actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarGrilla();
                LimpiarControles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_productoSeleccionado == null) return;

            DialogResult confirm = MessageBox.Show(
                $"¿Estás seguro de eliminar '{_productoSeleccionado.Nombre}'?",
                "Confirmar Baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _productoBLL.Borrar(_productoSeleccionado.Id);
                    MessageBox.Show("Eliminado correctamente.");
                    CargarGrilla();
                    LimpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        private void DgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataRowView row = (DataRowView)dgvProductos.Rows[e.RowIndex].DataBoundItem;

                if (row != null)
                {
                    _productoSeleccionado = new ENTITY.ProductoProveedor
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nombre = row["Nombre"].ToString(),
                        PrecioReferencia = Convert.ToDecimal(row["PrecioReferencia"]),
                        EstaActivo = Convert.ToBoolean(row["EstaActivo"])
                    };

                    txtNombre.Text = _productoSeleccionado.Nombre;
                    txtPrecio.Text = _productoSeleccionado.PrecioReferencia.ToString("0.00");
                    chkActivo.Checked = _productoSeleccionado.EstaActivo;

                    btnGuardar.Text = "Modificar";
                    btnEliminar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el producto: " + ex.Message);
            }
        }
    }
}
