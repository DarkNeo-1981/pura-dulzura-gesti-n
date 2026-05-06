namespace UI
{
    partial class DetalleOrdenCompra
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblIdOrden = new System.Windows.Forms.Label();
            this.gbCabecera = new System.Windows.Forms.GroupBox();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProveedor = new System.Windows.Forms.ComboBox();
            this.LblProveedor = new System.Windows.Forms.Label();
            this.dtpFechaEmision = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDetalle = new System.Windows.Forms.GroupBox();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.gbAccion = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.gbCabecera.SuspendLayout();
            this.gbDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.gbAccion.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIdOrden
            // 
            this.lblIdOrden.AutoSize = true;
            this.lblIdOrden.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblIdOrden.Location = new System.Drawing.Point(43, 27);
            this.lblIdOrden.Name = "lblIdOrden";
            this.lblIdOrden.Size = new System.Drawing.Size(44, 16);
            this.lblIdOrden.TabIndex = 0;
            this.lblIdOrden.Text = "label1";
            // 
            // gbCabecera
            // 
            this.gbCabecera.Controls.Add(this.txtEstado);
            this.gbCabecera.Controls.Add(this.label3);
            this.gbCabecera.Controls.Add(this.cmbProveedor);
            this.gbCabecera.Controls.Add(this.LblProveedor);
            this.gbCabecera.Controls.Add(this.dtpFechaEmision);
            this.gbCabecera.Controls.Add(this.label2);
            this.gbCabecera.Controls.Add(this.label1);
            this.gbCabecera.Controls.Add(this.lblIdOrden);
            this.gbCabecera.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.gbCabecera.Location = new System.Drawing.Point(11, 10);
            this.gbCabecera.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbCabecera.Name = "gbCabecera";
            this.gbCabecera.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbCabecera.Size = new System.Drawing.Size(763, 63);
            this.gbCabecera.TabIndex = 1;
            this.gbCabecera.TabStop = false;
            this.gbCabecera.Text = "Cabecera";
            // 
            // txtEstado
            // 
            this.txtEstado.Location = new System.Drawing.Point(632, 26);
            this.txtEstado.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.Size = new System.Drawing.Size(117, 22);
            this.txtEstado.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(573, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Estado";
            // 
            // cmbProveedor
            // 
            this.cmbProveedor.FormattingEnabled = true;
            this.cmbProveedor.Location = new System.Drawing.Point(407, 25);
            this.cmbProveedor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(152, 24);
            this.cmbProveedor.TabIndex = 5;
            this.cmbProveedor.SelectedIndexChanged += new System.EventHandler(this.cmbProveedor_SelectedValueChanged);
            this.cmbProveedor.SelectedValueChanged += new System.EventHandler(this.cmbProveedor_SelectedValueChanged);
            // 
            // LblProveedor
            // 
            this.LblProveedor.AutoSize = true;
            this.LblProveedor.Location = new System.Drawing.Point(330, 27);
            this.LblProveedor.Name = "LblProveedor";
            this.LblProveedor.Size = new System.Drawing.Size(71, 16);
            this.LblProveedor.TabIndex = 4;
            this.LblProveedor.Text = "Proveedor";
            // 
            // dtpFechaEmision
            // 
            this.dtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaEmision.Location = new System.Drawing.Point(204, 23);
            this.dtpFechaEmision.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpFechaEmision.Name = "dtpFechaEmision";
            this.dtpFechaEmision.Size = new System.Drawing.Size(112, 22);
            this.dtpFechaEmision.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha Emisión";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID";
            // 
            // gbDetalle
            // 
            this.gbDetalle.Controls.Add(this.dgvDetalles);
            this.gbDetalle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.gbDetalle.Location = new System.Drawing.Point(11, 78);
            this.gbDetalle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbDetalle.Name = "gbDetalle";
            this.gbDetalle.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbDetalle.Size = new System.Drawing.Size(763, 217);
            this.gbDetalle.TabIndex = 2;
            this.gbDetalle.TabStop = false;
            this.gbDetalle.Text = "Detalle";
            // 
            // dgvDetalles
            // 
            this.dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalles.Location = new System.Drawing.Point(5, 20);
            this.dgvDetalles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.RowHeadersWidth = 51;
            this.dgvDetalles.RowTemplate.Height = 28;
            this.dgvDetalles.Size = new System.Drawing.Size(752, 192);
            this.dgvDetalles.TabIndex = 0;
            this.dgvDetalles.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetalles_CellValueChanged_1);
            // 
            // gbAccion
            // 
            this.gbAccion.Controls.Add(this.label5);
            this.gbAccion.Controls.Add(this.lblTotal);
            this.gbAccion.Controls.Add(this.btnGuardar);
            this.gbAccion.Controls.Add(this.btnRemoveItem);
            this.gbAccion.Controls.Add(this.btnAddItem);
            this.gbAccion.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.gbAccion.Location = new System.Drawing.Point(16, 299);
            this.gbAccion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbAccion.Name = "gbAccion";
            this.gbAccion.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbAccion.Size = new System.Drawing.Size(757, 80);
            this.gbAccion.TabIndex = 3;
            this.gbAccion.TabStop = false;
            this.gbAccion.Text = "Acción";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(466, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Subtotal";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(365, 38);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(56, 16);
            this.lblTotal.TabIndex = 8;
            this.lblTotal.Text = "Subtotal";
            // 
            // btnGuardar
            // 
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Location = new System.Drawing.Point(549, 30);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(185, 30);
            this.btnGuardar.TabIndex = 2;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveItem.Location = new System.Drawing.Point(178, 30);
            this.btnRemoveItem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(150, 30);
            this.btnRemoveItem.TabIndex = 1;
            this.btnRemoveItem.Text = "Quitar Item";
            this.btnRemoveItem.UseVisualStyleBackColor = true;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Location = new System.Drawing.Point(12, 30);
            this.btnAddItem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(150, 30);
            this.btnAddItem.TabIndex = 0;
            this.btnAddItem.Text = "Agregar Item";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // DetalleOrdenCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(796, 388);
            this.Controls.Add(this.gbAccion);
            this.Controls.Add(this.gbDetalle);
            this.Controls.Add(this.gbCabecera);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DetalleOrdenCompra";
            this.Text = "DetalleOrdenCompra";
            this.Load += new System.EventHandler(this.DetalleOrdenCompra_Load);
            this.gbCabecera.ResumeLayout(false);
            this.gbCabecera.PerformLayout();
            this.gbDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.gbAccion.ResumeLayout(false);
            this.gbAccion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblIdOrden;
        private System.Windows.Forms.GroupBox gbCabecera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Label LblProveedor;
        private System.Windows.Forms.DateTimePicker dtpFechaEmision;
        private System.Windows.Forms.GroupBox gbDetalle;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.GroupBox gbAccion;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEstado;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotal;
    }
}