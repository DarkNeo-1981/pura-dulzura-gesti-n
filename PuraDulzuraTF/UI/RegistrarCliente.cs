using BLL;
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
    public partial class RegistrarCliente : Form
    {
        internal ENTITY.Usuarios usuario;
        public RegistrarCliente(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            this.usuario = pUsuario;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegistrarCliente_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string dni = txtDni.Text;
                string telefono = txtTelefono.Text;
                string email = txtEmail.Text;
                string calle = txtCalle.Text;
                string numero = txtNumero.Text;
                string piso = txtPiso.Text;
                string depto = txtDepto.Text;
                string localidad = txtLocalidad.Text;

                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(dni))
                {
                    MessageBox.Show("Nombre, Apellido y DNI son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ClienteBLL clienteBLL = new ClienteBLL();
                clienteBLL.Agregar(nombre, apellido, dni, telefono, email, calle, numero, piso, depto, localidad);
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se realizó un registro la base de datos Clientes({fecha})");
                MessageBox.Show("Cliente registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
