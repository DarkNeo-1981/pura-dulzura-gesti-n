namespace UI
{
    partial class LiquidacionSueldos
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProcesoTipo = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbTipoLiquidacion = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpPeriodo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLegajo = new System.Windows.Forms.ComboBox();
            this.lblLegajo = new System.Windows.Forms.Label();
            this.lblEstadoProceso = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLiquidar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvReciboDetalle = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReciboDetalle)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(66, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Proceso";
            // 
            // cmbProcesoTipo
            // 
            this.cmbProcesoTipo.FormattingEnabled = true;
            this.cmbProcesoTipo.Location = new System.Drawing.Point(139, 32);
            this.cmbProcesoTipo.Name = "cmbProcesoTipo";
            this.cmbProcesoTipo.Size = new System.Drawing.Size(278, 24);
            this.cmbProcesoTipo.TabIndex = 1;
            this.cmbProcesoTipo.SelectedIndexChanged += new System.EventHandler(this.cmbProcesoTipo_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbTipoLiquidacion);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpPeriodo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbLegajo);
            this.groupBox1.Controls.Add(this.lblLegajo);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(34, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(919, 81);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuracion de Liquidación";
            // 
            // cmbTipoLiquidacion
            // 
            this.cmbTipoLiquidacion.FormattingEnabled = true;
            this.cmbTipoLiquidacion.Location = new System.Drawing.Point(659, 33);
            this.cmbTipoLiquidacion.Name = "cmbTipoLiquidacion";
            this.cmbTipoLiquidacion.Size = new System.Drawing.Size(93, 24);
            this.cmbTipoLiquidacion.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(521, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tipo de Liquidación: ";
            // 
            // dtpPeriodo
            // 
            this.dtpPeriodo.CustomFormat = "MMM/yyyy";
            this.dtpPeriodo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPeriodo.Location = new System.Drawing.Point(389, 32);
            this.dtpPeriodo.Name = "dtpPeriodo";
            this.dtpPeriodo.ShowUpDown = true;
            this.dtpPeriodo.Size = new System.Drawing.Size(93, 22);
            this.dtpPeriodo.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(264, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Período a liquidar: ";
            // 
            // cmbLegajo
            // 
            this.cmbLegajo.FormattingEnabled = true;
            this.cmbLegajo.Location = new System.Drawing.Point(143, 34);
            this.cmbLegajo.Name = "cmbLegajo";
            this.cmbLegajo.Size = new System.Drawing.Size(93, 24);
            this.cmbLegajo.TabIndex = 3;
            // 
            // lblLegajo
            // 
            this.lblLegajo.AutoSize = true;
            this.lblLegajo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblLegajo.Location = new System.Drawing.Point(82, 37);
            this.lblLegajo.Name = "lblLegajo";
            this.lblLegajo.Size = new System.Drawing.Size(55, 16);
            this.lblLegajo.TabIndex = 3;
            this.lblLegajo.Text = "Legajo: ";
            // 
            // lblEstadoProceso
            // 
            this.lblEstadoProceso.AutoSize = true;
            this.lblEstadoProceso.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblEstadoProceso.Location = new System.Drawing.Point(256, 162);
            this.lblEstadoProceso.Name = "lblEstadoProceso";
            this.lblEstadoProceso.Size = new System.Drawing.Size(26, 16);
            this.lblEstadoProceso.TabIndex = 9;
            this.lblEstadoProceso.Text = "0%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(193, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Estado: ";
            // 
            // btnLiquidar
            // 
            this.btnLiquidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLiquidar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnLiquidar.Location = new System.Drawing.Point(34, 154);
            this.btnLiquidar.Name = "btnLiquidar";
            this.btnLiquidar.Size = new System.Drawing.Size(128, 32);
            this.btnLiquidar.TabIndex = 3;
            this.btnLiquidar.Text = "Iniciar Liquidación";
            this.btnLiquidar.UseVisualStyleBackColor = true;
            this.btnLiquidar.Click += new System.EventHandler(this.btnLiquidar_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvReciboDetalle);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(34, 201);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(919, 442);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detalle recubo individual calculado";
            // 
            // dgvReciboDetalle
            // 
            this.dgvReciboDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReciboDetalle.Location = new System.Drawing.Point(6, 21);
            this.dgvReciboDetalle.Name = "dgvReciboDetalle";
            this.dgvReciboDetalle.RowHeadersWidth = 51;
            this.dgvReciboDetalle.RowTemplate.Height = 24;
            this.dgvReciboDetalle.Size = new System.Drawing.Size(907, 415);
            this.dgvReciboDetalle.TabIndex = 0;
            // 
            // LiquidacionSueldos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(996, 677);
            this.Controls.Add(this.lblEstadoProceso);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnLiquidar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbProcesoTipo);
            this.Controls.Add(this.label1);
            this.Name = "LiquidacionSueldos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LiquidacionSueldos";
            this.Load += new System.EventHandler(this.LiquidacionSueldos_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReciboDetalle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProcesoTipo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpPeriodo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLegajo;
        private System.Windows.Forms.Label lblLegajo;
        private System.Windows.Forms.ComboBox cmbTipoLiquidacion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLiquidar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblEstadoProceso;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvReciboDetalle;
    }
}