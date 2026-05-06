using BLL;
using ENTITY;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class GestionOrdenesCompra : Form
    {
        private readonly OrdenCompraBLL _ordenCompraBLL;

        public GestionOrdenesCompra()
        {
            InitializeComponent();
            _ordenCompraBLL = new OrdenCompraBLL();
        }

        private void GestionOrdenesCompra_Load(object sender, EventArgs e)
        {
            // 1. Aplicar la estilización de la grilla 
            AplicarEstilosGrilla();

            // 2. Cargar los datos iniciales
            CargarGrilla();
        }

        // --- MÉTODOS DE SOPORTE ---

        private void AplicarEstilosGrilla()
        {            
            dgvOrdenesCompra.EnableHeadersVisualStyles = false; // Importante para aplicar estilos

            // Encabezados de Columna
            dgvOrdenesCompra.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SaddleBrown;
            dgvOrdenesCompra.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvOrdenesCompra.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            // Encabezados de Fila
            dgvOrdenesCompra.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(140, 90, 53);
            dgvOrdenesCompra.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;

            // Estilos de las celdas de datos 
            dgvOrdenesCompra.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvOrdenesCompra.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvOrdenesCompra.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AntiqueWhite;
        }

        private void CargarGrilla()
        {
            try
            {
                dgvOrdenesCompra.DataSource = _ordenCompraBLL.TraerTodasOrdenes();

                // Configurar la visibilidad/formato de las columnas
                ConfigurarGrilla();

                // Ajustar el tamaño del formulario al contenido (usando la lógica de AjustarTamanos)
                AjustarTamanos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las Órdenes de Compra: " + ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AjustarTamanos()
        {
            // --- 1. Ajustar la ALTURA ---

            // Calcular la altura de la grilla
            int numFilas = dgvOrdenesCompra.Rows.Count;
            int alturaFilas = dgvOrdenesCompra.Rows.Cast<DataGridViewRow>().Where(r => r.Visible).Sum(r => r.Height);

            // Si la grilla está vacía o tiene pocas filas, s limitan a un máximo para que no sea muy pequeña.
            if (numFilas < 5)
            {
                dgvOrdenesCompra.Height = (dgvOrdenesCompra.ColumnHeadersHeight * 6) + 15;
            }
            else
            {
                dgvOrdenesCompra.Height = alturaFilas + dgvOrdenesCompra.ColumnHeadersHeight + 15;
            }

            // Ajustar la altura del GroupBox
            gbListadoOrdenes.Height = dgvOrdenesCompra.Location.Y + dgvOrdenesCompra.Height + 20;


            // --- 2. Ajustar el ANCHO (Corrección de Superposición) ---

            // 2.1. Definir la posición de los botones 
            int inicioBotonesX = btnRegistrarOrden.Location.X;            
            int margenSeparacion = 20;
            int anchoRequeridoGroupBox = inicioBotonesX - gbListadoOrdenes.Location.X - margenSeparacion;

            // Asegurar un ancho mínimo para que las columnas FILL se vean bien
            int anchoMinimoGroupBox = 750; // Ancho mínimo razonable

            // Asignar el ancho final al GroupBox
            gbListadoOrdenes.Width = Math.Max(anchoRequeridoGroupBox, anchoMinimoGroupBox);

            // 2.2. Asegurar que la DGV ocupe todo el ancho del GroupBox (CLAVE para FILL)
            dgvOrdenesCompra.Width = gbListadoOrdenes.Width - 20; // 10px de padding izquierdo y 10px derecho

            // --- 3. Redimensionar el Formulario ---

            // El ancho del formulario debe basarse en el borde derecho del botón más a la derecha (btnCancelarOrden)
            int botonesDerecha = btnCancelarOrden.Location.X + btnCancelarOrden.Width;

            // Ajustamos el ancho del formulario (Borde derecho de los botones + 50 px de margen)
            this.Width = botonesDerecha + 50;

            // Altura final del formulario
            this.Height = gbListadoOrdenes.Location.Y + gbListadoOrdenes.Height + 60;
        }

        private void ConfigurarGrilla()
        {
            if (dgvOrdenesCompra.Columns.Count == 0) return;

            // 1. Configuración de Distribución
            dgvOrdenesCompra.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvOrdenesCompra.RowHeadersWidth = 35;

            // Ocultar ID y renombrar
            if (dgvOrdenesCompra.Columns.Contains("Id"))
            {
                dgvOrdenesCompra.Columns["Id"].Visible = false;
            }

            // 2. Columna Principal (FILL)
            if (dgvOrdenesCompra.Columns.Contains("RazonSocialProveedor"))
            {
                dgvOrdenesCompra.Columns["RazonSocialProveedor"].HeaderText = "Proveedor";
                dgvOrdenesCompra.Columns["RazonSocialProveedor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvOrdenesCompra.Columns["RazonSocialProveedor"].FillWeight = 250; // Gran peso
            }

            // 3. Columna de Valores Fijos
            if (dgvOrdenesCompra.Columns.Contains("Total"))
            {
                dgvOrdenesCompra.Columns["Total"].DefaultCellStyle.Format = "C2"; // Formato de moneda
                dgvOrdenesCompra.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvOrdenesCompra.Columns["Total"].Width = 90; // Ancho fijo
            }

            // Estado de la orden
            if (dgvOrdenesCompra.Columns.Contains("Estado"))
            {
                dgvOrdenesCompra.Columns["Estado"].Width = 90; // Ancho fijo
            }

            // Ejemplo de otras columnas comunes en una Orden
            if (dgvOrdenesCompra.Columns.Contains("FechaCreacion"))
            {
                dgvOrdenesCompra.Columns["FechaCreacion"].Width = 90; // Ancho fijo para fecha
            }

            if (dgvOrdenesCompra.Columns.Contains("NumeroOrden"))
            {
                dgvOrdenesCompra.Columns["NumeroOrden"].Width = 80; // Ancho fijo
            }
        }

        // --- MANEJADORES DE EVENTOS (BOTONES) ---

        // 1. Alta (Registrar)
        private void btnRegistrarOrden_Click(object sender, EventArgs e)
        {
            // Abres el formulario DetalleOrdenCompra en modo Alta
            DetalleOrdenCompra detalleForm = new DetalleOrdenCompra();
            if (detalleForm.ShowDialog() == DialogResult.OK)
            {
                CargarGrilla();
            }
        }

        // 2. Borrado Lógico (Cancelar Orden)
        private void btnCancelarOrden_Click(object sender, EventArgs e)
        {
            if (dgvOrdenesCompra.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden para cancelar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idOrden = (int)dgvOrdenesCompra.SelectedRows[0].Cells["Id"].Value;

            if (MessageBox.Show($"¿Desea realmente cancelar la Orden de Compra {idOrden}?", "Confirmar Cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Regla de Negocio simple: Solo se cancelan órdenes Pendientes o Emitidas.
                string estadoActual = dgvOrdenesCompra.SelectedRows[0].Cells["Estado"].Value.ToString();

                if (estadoActual == "Recibida")
                {
                    MessageBox.Show("No se puede cancelar una orden que ya ha sido recibida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_ordenCompraBLL.CambiarEstadoOrden(idOrden, "Cancelada") > 0)
                {
                    MessageBox.Show("Orden de Compra cancelada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrilla();
                }
                else
                {
                    MessageBox.Show("Error al cancelar la orden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnModificarOrden_Click(object sender, EventArgs e)
        {
            if (dgvOrdenesCompra.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione la orden de compra que desea modificar.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Obtener el ID de la fila seleccionada
                int idOrden = (int)dgvOrdenesCompra.SelectedRows[0].Cells["Id"].Value;
                string estadoActual = dgvOrdenesCompra.SelectedRows[0].Cells["Estado"].Value.ToString();

                // Regla de Negocio: Solo permitir modificar órdenes pendientes de recibir
                if (estadoActual == "Recibida" || estadoActual == "Cancelada")
                {
                    MessageBox.Show($"No se puede modificar la Orden {idOrden} porque su estado es '{estadoActual}'.",
                                    "Error de Modificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Abrir el formulario DetalleOrdenCompra en modo Modificación (usando el constructor con ID)
                DetalleOrdenCompra detalleForm = new DetalleOrdenCompra(idOrden);

                if (detalleForm.ShowDialog() == DialogResult.OK)
                {
                    // 3. Recargar la grilla si la modificación fue exitosa
                    CargarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar modificar la orden: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecibirOrden_Click(object sender, EventArgs e)
        {
            if (dgvOrdenesCompra.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione la orden de compra que desea marcar como recibida.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idOrden = (int)dgvOrdenesCompra.SelectedRows[0].Cells["Id"].Value;
            string estadoActual = dgvOrdenesCompra.SelectedRows[0].Cells["Estado"].Value.ToString();

            // 1. Validar el estado actual
            if (estadoActual == "Recibida")
            {
                MessageBox.Show($"La Orden {idOrden} ya se encuentra en estado 'Recibida'.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (estadoActual == "Cancelada")
            {
                MessageBox.Show($"La Orden {idOrden} fue cancelada y no puede ser recibida.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"¿Confirma que ha recibido la mercadería de la Orden {idOrden}?", "Confirmar Recepción",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // 2. Llamar a la BLL para cambiar el estado a "Recibida"
                    if (_ordenCompraBLL.CambiarEstadoOrden(idOrden, "Recibida") > 0)
                    {
                        MessageBox.Show("Orden de Compra marcada como Recibida correctamente.", "Éxito",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarGrilla();
                    }
                    else
                    {
                        MessageBox.Show("Error al marcar la orden como recibida.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error durante la recepción: " + ex.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelarOrden_Click_1(object sender, EventArgs e)
        {
            // 1. Verificación de selección
    if (dgvOrdenesCompra.SelectedRows.Count == 0)
    {
        MessageBox.Show("Seleccione una orden para cancelar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    try
    {
        int idOrden = (int)dgvOrdenesCompra.SelectedRows[0].Cells["Id"].Value;
        string estadoActual = dgvOrdenesCompra.SelectedRows[0].Cells["Estado"].Value.ToString();

        // 2. Regla de Negocio: Evitar cancelar órdenes que ya se completaron.
        if (estadoActual == "Recibida")
        {
            MessageBox.Show("No se puede cancelar una orden que ya ha sido recibida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        // Evitar doble cancelación
        if (estadoActual == "Cancelada")
        {
            MessageBox.Show("La orden ya se encuentra en estado 'Cancelada'.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 3. Confirmación del usuario
        if (MessageBox.Show($"¿Desea realmente cancelar la Orden de Compra {idOrden}? Esta acción no se puede revertir fácilmente.", 
                            "Confirmar Cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            // 4. Llamada a la BLL para cambiar el estado
            if (_ordenCompraBLL.CambiarEstadoOrden(idOrden, "Cancelada") > 0)
            {
                MessageBox.Show("Orden de Compra cancelada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 5. Refrescar la grilla
                CargarGrilla();
            }
            else
            {
                MessageBox.Show("Error al cancelar la orden. Intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error inesperado al procesar la cancelación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
        }
    }
}