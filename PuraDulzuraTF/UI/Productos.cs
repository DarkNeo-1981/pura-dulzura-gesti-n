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
    public partial class Productos : Form
    {
        private ClasificacionProductoBLL clasifBLL = new ClasificacionProductoBLL();
        private List<ClasificacionProducto> listaClasificaciones = new List<ClasificacionProducto>();
        private int idSeleccionado = -1;

        public Productos()
        {
            InitializeComponent();
            dgvProductos.EnableHeadersVisualStyles = false; // Importante
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvProductos.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación básica
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    cmbClasificacion.SelectedIndex == -1 ||
                    cmbPorciones.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(txtCosto.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioVenta.Text))
                {
                    MessageBox.Show("Por favor completá todos los campos obligatorios.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validación numérica
                int porciones;
                decimal costo, precioVenta;

                if (!int.TryParse(cmbPorciones.SelectedItem.ToString(), out porciones) ||
                    !decimal.TryParse(txtCosto.Text, out costo) ||
                    !decimal.TryParse(txtPrecioVenta.Text, out precioVenta))
                {
                    MessageBox.Show("Porciones debe ser un número entero. Costo y Precio de Venta deben ser valores numéricos válidos.", "Error de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener clasificación seleccionada
                var clasificacionSeleccionada = (ClasificacionProducto)cmbClasificacion.SelectedItem;

                // Crear el nuevo producto
                Producto nuevoProducto = new Producto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Clasificacion = clasificacionSeleccionada.Detalle, // o ClasificacionId si preferís
                    Porciones = porciones,
                    Costo = costo,
                    PrecioDeVenta = precioVenta,
                    ProductoActivo = cbActivarProducto.Checked
                };

                // Guardar el producto
                ProductoBLL bll = new ProductoBLL();
                bll.AgregarNuevoProducto(nuevoProducto);

                MessageBox.Show("Producto agregado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtCosto.Clear();
            txtPrecioVenta.Clear();
            cmbClasificacion.SelectedIndex = -1;
            cmbPorciones.SelectedIndex = -1;
            cbActivarProducto.Checked = false;
        }

        private void CargarGrilla()
        {
            try
            {
                ProductoBLL bll = new ProductoBLL();
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = bll.ObtenerTodosLosProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar grilla: " + ex.Message);
            }
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            try
            {
                CargarGrilla();
                cmbClasificacion.SelectedIndexChanged += cmbClasificacion_SelectedIndexChanged;

                listaClasificaciones = clasifBLL.ObtenerTodos();

                // Cargar Combo Clasificación
                cmbClasificacion.DataSource = listaClasificaciones;
                cmbClasificacion.DisplayMember = "Detalle"; // lo que se ve
                cmbClasificacion.ValueMember = "Id";        // clave oculta

                // Cargar Combo Porciones
                var porcionesUnicas = listaClasificaciones
                    .Select(x => x.Porciones)
                    .Distinct()
                    .OrderBy(p => p)
                    .ToList();

                cmbPorciones.DataSource = porcionesUnicas;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clasificaciones: " + ex.Message);
            }
            dgvProductos.EnableHeadersVisualStyles = false; // Importante
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvProductos.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void cmbClasificacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbClasificacion.SelectedItem is ClasificacionProducto seleccionada)
                {
                    txtCosto.Text = seleccionada.Costo.ToString("0.00");
                }
            }
            catch (Exception)
            {
                txtCosto.Text = "";
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                    idSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
                    txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                    txtCosto.Text = fila.Cells["Costo"].Value.ToString();
                    txtPrecioVenta.Text = fila.Cells["PrecioDeVenta"].Value.ToString();
                    cbActivarProducto.Checked = Convert.ToBoolean(fila.Cells["ProductoActivo"].Value);

                    // Buscar la clasificación en la lista para setearla en el combo
                    string detalleClasificacion = fila.Cells["Clasificacion"].Value.ToString();
                    var clasif = listaClasificaciones.FirstOrDefault(c => c.Detalle == detalleClasificacion);
                    if (clasif != null)
                    {
                        cmbClasificacion.SelectedItem = clasif;
                        cmbPorciones.SelectedItem = clasif.Porciones;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar producto: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado == -1)
                {
                    MessageBox.Show("Seleccioná un producto de la lista para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    cmbClasificacion.SelectedIndex == -1 ||
                    cmbPorciones.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(txtCosto.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioVenta.Text))
                {
                    MessageBox.Show("Completá todos los campos antes de modificar.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar entradas numéricas
                int porciones;
                decimal costo, precioVenta;

                if (!int.TryParse(cmbPorciones.SelectedItem.ToString(), out porciones) ||
                    !decimal.TryParse(txtCosto.Text, out costo) ||
                    !decimal.TryParse(txtPrecioVenta.Text, out precioVenta))
                {
                    MessageBox.Show("Porciones debe ser un número entero. Costo y Precio de Venta deben ser válidos.", "Error de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var clasificacionSeleccionada = (ClasificacionProducto)cmbClasificacion.SelectedItem;

                Producto productoModificado = new Producto
                {
                    ID = idSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Clasificacion = clasificacionSeleccionada.Detalle, // o ClasificacionId
                    Porciones = porciones,
                    Costo = costo,
                    PrecioDeVenta = precioVenta,
                    ProductoActivo = cbActivarProducto.Checked
                };

                ProductoBLL bll = new ProductoBLL();
                bll.ActualizarProducto(productoModificado);

                MessageBox.Show("Producto modificado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarGrilla();
                idSeleccionado = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar producto: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado == -1)
                {
                    MessageBox.Show("Seleccioná un producto de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show("¿Estás seguro de que querés eliminar el producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    ProductoBLL bll = new ProductoBLL();
                    bll.EliminarProducto(idSeleccionado);

                    MessageBox.Show("Producto eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarFormulario();
                    CargarGrilla();
                    idSeleccionado = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar producto: " + ex.Message);
            }
        }
    }
}
