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
    public partial class VerProductos : Form
    {
        private ProductoBLL bll = new ProductoBLL();

        public VerProductos()
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

        private void VerProductos_Load(object sender, EventArgs e)
        {
            try
            {
                CargarProductos(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message);
            }
        }

        private void CargarProductos()
        {
            try
            {
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = bll.ObtenerTodosLosProductos();
                dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvProductos.ClearSelection();             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }   

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
