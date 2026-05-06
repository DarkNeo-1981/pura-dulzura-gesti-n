using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace UI
{
    public partial class CambioClave : Form
    {
        ENTITY.Usuarios usuario;
        public CambioClave(ENTITY.Usuarios usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.BackColor = ColorTranslator.FromHtml("#8C5A35");
            groupBox1.BackColor = ColorTranslator.FromHtml("#8C5A35");
            groupBox1.ForeColor = ColorTranslator.FromHtml("#F8E9DA");
            lblNuevaClave.ForeColor = ColorTranslator.FromHtml("#EBD5B3");
            btnGuardar.BackColor = ColorTranslator.FromHtml("#A47148");
            btnGuardar.ForeColor = ColorTranslator.FromHtml("#FFF7E8");
        }

        private void CambioClave_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Debe cambiar la clave, ingrese una nueva clave...");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int resultado = 0;
                if (txtClave.Text != string.Empty)
                {
                    BLL.UsuariosBLL op = new BLL.UsuariosBLL();
                    resultado = op.CambiarClave(usuario.Id, txtClave.Text);
                    if (resultado == 1) { this.Close(); }
                    else { MessageBox.Show("Ocurrió un problema, contacte al Administrador"); }
                }
                else { MessageBox.Show("Debe ingresar una nueva clave antes..."); }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar modificar la clave"); }
        }
    }
}
