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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace UI
{
    public partial class ClasificacionProductos : Form
    {
        private ClasificacionProductoBLL bll = new ClasificacionProductoBLL();
        private int idSeleccionado = 0;
        Menu Principal;
        public ClasificacionProductos()
        {
            InitializeComponent();
            dgvClasificacionProductos.EnableHeadersVisualStyles = false; // Importante
            dgvClasificacionProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvClasificacionProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClasificacionProductos.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvClasificacionProductos.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClasificacionProductos.EnableHeadersVisualStyles = false;
            dgvClasificacionProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void ClasificacionProductos_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            try
            {
                CargarGrilla();
            }
            catch (Exception ex) {MessageBox.Show("Error al cargar la grilla: " + ex.Message);}
        }

        private void CargarGrilla()
        {
            try
            {
                dgvClasificacionProductos.DataSource = null;
                dgvClasificacionProductos.DataSource = bll.ObtenerTodos();
                dgvClasificacionProductos.ClearSelection();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            try
            {
                txtClasificacion.Clear();
                txtPorciones.Clear();
                txtCosto.Clear();
                idSeleccionado = 0;
            }
            catch (Exception ex) { MessageBox.Show("Error al limpiar los campos: " + ex.Message);}
        }

        private ClasificacionProducto ObtenerDesdeFormulario()
        {
            return new ClasificacionProducto
            {
                Id = idSeleccionado,
                Detalle = txtClasificacion.Text,
                Porciones = int.Parse(txtPorciones.Text),
                Costo = decimal.Parse(txtCosto.Text),
                Eliminado = false
            };
        }

        private void dgvClasificacionProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow fila = dgvClasificacionProductos.Rows[e.RowIndex];
                    idSeleccionado = int.Parse(fila.Cells["Id"].Value.ToString());
                    txtClasificacion.Text = fila.Cells["Detalle"].Value.ToString();
                    txtPorciones.Text = fila.Cells["Porciones"].Value.ToString();
                    txtCosto.Text = fila.Cells["Costo"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar fila: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var nuevo = ObtenerDesdeFormulario();
                nuevo.Id = 0; // lo genera la BLL
                bll.Agregar(nuevo);
                MessageBox.Show("Clasificación agregada.");
                Principal.AgregarALaBitacora("Se ha agregado una nueva clasificación");
                CargarGrilla();
            }
            catch (FormatException)
            {
                MessageBox.Show("Porciones debe ser entero y Costo decimal.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado == 0)
                    throw new Exception("Debe seleccionar un elemento para modificar.");

                var modificado = ObtenerDesdeFormulario();
                bll.Modificar(modificado);
                MessageBox.Show("Clasificación modificada.");
                Principal.AgregarALaBitacora($"Se ha modificado la clasificación: {modificado.Detalle}");
                CargarGrilla();
            }
            catch (FormatException)
            {
                MessageBox.Show("Porciones debe ser entero y Costo decimal.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado == 0)
                    throw new Exception("Debe seleccionar un elemento para eliminar.");

                var confirm = MessageBox.Show("¿Eliminar esta clasificación?", "Confirmación", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    bll.Eliminar(idSeleccionado);
                    MessageBox.Show("Clasificación eliminada.");
                    Principal.AgregarALaBitacora($"Se ha eliminado la clasificación: {idSeleccionado} - {txtClasificacion.Text}");
                    CargarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
