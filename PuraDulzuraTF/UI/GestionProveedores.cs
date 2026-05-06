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
    public partial class GestionProveedores : Form
    {
        private ProveedoresBLL _proveedorBLL;
        public ENTITY.Usuarios usuario;
        Menu Principal;

        public GestionProveedores(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            _proveedorBLL = new ProveedoresBLL();           
        }

        private void GestionProveedores_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            // --- Estilos de la Grilla ---
            dgvProveedores.EnableHeadersVisualStyles = false; // Importante para aplicar estilos personalizados

            // Estilo de los encabezados de columna
            dgvProveedores.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SaddleBrown;
            dgvProveedores.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvProveedores.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            // Estilo de los encabezados de fila (aunque no se vean, es bueno mantener la consistencia)
            dgvProveedores.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(140, 90, 53);
            dgvProveedores.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvProveedores.RowHeadersVisible = false; // Como antes, puedes ocultarlas si no las necesitas.

            // Estilo de las celdas de datos
            dgvProveedores.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvProveedores.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvProveedores.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AntiqueWhite; // Opcional, para mejor separación

            // --- Lógica de Carga de Datos y Ajuste de Tamaño ---
            CargarGrilla();

            if (dgvProveedores.Columns.Contains("EstaActivo"))
            {
                dgvProveedores.Columns["EstaActivo"].HeaderText = "Activo";
            }

            AjustarTamanos(); // Llamar al nuevo método de ajuste
        }

        private void AjustarTamanos()
        {
            // --- 1. Ajustar la ALTURA (Sin cambios, funciona bien) ---

            // Calcular la altura de la grilla
            int numFilas = dgvProveedores.Rows.Count;
            int alturaFilas = dgvProveedores.Rows.Cast<DataGridViewRow>().Where(r => r.Visible).Sum(r => r.Height);

            if (numFilas < 5)
            {
                dgvProveedores.Height = (dgvProveedores.ColumnHeadersHeight * 6) + 15;
            }
            else
            {
                dgvProveedores.Height = alturaFilas + dgvProveedores.ColumnHeadersHeight + 15;
            }

            // Ajustar la altura del GroupBox
            gbListadoProveedores.Height = dgvProveedores.Location.Y + dgvProveedores.Height + 20;


            // --- 2. Ajustar el ANCHO (Corrección de Superposición) ---

            // 2.1. Definir la posición de los botones            
            int inicioBotonesX = btnAltaProveedor.Location.X; // Posición X donde deben empezar los botones

            // 2.2. CALCULAR ANCHO DEL GROUPBOX            
            int margenSeparacion = 20;
            int anchoRequeridoGroupBox = inicioBotonesX - gbListadoProveedores.Location.X - margenSeparacion;
            int anchoMinimoGroupBox = 750;
            gbListadoProveedores.Width = Math.Max(anchoRequeridoGroupBox, anchoMinimoGroupBox);

            // 2.3. Asegurar que la DGV ocupe todo el ancho del GroupBox (CLAVE para FILL)
            dgvProveedores.Width = gbListadoProveedores.Width - 20; // 10px de padding izquierdo y 10px derecho

            // --- 3. Redimensionar el Formulario ---

            // El ancho del formulario debe basarse en el borde derecho de los botones
            int botonesDerecha = btnEliminarProveedor.Location.X + btnEliminarProveedor.Width;

            // Ajustar el ancho del formulario (Borde derecho de los botones + 50 px de margen)
            this.Width = botonesDerecha + 50;

            // Altura final del formulario
            this.Height = gbListadoProveedores.Location.Y + gbListadoProveedores.Height + 60;
        }

        private void CargarGrilla()
        {
            try
            {
                
                dgvProveedores.DataSource = _proveedorBLL.TraerProveedores(false);                
                ConfigurarGrilla();
                AjustarTamanos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista de proveedores: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ConfigurarGrilla()
        {
            // Asegurar que solo se aplica esto si hay columnas cargadas.
            if (dgvProveedores.Columns.Count == 0) return;

            // --- Columna Oculta ---
            if (dgvProveedores.Columns.Contains("Id"))
            {
                dgvProveedores.Columns["Id"].Visible = false; // Oculto el ID
            }

            // Configuración inicial de todas las columnas
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Columna de selección de fila (RowHeader)
            dgvProveedores.RowHeadersWidth = 35;

            // --- Configuración de Columnas Fijas y AutoCells ---
            // CUIL/CUIT
            if (dgvProveedores.Columns.Contains("CUIT"))
            {
                dgvProveedores.Columns["CUIT"].Width = 100;
            }

            // Teléfono
            if (dgvProveedores.Columns.Contains("Telefono"))
            {
                dgvProveedores.Columns["Telefono"].Width = 100;
            }

            // FechaAlta
            if (dgvProveedores.Columns.Contains("FechaAlta"))
            {
                dgvProveedores.Columns["FechaAlta"].Width = 90;
            }

            // EstaActivo (Ahora "Activo") y Eliminado
            if (dgvProveedores.Columns.Contains("EstaActivo"))
            {
                dgvProveedores.Columns["EstaActivo"].Width = 50;
            }
            if (dgvProveedores.Columns.Contains("Eliminado"))
            {
                dgvProveedores.Columns["Eliminado"].Width = 60;
            }

            // CondicionIVA (Autoajusta al contenido de la celda)
            if (dgvProveedores.Columns.Contains("CondicionIVA"))
            {
                dgvProveedores.Columns["CondicionIVA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // --- Configuración de Columnas Flexibles (FILL) ---

            // RazonSocial 
            if (dgvProveedores.Columns.Contains("RazonSocial"))
            {
                dgvProveedores.Columns["RazonSocial"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvProveedores.Columns["RazonSocial"].FillWeight = 300;
            }

            // Email 
            if (dgvProveedores.Columns.Contains("Email"))
            {
                dgvProveedores.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvProveedores.Columns["Email"].FillWeight = 200; 
            }

            // Direccion (Mantenemos un peso sólido)
            if (dgvProveedores.Columns.Contains("Direccion"))
            {
                dgvProveedores.Columns["Direccion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvProveedores.Columns["Direccion"].FillWeight = 200; 
            }
        }

        private void btnAltaProveedor_Click(object sender, EventArgs e)
        {
            using (DetalleProveedor frm = new DetalleProveedor(this.usuario, this.Principal, null))
            {                
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Proveedor agregado exitosamente", "Éxito");
                    Principal.AgregarALaBitacora("Se agregó un nuevo proveedor");
                    CargarGrilla();
                }
            }
        }

        private void btnModificarProveedor_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un proveedor para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Obtener el ID del proveedor de la fila seleccionada 
                int idProveedor = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["Id"].Value);

                // 2. Abrir el formulario DetalleProveedor en modo Modificación
                using (DetalleProveedor frm = new DetalleProveedor(this.usuario, this.Principal, idProveedor))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Proveedor modificado exitosamente", "Éxito");
                        Principal.AgregarALaBitacora("Se modificó el proveedor con ID: " + idProveedor);
                        CargarGrilla();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar modificar el proveedor: " + ex.Message, "Error de Modificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarProveedor_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un proveedor para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Confirmación de Usuario
            DialogResult confirmacion = MessageBox.Show(
                "¿Está seguro que desea dar de baja (Eliminar Lógicamente) el proveedor seleccionado?",
                "Confirmar Baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // 2. Obtener el ID
                    int idProveedor = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["Id"].Value);
                    string razonSocial = dgvProveedores.CurrentRow.Cells["RazonSocial"].Value.ToString();

                    // 3. Llamar a la BLL para realizar la Baja Lógica
                    _proveedorBLL.Borrar(idProveedor);

                    // 4. Éxito
                    MessageBox.Show($"Proveedor '{razonSocial}' dado de baja exitosamente.", "Baja Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Principal.AgregarALaBitacora($"Se dio de baja lógicamente el proveedor con ID: {idProveedor}");
                    CargarGrilla();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al intentar dar de baja el proveedor: " + ex.Message, "Error de Baja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Instanciar el formulario que gestiona las materias primas              

                using (ProductoProveedor frm = new ProductoProveedor())
                {
                    // 2. Mostrar el formulario
                    frm.ShowDialog();
                    Principal.AgregarALaBitacora("Se agregó/Modifico/Elimino un producto proveedor");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la gestión de productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
