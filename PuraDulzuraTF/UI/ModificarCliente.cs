using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace UI
{
    public partial class ModificarCliente : Form
    {
        private int clienteId;
        private ClienteBLL clienteBLL = new ClienteBLL();
        internal ENTITY.Usuarios usuario;

        public ModificarCliente(int id, string nombre, string apellido, string dni, string telefono, string email, string calle, string numero, string depto, string piso, string localidad, ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            clienteId = id;
            txtNombre.Text = nombre;
            txtApellido.Text = apellido;
            txtDni.Text = dni;
            txtTelefono.Text = telefono;
            txtEmail.Text = email;
            txtCalle.Text = calle;
            txtNumero.Text = numero;
            txtDepto.Text = depto;
            txtPiso.Text = piso;
            txtLocalidad.Text = localidad;
            this.usuario = pUsuario;
        }

        public ModificarCliente()
        {
            InitializeComponent();
        }

        private void ModificarCliente_Load(object sender, EventArgs e)
        {
                      
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string dni = txtDni.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string email = txtEmail.Text.Trim();
            string calle = txtCalle.Text.Trim();
            string numero = txtNumero.Text.Trim();
            string piso = txtPiso.Text.Trim();
            string depto = txtDepto.Text.Trim();
            string localidad = txtLocalidad.Text.Trim();

            int resultado = clienteBLL.Modificar(clienteId, nombre, apellido, dni, telefono, email, calle, numero, depto, piso, localidad);


            if (resultado == 1)
            {                
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se realizó una modificación de la base de datos Clientes({fecha})");
                MessageBox.Show("Se modifico al cliente correctamente.");
                this.DialogResult = DialogResult.OK;  
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al modificar el cliente.");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
