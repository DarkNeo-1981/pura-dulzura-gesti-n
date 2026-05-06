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
    public partial class VerClientes : Form
    {
        ClienteBLL clienteBLL = new ClienteBLL();
        DataTable tablaClientes;
        internal ENTITY.Usuarios usuario;

        public VerClientes(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            this.usuario = pUsuario;
            dgvClientes.EnableHeadersVisualStyles = false; // Importante
            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClientes.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvClientes.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void VerClientes_Load(object sender, EventArgs e)
        {
            cmbFiltro.Items.AddRange(new string[] { "Nombre", "Apellido", "DNI" });
            cmbFiltro.SelectedIndex = 0;

            tablaClientes = clienteBLL.Buscar_Todos(false);
            dgvClientes.DataSource = tablaClientes;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            FiltrarClientes();
        }

        private void FiltrarClientes()
        {
            if (tablaClientes == null) return;

            string campo = cmbFiltro.SelectedItem?.ToString();
            string texto = txtBuscar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(campo)) return;

            string columna = "Nombre";
            switch (campo)
            {
                case "Nombre":
                    columna = "Nombre";
                    break;
                case "Apellido":
                    columna = "Apellido";
                    break;
                case "DNI":
                    columna = "Dni";
                    break;
            }

            DataView dv = tablaClientes.DefaultView;
            dv.RowFilter = $"{columna} LIKE '%{texto}%'";
            dgvClientes.DataSource = dv;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["Id"].Value);
                DialogResult result = MessageBox.Show("¿Estás seguro que deseas eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    clienteBLL.Borrar(id);
                    BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                    string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se eliminó un registro la base de datos Clientes({fecha})");
                    MessageBox.Show("Cliente eliminado.");
                    tablaClientes = clienteBLL.Buscar_Todos(false);
                    dgvClientes.DataSource = tablaClientes;
                }
            }
            else
            {
                MessageBox.Show("Seleccioná un cliente.");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["Id"].Value);
                string nombre = dgvClientes.SelectedRows[0].Cells["Nombre"].Value.ToString();
                string apellido = dgvClientes.SelectedRows[0].Cells["Apellido"].Value.ToString();
                string dni = dgvClientes.SelectedRows[0].Cells["Dni"].Value.ToString();
                string telefono = dgvClientes.SelectedRows[0].Cells["Telefono"].Value.ToString();
                string email = dgvClientes.SelectedRows[0].Cells["Email"].Value.ToString();
                string calle = dgvClientes.SelectedRows[0].Cells["Calle"].Value.ToString();
                string numero = dgvClientes.SelectedRows[0].Cells["Numero"].Value.ToString();
                string piso = dgvClientes.SelectedRows[0].Cells["Piso"].Value.ToString();
                string depto = dgvClientes.SelectedRows[0].Cells["Depto"].Value.ToString();
                string localidad = dgvClientes.SelectedRows[0].Cells["Localidad"].Value.ToString();
                string eliminado = dgvClientes.SelectedRows[0].Cells["Eliminado"].Value.ToString();

                ModificarCliente modificar = new ModificarCliente(id, nombre, apellido, dni, telefono, email, calle, numero, piso, depto, localidad, this.usuario);
                modificar.ShowDialog();
                tablaClientes = clienteBLL.Buscar_Todos(false);
                dgvClientes.DataSource = tablaClientes;
            }
            else
            {
                MessageBox.Show("Seleccioná un cliente para modificar.");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
