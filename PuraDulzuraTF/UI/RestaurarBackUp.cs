using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class RestaurarBackUp : Form
    {
        UI.Menu Principal;

        public RestaurarBackUp()
        {
            InitializeComponent();
        }

        private void RestaurarBackUp_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            CargarComboBackUp();
        }

        private List<string> CargarBackUps()
        {
            try
            {
                string directorio = Environment.CurrentDirectory + "\\BackUp\\";
                List<string> backups = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(directorio);
                if (Directory.Exists(directorio))
                {
                    foreach (DirectoryInfo d in dir.GetDirectories())
                    {
                        backups.Add(d.Name);
                    }
                }
                return backups;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de backups"); return null; }
        }
        private void CargarComboBackUp()
        {
            try
            {
                cmbBackUp.DataSource = CargarBackUps();
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar el combo de backups"); }
        }
        private void Restaurar(string pBackUp)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\BackUp\\" + pBackUp);
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.CopyTo(Path.Combine(Environment.CurrentDirectory + "\\DB\\", file.Name), true);
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al restaurar el backup"); }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBackUp.Items.Count > 0)
                {
                    var seleccion = MessageBox.Show("Va a restaurar el BackUp seleccionado, ¿Esta seguro?", "Restaurar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (seleccion == DialogResult.Yes)
                    {
                        Restaurar(cmbBackUp.SelectedValue.ToString());
                        BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                        b.AgregarUno($"{Principal.usuario.Id} - {Principal.usuario.Usuario}", $"Se restauró el BackUp {cmbBackUp.SelectedValue.ToString()}");
                        MessageBox.Show("Se restauró el BackUp seleccionado correctamente.");
                    }
                }
                else { MessageBox.Show("No hay BackUps para restarurar."); }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al restaurar el backup"); }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
