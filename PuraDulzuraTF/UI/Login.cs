using ENTITY;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace UI
{
    public partial class Login : Form
    {
        Menu Principal;
        CambioClave f_CambiarClave;
        ENTITY.Usuarios usuario;

        public Login()
        {
            InitializeComponent();
            btnVerContrasena.FlatAppearance.BorderSize = 0;
            btnVerContrasena.TabStop = false; // evita que se seleccione con TAB           
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVerContrasena_MouseDown(object sender, MouseEventArgs e)
        {
            txtContrasena.PasswordChar = '\0';
        }

        private void btnVerContrasena_MouseUp(object sender, MouseEventArgs e)
        {
            txtContrasena.PasswordChar = '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Seguridad.Login login = new Seguridad.Login();                
                ENTITY.Usuarios usuario = login.ValidarUsuario(this.txtUsuario.Text, txtContrasena.Text);
                
                if (usuario != null)
                {
                    Principal.usuario = usuario;

                    if (SERVICIOS.Encriptacion.Desencriptar(usuario.Clave) == "cambiar")
                    {
                        f_CambiarClave = new CambioClave(usuario);
                        f_CambiarClave.FormClosed += ClaveModificada;
                        this.Hide();
                        f_CambiarClave.Show();
                    }
                    else
                    {
                        Principal.usuario = usuario;
                        Principal.MostrarMenu();
                        Principal.AgregarALaBitacora($"Se ingresó al sistema con el operador {usuario.Usuario} - {usuario.DNI}");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Nombre de usuario o contraseña incorrectos");  Principal.AgregarALaBitacora($"Se intentó ingresar al sistema con el usuario {txtUsuario.Text}");
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar loguearse"); }
        }

        private void ClaveModificada(object sender, FormClosedEventArgs e)
        {
            try
            {
                f_CambiarClave = null;
                this.Show();
                MessageBox.Show("Clave modificada exitosamente.");                
                Principal.MostrarMenu();
                Principal.AgregarALaBitacora($"Se ingresó al sistema el operador {Principal.usuario.Usuario}");
                this.Close();
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema cuando se modificó la clave"); }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;            
            this.BackColor = ColorTranslator.FromHtml("#8C5A35");
            lblTitulo.ForeColor = ColorTranslator.FromHtml("#F8E9DA");     // Beige claro
            lblUsuario.ForeColor = ColorTranslator.FromHtml("#EBD5B3");   // Arena claro
            lblContrasena.ForeColor = ColorTranslator.FromHtml("#EBD5B3");
            txtUsuario.BackColor = ColorTranslator.FromHtml("#F8E9DA");
            txtUsuario.ForeColor = ColorTranslator.FromHtml("#3B241A");
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.BackColor = ColorTranslator.FromHtml("#F8E9DA");
            txtContrasena.ForeColor = ColorTranslator.FromHtml("#3B241A");
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            btnLogin.BackColor = ColorTranslator.FromHtml("#A47148");
            btnLogin.ForeColor = ColorTranslator.FromHtml("#FFF7E8");
            btnSalir.BackColor = ColorTranslator.FromHtml("#3B241A");
            btnSalir.ForeColor = ColorTranslator.FromHtml("#FFF7E8");
        }

        private void txtContrasena_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
