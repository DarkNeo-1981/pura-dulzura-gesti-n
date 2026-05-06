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
    public partial class Bitacora : Form
    {
        List<ENTITY.Bitacora> ListaBitacora;

        public Bitacora()
        {
            InitializeComponent();
        }

        private void Bitacora_Load(object sender, EventArgs e)
        {
            CargarListaBitacora();
            ActualizarGrilla();
            dgvBitacora.EnableHeadersVisualStyles = false; // Importante
            dgvBitacora.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvBitacora.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBitacora.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvBitacora.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBitacora.EnableHeadersVisualStyles = false;
            dgvBitacora.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CargarListaBitacora()
        {
            try
            {
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                ListaBitacora = b.BuscarTodos();
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar cargar la lista de entradas"); }
        }
        private void ActualizarGrilla()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("Usuario");
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Detalle");
                if (ListaBitacora != null)
                {
                    foreach (ENTITY.Bitacora b in ListaBitacora)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = b.Id.ToString();
                        dr[1] = b.Usuario.ToString();
                        dr[2] = b.Fecha.ToString();
                        dr[3] = b.Detalle.ToString();
                        dt.Rows.Add(dr);
                    }
                }
                dgvBitacora.DataSource = dt;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la grilla de entradas"); }
        }

        private void btnTodo_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                ListaBitacora = b.BuscarTodos();
                ActualizarGrilla();
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar cargar la lista de entradas"); }
        }

        private void btnBackUps_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                ListaBitacora = b.Buscar_BackUps();
                ActualizarGrilla();
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar cargar la lista de entradas"); }
        }
    }
}
