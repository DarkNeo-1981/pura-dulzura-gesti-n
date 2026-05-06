namespace UI
{
    partial class GestionPagosOC
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
            this.components = new System.ComponentModel.Container();
            this.dgvOrdenesCompras = new System.Windows.Forms.DataGridView();
            this.btnPagarOrden = new System.Windows.Forms.Button();
            this.cmsOpcionesPago = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMarcarComoPagada = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMarcarComoCancelada = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMarcarComoPagoParcial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMarcarComoDevolucion = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesCompras)).BeginInit();
            this.cmsOpcionesPago.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrdenesCompras
            // 
            this.dgvOrdenesCompras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenesCompras.ContextMenuStrip = this.cmsOpcionesPago;
            this.dgvOrdenesCompras.Location = new System.Drawing.Point(32, 42);
            this.dgvOrdenesCompras.Name = "dgvOrdenesCompras";
            this.dgvOrdenesCompras.RowHeadersWidth = 51;
            this.dgvOrdenesCompras.RowTemplate.Height = 24;
            this.dgvOrdenesCompras.Size = new System.Drawing.Size(1056, 359);
            this.dgvOrdenesCompras.TabIndex = 0;
            // 
            // btnPagarOrden
            // 
            this.btnPagarOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagarOrden.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnPagarOrden.Location = new System.Drawing.Point(828, 417);
            this.btnPagarOrden.Name = "btnPagarOrden";
            this.btnPagarOrden.Size = new System.Drawing.Size(260, 34);
            this.btnPagarOrden.TabIndex = 1;
            this.btnPagarOrden.Text = "Pagar Orden";
            this.btnPagarOrden.UseVisualStyleBackColor = true;
            this.btnPagarOrden.Click += new System.EventHandler(this.btnPagarOrden_Click);
            // 
            // cmsOpcionesPago
            // 
            this.cmsOpcionesPago.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsOpcionesPago.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMarcarComoPagada,
            this.tsmiMarcarComoCancelada,
            this.tsmiMarcarComoPagoParcial,
            this.tsmiMarcarComoDevolucion});
            this.cmsOpcionesPago.Name = "cmsOpcionesPago";
            this.cmsOpcionesPago.Size = new System.Drawing.Size(177, 100);
            // 
            // tsmiMarcarComoPagada
            // 
            this.tsmiMarcarComoPagada.Name = "tsmiMarcarComoPagada";
            this.tsmiMarcarComoPagada.Size = new System.Drawing.Size(176, 24);
            this.tsmiMarcarComoPagada.Text = "PAGADA";
            this.tsmiMarcarComoPagada.Click += new System.EventHandler(this.tsmi_CambiarEstadoPago_Click);
            // 
            // tsmiMarcarComoCancelada
            // 
            this.tsmiMarcarComoCancelada.Name = "tsmiMarcarComoCancelada";
            this.tsmiMarcarComoCancelada.Size = new System.Drawing.Size(176, 24);
            this.tsmiMarcarComoCancelada.Text = "CANCELADA";
            this.tsmiMarcarComoCancelada.Click += new System.EventHandler(this.tsmi_CambiarEstadoPago_Click);
            // 
            // tsmiMarcarComoPagoParcial
            // 
            this.tsmiMarcarComoPagoParcial.Name = "tsmiMarcarComoPagoParcial";
            this.tsmiMarcarComoPagoParcial.Size = new System.Drawing.Size(176, 24);
            this.tsmiMarcarComoPagoParcial.Text = "PAGO PARCIAL";
            this.tsmiMarcarComoPagoParcial.Click += new System.EventHandler(this.tsmi_CambiarEstadoPago_Click);
            // 
            // tsmiMarcarComoDevolucion
            // 
            this.tsmiMarcarComoDevolucion.Name = "tsmiMarcarComoDevolucion";
            this.tsmiMarcarComoDevolucion.Size = new System.Drawing.Size(176, 24);
            this.tsmiMarcarComoDevolucion.Text = "DEVOLUCION";
            this.tsmiMarcarComoDevolucion.Click += new System.EventHandler(this.tsmi_CambiarEstadoPago_Click);
            // 
            // GestionPagosOC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(1100, 467);
            this.Controls.Add(this.btnPagarOrden);
            this.Controls.Add(this.dgvOrdenesCompras);
            this.Name = "GestionPagosOC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GestionPagosOC";
            this.Load += new System.EventHandler(this.GestionPagosOC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesCompras)).EndInit();
            this.cmsOpcionesPago.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrdenesCompras;
        private System.Windows.Forms.Button btnPagarOrden;
        private System.Windows.Forms.ContextMenuStrip cmsOpcionesPago;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarcarComoPagada;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarcarComoCancelada;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarcarComoPagoParcial;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarcarComoDevolucion;
    }
}