using BLL;
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

namespace UI
{
    public partial class DetalleProveedor : Form
    {
        private ProveedoresBLL _proveedorBLL = new ProveedoresBLL();
        private ENTITY.Proveedores _proveedorActual;        
        private Menu _principal;
        internal ENTITY.Usuarios usuario;

        // NUEVAS VARIABLES PARA PRODUCTOS
        private ProductoProveedorBLL _productoBLL = new ProductoProveedorBLL();
        private List<ENTITY.ProductoProveedor> _productosDisponibles; // Lista completa de entidades

        public DetalleProveedor(ENTITY.Usuarios pUsuario, Menu pPrincipal, int? idProveedor = null)
        {
            InitializeComponent();
            this.usuario = pUsuario;
            this._principal = pPrincipal;
            CargarProductosDisponibles();            

            if (idProveedor.HasValue)
            {
                this.Text = "Modificar Proveedor";
                CargarDatosProveedor(idProveedor.Value);
            }
            else
            {
                this.Text = "Registrar Nuevo Proveedor";
                _proveedorActual = new Proveedores();
                _proveedorActual.IdsProductosSuministrados = new List<int>();
            }
        }        
       
        private void CargarProductosDisponibles()
        {
            try
            {                
                DataTable dt = _productoBLL.TraerTodos(true);

                // Mapea el DataTable a la lista de entidades ProductoProveedor
                _productosDisponibles = dt.AsEnumerable().Select(row => new ENTITY.ProductoProveedor
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()                   
                }).ToList();
                
                clbProductosSuministrados.DataSource = _productosDisponibles;
                clbProductosSuministrados.DisplayMember = "Nombre"; 
                clbProductosSuministrados.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos disponibles: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }
        }

        private void DetalleProveedor_Load(object sender, EventArgs e)
        {            
            // 1. Cargar el ComboBox de Condición IVA
            cmbCondicionIva.DataSource = DatosFijos.ObtenerCondicionesIVA();

            // 2. Cargar el ComboBox de Estado (Activo/Inactivo)
            cmbEstadoProveedor.DataSource = DatosFijos.ObtenerEstadosLogicos();
            cmbEstadoProveedor.DisplayMember = "Value"; // Mostrar la descripción ("Activo" / "Inactivo")
            cmbEstadoProveedor.ValueMember = "Key";    // Usar el valor booleano (true / false)            

            // --- LÓGICA DE CARGA DE DATOS AL INICIO ---

            if (_proveedorActual != null && _proveedorActual.Id != 0)
            {
                // Cargar campos con datos de _proveedorActual
                txtIdProveedor.Text = _proveedorActual.Id.ToString();
                cmbCondicionIva.Text = _proveedorActual.CondicionIVA;
                cmbEstadoProveedor.SelectedValue = _proveedorActual.EstaActivo;

                // Marcar los Productos Suministrados
                MarcarProductosSuministrados();               
            }
            else
            {
                // Configurar la vista para un nuevo registro
                txtIdProveedor.Text = "Nuevo";
                dtpFechaAltaProveedor.Value = DateTime.Now;
            }
        }
        
        private void MarcarProductosSuministrados()
        {            
            if (_proveedorActual.IdsProductosSuministrados == null || !_proveedorActual.IdsProductosSuministrados.Any()) return;

            for (int i = 0; i < clbProductosSuministrados.Items.Count; i++)
            {                
                ENTITY.ProductoProveedor itemProducto = (ENTITY.ProductoProveedor)clbProductosSuministrados.Items[i];
                                
                if (_proveedorActual.IdsProductosSuministrados.Contains(itemProducto.Id))
                {
                    clbProductosSuministrados.SetItemChecked(i, true);
                }
            }
        }

        private void CargarDatosProveedor(int id)
        {
            _proveedorActual = _proveedorBLL.TraerPorId(id);

            if (_proveedorActual != null)
            {               
                txtIdProveedor.Text = Convert.ToString(_proveedorActual.Id);
                txtRazonSocial.Text = _proveedorActual.RazonSocial;
                txtCUIT.Text = _proveedorActual.CUIT;
                txtDireccion.Text = _proveedorActual.Direccion;
                txtEmail.Text = _proveedorActual.Email;
                txtTelefono.Text = _proveedorActual.Telefono;
                dtpFechaAltaProveedor.Value = _proveedorActual.FechaAlta;                
            }
            else
            {
                MessageBox.Show("Error: Proveedor no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar campos obligatorios básicos en la UI (aunque la BLL los valida también)
                if (string.IsNullOrWhiteSpace(txtRazonSocial.Text) || string.IsNullOrWhiteSpace(txtCUIT.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text))
                {
                    MessageBox.Show("La Razón Social, CUIT y Dirección son campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Mapear los datos de la UI a la entidad Proveedor
                _proveedorActual.RazonSocial = txtRazonSocial.Text.Trim();
                _proveedorActual.CUIT = txtCUIT.Text.Trim();
                _proveedorActual.Direccion = txtDireccion.Text.Trim();
                _proveedorActual.Email = txtEmail.Text.Trim();
                _proveedorActual.Telefono = txtTelefono.Text.Trim();

                // Mapeo de ComboBoxes
                _proveedorActual.CondicionIVA = cmbCondicionIva.SelectedValue.ToString();
                _proveedorActual.EstaActivo = (bool)cmbEstadoProveedor.SelectedValue;

                // 3: Mapear la lista de IDs de Productos Suministrados del CheckListBox
                _proveedorActual.IdsProductosSuministrados.Clear(); // Limpiar la lista para rellenar

                // Iterar sobre los ítems chequeados, que son objetos ProductoProveedor
                foreach (ENTITY.ProductoProveedor productoSeleccionado in clbProductosSuministrados.CheckedItems)
                {                   
                    _proveedorActual.IdsProductosSuministrados.Add(productoSeleccionado.Id);
                }               
               
                if (_proveedorActual.Id == 0)
                {
                    _proveedorActual.FechaAlta = DateTime.Now;
                }

                int resultado;

                // 3. Determinar Operación (Alta vs. Modificación)
                if (_proveedorActual.Id == 0) // Es un nuevo proveedor
                {
                    resultado = _proveedorBLL.Agregar(_proveedorActual);
                }
                else // Es una modificación
                {
                    resultado = _proveedorBLL.Modificar(_proveedorActual);
                }

                if (resultado > 0)
                {
                    // Operación exitosa: Cierra el formulario con DialogResult.OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("La operación no pudo completarse. Contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // 4. Manejo de Errores de la BLL
                MessageBox.Show(ex.Message, "Error de Negocio/Persistencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    _principal.AgregarALaBitacora("Se agregó/Modifico/Elimino un producto proveedor");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la gestión de productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
