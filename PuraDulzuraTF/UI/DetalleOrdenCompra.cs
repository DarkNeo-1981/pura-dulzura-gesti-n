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
    public partial class DetalleOrdenCompra : Form
    {
        private readonly OrdenCompraBLL _ordenCompraBLL = new OrdenCompraBLL();
        private readonly ProveedoresBLL _proveedoresBLL = new ProveedoresBLL();
        private readonly ProductoBLL _productosBLL = new ProductoBLL();
        private OrdenCompra _ordenActual;
        private bool _esModificacion;
        Menu Principal;

        // Constructor para NUEVA orden
        public DetalleOrdenCompra()
        {
            InitializeComponent();
            _ordenActual = new OrdenCompra { Detalles = new List<ENTITY.DetalleOrdenCompra>(), Estado = "PENDIENTE" };
            _esModificacion = false;
        }

        public DetalleOrdenCompra(ENTITY.Usuarios pUsuario)
        {
            InitializeComponent();
            _ordenActual = new OrdenCompra { Detalles = new List<ENTITY.DetalleOrdenCompra>(), Estado = "PENDIENTE" };
            _esModificacion = false;
        }

        // Constructor para MODIFICAR orden 
        public DetalleOrdenCompra(int idOrden)
        {
            InitializeComponent();
            _ordenActual = _ordenCompraBLL.TraerOrdenPorId(idOrden) ?? new OrdenCompra { Detalles = new List<ENTITY.DetalleOrdenCompra>(), Estado = "PENDIENTE" };
            _esModificacion = true;
        }

        private void DetalleOrdenCompra_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            CargarCombos();
            CargarDatosOrden();
        }

        private void CargarCombos()
        {
            DataTable dtProveedores = _proveedoresBLL.TraerProveedores(true);

            cmbProveedor.DataSource = dtProveedores;
            cmbProveedor.DisplayMember = "RazonSocial";
            cmbProveedor.ValueMember = "Id";
        }

        private void CargarDatosOrden()
        {
            if (_esModificacion)
            {
                this.Text = "Modificar Orden de Compra: " + _ordenActual.Id;
                lblIdOrden.Text = _ordenActual.Id.ToString();
                cmbProveedor.SelectedValue = _ordenActual.Proveedor?.Id;

                if (_ordenActual.Estado != "PENDIENTE")
                {
                    cmbProveedor.Enabled = false;
                    btnGuardar.Enabled = false; // Deshabilitar si no está pendiente.
                }
            }
            else
            {
                this.Text = "Registrar Nueva Orden de Compra";
                lblIdOrden.Text = "NUEVO";
                if (cmbProveedor.Items.Count > 0)
                {
                    cmbProveedor.SelectedIndex = 0;
                }
            }

            // Dispara la carga de la grilla con los productos del proveedor seleccionado.
            cmbProveedor_SelectedValueChanged(this, EventArgs.Empty);

            // Si es modificación, se cargan los ítems existentes en la grilla.
            if (_esModificacion)
            {
                CargarGrillaDetalleItems();
            }

            dtpFechaEmision.Value = _ordenActual.FechaEmision;
            txtEstado.Text = _ordenActual.Estado;
            ActualizarTotal();
        }

        private DataTable TraerProductosDisponibles()
        {
            List<ENTITY.Producto> listaProductos = TraerProductosEntidad();

            if (listaProductos.Count == 0)
            {
                return CrearDataTableProductosVacio();
            }

            return SERVICIOS.ConvertirListaADatatable.ListToDatatable(listaProductos);
        }

        private void ConfigurarGrillaDetalle()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();

            AplicarEstilosGrilla();

            DataTable dtProductos = TraerProductosDisponibles();
            dgvDetalles.DataSource = CrearDataTableDetalle();

            // COLUMNA 1: PRODUCTO (COMBOBOX) - **MODIFICADO** para usar FILL
            DataGridViewComboBoxColumn comboProducto = new DataGridViewComboBoxColumn
            {
                Name = "Producto",
                HeaderText = "Producto",
                DataSource = dtProductos,
                DisplayMember = "Nombre",
                ValueMember = "ID",
                DataPropertyName = "IdProducto", 
            };
            dgvDetalles.Columns.Add(comboProducto);
            // Aplicar FILL MODE
            dgvDetalles.Columns["Producto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDetalles.Columns["Producto"].FillWeight = 150;

            // COLUMNA 2: CANTIDAD (TEXTO)
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                Width = 80,
                // Alineación al centro/derecha para números
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } 
            });

            // COLUMNA 3: PRECIO UNITARIO (TEXTO con formato) - **MODIFICADO** para alinear a la derecha
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrecioUnitario",
                HeaderText = "Precio Unitario",
                DataPropertyName = "PrecioUnitario",
                Width = 120,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight } 
            });

            // COLUMNA 4: SUBTOTAL (SÓLO LECTURA) - **MODIFICADO** para alinear a la derecha y con ancho garantizado
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                Width = 120, 
                ReadOnly = true,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight } 
            });

            // **IMPORTANTE**: Mantener AutoSizeColumnsMode.None para respetar los Width/FillMode específicos.
            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvDetalles.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void AplicarEstilosGrilla()
        {
            dgvDetalles.EnableHeadersVisualStyles = false;
            dgvDetalles.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SaddleBrown;
            dgvDetalles.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvDetalles.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            dgvDetalles.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(140, 90, 53);
            dgvDetalles.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvDetalles.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvDetalles.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvDetalles.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AntiqueWhite;
        }

        private void CargarGrillaDetalleItems()
        {            
            if (!(dgvDetalles.DataSource is DataTable dt)) return;

            dt.Clear();
            
            foreach (var item in _ordenActual.Detalles)
            {
                DataRow newRow = dt.NewRow();
                newRow["IdProducto"] = item.IdProducto;
                newRow["Cantidad"] = item.Cantidad;
                newRow["PrecioUnitario"] = item.PrecioUnitario;
                newRow["Subtotal"] = item.Subtotal;
                dt.Rows.Add(newRow);
            }
            dt.AcceptChanges();
            ActualizarTotal();
        }

        private DataTable CrearDataTableDetalle()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdProducto", typeof(int));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("PrecioUnitario", typeof(decimal));
            dt.Columns.Add("Subtotal", typeof(decimal));
            return dt;
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = $"Total: {_ordenActual.Total:C2}";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // **PASO 1: SINCRONIZACIÓN Y LIMPIEZA DE DATOS**
                dgvDetalles.EndEdit();

                //Forzar el commit del CurrencyManager
                CurrencyManager cm = (CurrencyManager)dgvDetalles.BindingContext[dgvDetalles.DataSource];
                cm.EndCurrentEdit();

                // Obtener la fuente de datos limpia
                if (!(dgvDetalles.DataSource is DataTable detallesDt))
                {
                    MessageBox.Show("Error: La fuente de datos de la grilla no es un DataTable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Eliminar la fila vacía al final si existe
                if (dgvDetalles.AllowUserToAddRows && detallesDt.Rows.Count > 0)
                {
                    DataRow lastRow = detallesDt.Rows[detallesDt.Rows.Count - 1];
                    // Si el IdProducto de la última fila es nulo o 0, se elimina.
                    if (lastRow.IsNull("IdProducto") ||
                        (lastRow["IdProducto"] is int id && id <= 0))
                    {
                        lastRow.Delete();
                    }
                }

                // Recalcular y validar los datos finales de la grilla. El "true" fuerza excepciones de validación.
                RecalcularDetallesDesdeGrilla(true);

                if (_ordenActual.Detalles.Count == 0)
                {
                    MessageBox.Show("La Orden de Compra debe tener al menos un producto válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // **PASO 2: ACTUALIZAR ENTIDAD Y LLAMAR A LA BLL**

                // Actualizar la cabecera de la _ordenActual
                _ordenActual.FechaEmision = dtpFechaEmision.Value;
                _ordenActual.Estado = "PENDIENTE";
                _ordenActual.Proveedor = new Proveedores()
                {
                    Id = Convert.ToInt32(cmbProveedor.SelectedValue)
                };

                int resultado = 0;

                if (_esModificacion)
                {
                    resultado = _ordenCompraBLL.Modificar(_ordenActual);
                    MessageBox.Show($"Orden de Compra N° {_ordenActual.Id} modificada con éxito.", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Principal.AgregarALaBitacora($"Se modifico la orden: {_ordenActual.Id}");
                }
                else
                {
                    resultado = _ordenCompraBLL.Agregar(_ordenActual);
                    MessageBox.Show($"Orden de Compra N° {resultado} guardada con éxito.", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Principal.AgregarALaBitacora($"Se Guardo la orden: {_ordenActual.Id}");
                }

                if (resultado > 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDetalles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (dgvDetalles.Columns[e.ColumnIndex].Name == "Cantidad" ||
                                     dgvDetalles.Columns[e.ColumnIndex].Name == "PrecioUnitario" ||
                                     dgvDetalles.Columns[e.ColumnIndex].Name == "Producto"))
            {
                dgvDetalles_CellValueChanged_1(sender, e);
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.DataSource is DataTable dt)
            {
                DataRow newRow = dt.NewRow();
                newRow["Cantidad"] = 1;
                newRow["PrecioUnitario"] = 0.00m;
                newRow["Subtotal"] = 0.00m;
                dt.Rows.Add(newRow);

                dt.AcceptChanges();
                dgvDetalles.Refresh();

                if (dgvDetalles.Rows.Count > 0)
                {
                    int lastRowIndex = dgvDetalles.Rows.Count - 1;

                    // Asegura que la última fila sea visible
                    dgvDetalles.FirstDisplayedScrollingRowIndex = lastRowIndex;

                    // Selecciona la última fila y la celda inicial para empezar a editar
                    dgvDetalles.Rows[lastRowIndex].Selected = true;
                    dgvDetalles.CurrentCell = dgvDetalles.Rows[lastRowIndex].Cells["Producto"];
                }
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione la fila de detalle que desea quitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDetalles.DataSource is DataTable dt)
            {
                try
                {
                    DataRowView drv = dgvDetalles.SelectedRows[0].DataBoundItem as DataRowView;
                    if (drv != null)
                    {
                        drv.Row.Delete();
                        dt.AcceptChanges();
                        dgvDetalles.Refresh();
                        // Actualizar la entidad y el total
                        RecalcularDetallesDesdeGrilla();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al intentar quitar el detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RecalcularDetallesDesdeGrilla(bool lanzarExcepcion = false)
        {
            // Obtener la lista de productos disponibles para el proveedor actual
            List<ENTITY.Producto> todosLosProductos = TraerProductosEntidad();

            // Forzar el commit del control DataGridView al DataTable
            dgvDetalles.EndEdit();

            // Forzar el commit del CurrencyManager (si no es al guardar, lo hacemos aquí para el recálculo)
            CurrencyManager cm = (CurrencyManager)dgvDetalles.BindingContext[dgvDetalles.DataSource];
            cm.EndCurrentEdit();

            // Limpiar la lista de detalles de la entidad Orden para reconstruirla
            _ordenActual.Detalles.Clear();

            if (!(dgvDetalles.DataSource is DataTable dt))
            {
                ActualizarTotal();
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;

                // Función auxiliar para obtener el valor de la columna de forma segura (maneja DBNull)
                string ObtenerValorCelda(string columnName)
                {
                    object value = row[columnName];
                    return (value == null || value == DBNull.Value) ? string.Empty : value.ToString();
                }

                // 1. Validar Producto
                string idProductoString = ObtenerValorCelda("IdProducto");
                if (!int.TryParse(idProductoString, out int idProducto) || idProducto <= 0)
                {
                    continue; // Ignorar filas sin producto
                }

                // Buscar producto
                ENTITY.Producto productoSeleccionado = todosLosProductos.FirstOrDefault(p => p.ID == idProducto);
                string productoNombre = productoSeleccionado?.Nombre;

                if (productoSeleccionado == null)
                {
                    if (lanzarExcepcion) throw new Exception($"Error: No se encontró el producto con ID {idProducto} en el catálogo del proveedor.");
                    continue;
                }

                // 2. Validar Cantidad
                string cantidadString = ObtenerValorCelda("Cantidad");
                if (!int.TryParse(cantidadString, out int cantidad) || cantidad <= 0)
                {
                    if (lanzarExcepcion) throw new Exception($"La cantidad para el producto '{productoNombre}' no es válida o es cero.");
                    continue;
                }

                // 3. Validar Precio Unitario
                string precioUnitarioString = ObtenerValorCelda("PrecioUnitario");
                if (!decimal.TryParse(precioUnitarioString, System.Globalization.NumberStyles.Currency,
                                             System.Globalization.CultureInfo.CurrentCulture, out decimal precioUnitario) || precioUnitario < 0)
                {
                    if (lanzarExcepcion) throw new Exception($"El precio unitario para el producto '{productoNombre}' no es válido.");
                    continue;
                }

                // --- 4. RECONSTRUIR LA ENTIDAD ---
                _ordenActual.Detalles.Add(new ENTITY.DetalleOrdenCompra
                {
                    IdProducto = idProducto,
                    Producto = productoNombre,
                    Cantidad = cantidad,
                    PrecioUnitario = precioUnitario
                });

                // 5. Recalcular Subtotal en la Grilla 
                decimal subtotalCalculado = cantidad * precioUnitario;
                row["Subtotal"] = subtotalCalculado;
            }

            // 6. Actualizar el total de la cabecera            
            ActualizarTotal();

            // 7. Aplicar cambios al DataTable (solo si se está guardando o limpiando)
            if (lanzarExcepcion)
            {
                dt.AcceptChanges();
            }
        }

        private List<ENTITY.Producto> TraerProductosEntidad()
        {
            int idProveedor = 0;
            object selectedValue = cmbProveedor.SelectedValue;

            // 1. Manejar el caso de que no haya valor seleccionado (null o DBNull)
            if (selectedValue == null || selectedValue == DBNull.Value)
            {
                return new List<ENTITY.Producto>();
            }

            try
            {                
                System.Data.DataRowView drv = selectedValue as System.Data.DataRowView;

                if (drv != null)
                {
                    
                    idProveedor = Convert.ToInt32(drv.Row[0]);
                }
                else
                {
                    idProveedor = Convert.ToInt32(selectedValue);
                }
            }
            catch (Exception)
            {                
                return new List<ENTITY.Producto>();
            }
                        
            if (idProveedor <= 0)
            {
                return new List<ENTITY.Producto>();
            }
            
            return _productosBLL.ObtenerProductosDeProveedores(idProveedor);
        }

        private DataTable CrearDataTableProductosVacio()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            return dt;
        }

        private void cmbProveedor_SelectedValueChanged(object sender, EventArgs e)
        {           
            if (cmbProveedor.SelectedValue != null && cmbProveedor.SelectedValue != DBNull.Value)
            {
                // 1. Reconfigurar la grilla (esto cambia el DataSource del ComboBox)
                ConfigurarGrillaDetalle();

                // 2. Si NO es una modificación de una orden ya cargada, se limpian los detalles
                if (!_esModificacion || _ordenActual.Id == 0)
                {
                    _ordenActual.Detalles.Clear();

                    // Forzar la limpieza del DataTable subyacente del DGV
                    if (dgvDetalles.DataSource is DataTable dt)
                    {
                        dt.Clear();
                        dt.AcceptChanges();
                    }
                    RecalcularDetallesDesdeGrilla(false);
                }
                else
                {                    
                    CargarGrillaDetalleItems();
                }
            }
        }

        private void dgvDetalles_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.RowIndex >= 0)
            {
                string columnName = dgvDetalles.Columns[e.ColumnIndex].Name;

                if (columnName == "Producto")
                {                    
                    CargarPrecioReferenciaEnDetalle(e.RowIndex, e.ColumnIndex);
                }

                if (columnName == "Cantidad" || columnName == "PrecioUnitario" || columnName == "Producto")
                {
                    try
                    {                        
                        RecalcularDetallesDesdeGrilla(false);
                    }
                    catch (Exception)
                    {                        
                    }
                }
            }
        }

        private void CargarPrecioReferenciaEnDetalle(int rowIndex, int productoColumnIndex)
        {                        
            object cellValue = dgvDetalles.Rows[rowIndex].Cells[productoColumnIndex].Value;

            if (cellValue != null && cellValue != DBNull.Value)
            {
                int idProducto;                
                if (int.TryParse(cellValue.ToString(), out idProducto))
                {                    
                    ProductoProveedorBLL ppBLL = new ProductoProveedorBLL();                    
                    ENTITY.ProductoProveedor productoCompleto = ppBLL.TraerPorId(idProducto);

                    if (productoCompleto != null)
                    {                        
                        dgvDetalles.Rows[rowIndex].Cells["PrecioUnitario"].Value = productoCompleto.PrecioReferencia;
                    }
                }
            }
        }
    }
}