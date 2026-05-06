namespace UI
{
    partial class GestionOrdenesCompra
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
            this.gbListadoOrdenes = new System.Windows.Forms.GroupBox();
            this.dgvOrdenesCompra = new System.Windows.Forms.DataGridView();
            this.btnRegistrarOrden = new System.Windows.Forms.Button();
            this.btnModificarOrden = new System.Windows.Forms.Button();
            this.btnRecibirOrden = new System.Windows.Forms.Button();
            this.btnCancelarOrden = new System.Windows.Forms.Button();
            this.gbListadoOrdenes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesCompra)).BeginInit();
            this.SuspendLayout();
            // 
            // gbListadoOrdenes
            // 
            this.gbListadoOrdenes.Controls.Add(this.dgvOrdenesCompra);
            this.gbListadoOrdenes.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.gbListadoOrdenes.Location = new System.Drawing.Point(11, 18);
            this.gbListadoOrdenes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbListadoOrdenes.Name = "gbListadoOrdenes";
            this.gbListadoOrdenes.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbListadoOrdenes.Size = new System.Drawing.Size(846, 325);
            this.gbListadoOrdenes.TabIndex = 0;
            this.gbListadoOrdenes.TabStop = false;
            this.gbListadoOrdenes.Text = "Ordenes de compra";
            // 
            // dgvOrdenesCompra
            // 
            this.dgvOrdenesCompra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenesCompra.Location = new System.Drawing.Point(16, 30);
            this.dgvOrdenesCompra.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvOrdenesCompra.Name = "dgvOrdenesCompra";
            this.dgvOrdenesCompra.RowHeadersWidth = 51;
            this.dgvOrdenesCompra.RowTemplate.Height = 28;
            this.dgvOrdenesCompra.Size = new System.Drawing.Size(824, 281);
            this.dgvOrdenesCompra.TabIndex = 0;
            // 
            // btnRegistrarOrden
            // 
            this.btnRegistrarOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrarOrden.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRegistrarOrden.Location = new System.Drawing.Point(882, 29);
            this.btnRegistrarOrden.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRegistrarOrden.Name = "btnRegistrarOrden";
            this.btnRegistrarOrden.Size = new System.Drawing.Size(144, 27);
            this.btnRegistrarOrden.TabIndex = 1;
            this.btnRegistrarOrden.Text = "Registrar Orden";
            this.btnRegistrarOrden.UseVisualStyleBackColor = true;
            // 
            // btnModificarOrden
            // 
            this.btnModificarOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificarOrden.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnModificarOrden.Location = new System.Drawing.Point(882, 68);
            this.btnModificarOrden.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnModificarOrden.Name = "btnModificarOrden";
            this.btnModificarOrden.Size = new System.Drawing.Size(144, 27);
            this.btnModificarOrden.TabIndex = 2;
            this.btnModificarOrden.Text = "Modificar Orden";
            this.btnModificarOrden.UseVisualStyleBackColor = true;
            this.btnModificarOrden.Click += new System.EventHandler(this.btnModificarOrden_Click);
            // 
            // btnRecibirOrden
            // 
            this.btnRecibirOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecibirOrden.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRecibirOrden.Location = new System.Drawing.Point(882, 107);
            this.btnRecibirOrden.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRecibirOrden.Name = "btnRecibirOrden";
            this.btnRecibirOrden.Size = new System.Drawing.Size(144, 27);
            this.btnRecibirOrden.TabIndex = 3;
            this.btnRecibirOrden.Text = "Recibir Orden";
            this.btnRecibirOrden.UseVisualStyleBackColor = true;
            this.btnRecibirOrden.Click += new System.EventHandler(this.btnRecibirOrden_Click);
            // 
            // btnCancelarOrden
            // 
            this.btnCancelarOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarOrden.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCancelarOrden.Location = new System.Drawing.Point(882, 150);
            this.btnCancelarOrden.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancelarOrden.Name = "btnCancelarOrden";
            this.btnCancelarOrden.Size = new System.Drawing.Size(144, 27);
            this.btnCancelarOrden.TabIndex = 4;
            this.btnCancelarOrden.Text = "Cancelar Orden";
            this.btnCancelarOrden.UseVisualStyleBackColor = true;
            this.btnCancelarOrden.Click += new System.EventHandler(this.btnCancelarOrden_Click_1);
            // 
            // GestionOrdenesCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(1038, 360);
            this.Controls.Add(this.btnCancelarOrden);
            this.Controls.Add(this.btnRecibirOrden);
            this.Controls.Add(this.btnModificarOrden);
            this.Controls.Add(this.btnRegistrarOrden);
            this.Controls.Add(this.gbListadoOrdenes);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GestionOrdenesCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GestionOrdenesCompra";
            this.Load += new System.EventHandler(this.GestionOrdenesCompra_Load);
            this.gbListadoOrdenes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesCompra)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbListadoOrdenes;
        private System.Windows.Forms.DataGridView dgvOrdenesCompra;
        private System.Windows.Forms.Button btnRegistrarOrden;
        private System.Windows.Forms.Button btnModificarOrden;
        private System.Windows.Forms.Button btnRecibirOrden;
        private System.Windows.Forms.Button btnCancelarOrden;
    }
}