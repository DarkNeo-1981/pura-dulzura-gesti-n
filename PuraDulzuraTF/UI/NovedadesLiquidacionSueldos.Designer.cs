namespace UI
{
    partial class NovedadesLiquidacionSueldos
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLegajo = new System.Windows.Forms.ComboBox();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpPeriodo = new System.Windows.Forms.DateTimePicker();
            this.cmbTipoNovedad = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblValorTipo = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.chkDescuento = new System.Windows.Forms.CheckBox();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.btnAgregarNovedad = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvNovedades = new System.Windows.Forms.DataGridView();
            this.btnGuardarNovedades = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovedades)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnAgregarNovedad);
            this.groupBox1.Controls.Add(this.txtObservacion);
            this.groupBox1.Controls.Add(this.chkDescuento);
            this.groupBox1.Controls.Add(this.txtValor);
            this.groupBox1.Controls.Add(this.lblValorTipo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbTipoNovedad);
            this.groupBox1.Controls.Add(this.dtpPeriodo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtNombre);
            this.groupBox1.Controls.Add(this.txtApellido);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDNI);
            this.groupBox1.Controls.Add(this.cmbLegajo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1058, 339);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registro Novedades";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "DNI Empleado:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Legajo:";
            // 
            // cmbLegajo
            // 
            this.cmbLegajo.FormattingEnabled = true;
            this.cmbLegajo.Location = new System.Drawing.Point(132, 34);
            this.cmbLegajo.Name = "cmbLegajo";
            this.cmbLegajo.Size = new System.Drawing.Size(168, 24);
            this.cmbLegajo.TabIndex = 2;
            this.cmbLegajo.SelectedIndexChanged += new System.EventHandler(this.cmbLegajo_SelectedIndexChanged);
            // 
            // txtDNI
            // 
            this.txtDNI.Location = new System.Drawing.Point(132, 77);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.ReadOnly = true;
            this.txtDNI.Size = new System.Drawing.Size(168, 22);
            this.txtDNI.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(583, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nombre:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(321, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Apellido:";
            // 
            // txtApellido
            // 
            this.txtApellido.Location = new System.Drawing.Point(387, 77);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.ReadOnly = true;
            this.txtApellido.Size = new System.Drawing.Size(168, 22);
            this.txtApellido.TabIndex = 6;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(648, 77);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(168, 22);
            this.txtNombre.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(323, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Período:";
            // 
            // dtpPeriodo
            // 
            this.dtpPeriodo.CustomFormat = "MMM/yyyy";
            this.dtpPeriodo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPeriodo.Location = new System.Drawing.Point(387, 37);
            this.dtpPeriodo.Name = "dtpPeriodo";
            this.dtpPeriodo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpPeriodo.ShowUpDown = true;
            this.dtpPeriodo.Size = new System.Drawing.Size(99, 22);
            this.dtpPeriodo.TabIndex = 9;
            // 
            // cmbTipoNovedad
            // 
            this.cmbTipoNovedad.FormattingEnabled = true;
            this.cmbTipoNovedad.Location = new System.Drawing.Point(132, 117);
            this.cmbTipoNovedad.Name = "cmbTipoNovedad";
            this.cmbTipoNovedad.Size = new System.Drawing.Size(168, 24);
            this.cmbTipoNovedad.TabIndex = 11;
            this.cmbTipoNovedad.SelectedIndexChanged += new System.EventHandler(this.cmbTipoNovedad_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tipo Novedad:";
            // 
            // lblValorTipo
            // 
            this.lblValorTipo.AutoSize = true;
            this.lblValorTipo.Location = new System.Drawing.Point(323, 122);
            this.lblValorTipo.Name = "lblValorTipo";
            this.lblValorTipo.Size = new System.Drawing.Size(100, 16);
            this.lblValorTipo.TabIndex = 14;
            this.lblValorTipo.Text = "Valor (Monto $):";
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(429, 119);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(168, 22);
            this.txtValor.TabIndex = 15;
            // 
            // chkDescuento
            // 
            this.chkDescuento.AutoSize = true;
            this.chkDescuento.Location = new System.Drawing.Point(621, 122);
            this.chkDescuento.Name = "chkDescuento";
            this.chkDescuento.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkDescuento.Size = new System.Drawing.Size(97, 20);
            this.chkDescuento.TabIndex = 16;
            this.chkDescuento.Text = ":Descuento";
            this.chkDescuento.UseVisualStyleBackColor = true;
            this.chkDescuento.CheckedChanged += new System.EventHandler(this.chkDescuento_CheckedChanged);
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(30, 180);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservacion.Size = new System.Drawing.Size(1011, 86);
            this.txtObservacion.TabIndex = 17;
            // 
            // btnAgregarNovedad
            // 
            this.btnAgregarNovedad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarNovedad.Location = new System.Drawing.Point(349, 288);
            this.btnAgregarNovedad.Name = "btnAgregarNovedad";
            this.btnAgregarNovedad.Size = new System.Drawing.Size(396, 26);
            this.btnAgregarNovedad.TabIndex = 18;
            this.btnAgregarNovedad.Text = "Agregar Novedad";
            this.btnAgregarNovedad.UseVisualStyleBackColor = true;
            this.btnAgregarNovedad.Click += new System.EventHandler(this.btnAgregarNovedad_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvNovedades);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(12, 357);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1058, 355);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Novedades Pendientes";
            // 
            // dgvNovedades
            // 
            this.dgvNovedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNovedades.Location = new System.Drawing.Point(15, 33);
            this.dgvNovedades.Name = "dgvNovedades";
            this.dgvNovedades.RowHeadersWidth = 51;
            this.dgvNovedades.RowTemplate.Height = 24;
            this.dgvNovedades.Size = new System.Drawing.Size(1026, 298);
            this.dgvNovedades.TabIndex = 0;
            // 
            // btnGuardarNovedades
            // 
            this.btnGuardarNovedades.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarNovedades.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnGuardarNovedades.Location = new System.Drawing.Point(361, 738);
            this.btnGuardarNovedades.Name = "btnGuardarNovedades";
            this.btnGuardarNovedades.Size = new System.Drawing.Size(396, 26);
            this.btnGuardarNovedades.TabIndex = 19;
            this.btnGuardarNovedades.Text = "Guardar Novedades";
            this.btnGuardarNovedades.UseVisualStyleBackColor = true;
            this.btnGuardarNovedades.Click += new System.EventHandler(this.btnGuardarNovedades_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 16);
            this.label8.TabIndex = 19;
            this.label8.Text = "Observaciones:";
            // 
            // NovedadesLiquidacionSueldos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(1082, 792);
            this.Controls.Add(this.btnGuardarNovedades);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "NovedadesLiquidacionSueldos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NovedadesLiquidacionSueldos";
            this.Load += new System.EventHandler(this.NovedadesLiquidacionSueldos_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovedades)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbLegajo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpPeriodo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbTipoNovedad;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label lblValorTipo;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.CheckBox chkDescuento;
        private System.Windows.Forms.Button btnAgregarNovedad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvNovedades;
        private System.Windows.Forms.Button btnGuardarNovedades;
        private System.Windows.Forms.Label label8;
    }
}