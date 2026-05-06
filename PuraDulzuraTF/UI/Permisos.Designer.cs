namespace UI
{
    partial class Permisos
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
            this.btnRemoverRol = new System.Windows.Forms.Button();
            this.tvPermisos = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAgregarPermiso = new System.Windows.Forms.Button();
            this.tvMenus = new System.Windows.Forms.TreeView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRemoverPermiso = new System.Windows.Forms.Button();
            this.tvPermisosHabilitados = new System.Windows.Forms.TreeView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAgregarRol = new System.Windows.Forms.Button();
            this.txtRol = new System.Windows.Forms.TextBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemoverRol);
            this.groupBox1.Controls.Add(this.tvPermisos);
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(31, 26);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(280, 466);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Roles disponibles";
            // 
            // btnRemoverRol
            // 
            this.btnRemoverRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoverRol.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoverRol.ForeColor = System.Drawing.Color.White;
            this.btnRemoverRol.Location = new System.Drawing.Point(5, 426);
            this.btnRemoverRol.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemoverRol.Name = "btnRemoverRol";
            this.btnRemoverRol.Size = new System.Drawing.Size(270, 30);
            this.btnRemoverRol.TabIndex = 1;
            this.btnRemoverRol.Text = "Quitar rol";
            this.btnRemoverRol.UseVisualStyleBackColor = true;
            this.btnRemoverRol.Click += new System.EventHandler(this.btnRemoverRol_Click);
            // 
            // tvPermisos
            // 
            this.tvPermisos.Location = new System.Drawing.Point(5, 20);
            this.tvPermisos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvPermisos.Name = "tvPermisos";
            this.tvPermisos.Size = new System.Drawing.Size(270, 402);
            this.tvPermisos.TabIndex = 0;
            this.tvPermisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPermisos_AfterSelect);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAgregarPermiso);
            this.groupBox2.Controls.Add(this.tvMenus);
            this.groupBox2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Location = new System.Drawing.Point(316, 26);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(280, 569);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Permisos";
            // 
            // btnAgregarPermiso
            // 
            this.btnAgregarPermiso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarPermiso.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarPermiso.ForeColor = System.Drawing.Color.White;
            this.btnAgregarPermiso.Location = new System.Drawing.Point(6, 525);
            this.btnAgregarPermiso.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAgregarPermiso.Name = "btnAgregarPermiso";
            this.btnAgregarPermiso.Size = new System.Drawing.Size(270, 30);
            this.btnAgregarPermiso.TabIndex = 2;
            this.btnAgregarPermiso.Text = "Agregar permiso";
            this.btnAgregarPermiso.UseVisualStyleBackColor = true;
            this.btnAgregarPermiso.Click += new System.EventHandler(this.btnAgregarPermiso_Click);
            // 
            // tvMenus
            // 
            this.tvMenus.Location = new System.Drawing.Point(5, 20);
            this.tvMenus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvMenus.Name = "tvMenus";
            this.tvMenus.Size = new System.Drawing.Size(269, 497);
            this.tvMenus.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRemoverPermiso);
            this.groupBox3.Controls.Add(this.tvPermisosHabilitados);
            this.groupBox3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Location = new System.Drawing.Point(602, 26);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(280, 569);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Permisos habilitados";
            // 
            // btnRemoverPermiso
            // 
            this.btnRemoverPermiso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoverPermiso.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoverPermiso.ForeColor = System.Drawing.Color.White;
            this.btnRemoverPermiso.Location = new System.Drawing.Point(4, 525);
            this.btnRemoverPermiso.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemoverPermiso.Name = "btnRemoverPermiso";
            this.btnRemoverPermiso.Size = new System.Drawing.Size(270, 30);
            this.btnRemoverPermiso.TabIndex = 2;
            this.btnRemoverPermiso.Text = "Quitar permiso";
            this.btnRemoverPermiso.UseVisualStyleBackColor = true;
            this.btnRemoverPermiso.Click += new System.EventHandler(this.btnRemoverPermiso_Click);
            // 
            // tvPermisosHabilitados
            // 
            this.tvPermisosHabilitados.Location = new System.Drawing.Point(5, 20);
            this.tvPermisosHabilitados.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvPermisosHabilitados.Name = "tvPermisosHabilitados";
            this.tvPermisosHabilitados.Size = new System.Drawing.Size(269, 497);
            this.tvPermisosHabilitados.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnAgregarRol);
            this.groupBox4.Controls.Add(this.txtRol);
            this.groupBox4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Location = new System.Drawing.Point(30, 496);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(280, 99);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Nuevo Rol";
            // 
            // btnAgregarRol
            // 
            this.btnAgregarRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarRol.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarRol.ForeColor = System.Drawing.Color.White;
            this.btnAgregarRol.Location = new System.Drawing.Point(5, 54);
            this.btnAgregarRol.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAgregarRol.Name = "btnAgregarRol";
            this.btnAgregarRol.Size = new System.Drawing.Size(270, 30);
            this.btnAgregarRol.TabIndex = 1;
            this.btnAgregarRol.Text = "Agregar rol";
            this.btnAgregarRol.UseVisualStyleBackColor = true;
            this.btnAgregarRol.Click += new System.EventHandler(this.btnAgregarRol_Click);
            // 
            // txtRol
            // 
            this.txtRol.Location = new System.Drawing.Point(5, 20);
            this.txtRol.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRol.Name = "txtRol";
            this.txtRol.Size = new System.Drawing.Size(270, 22);
            this.txtRol.TabIndex = 0;
            // 
            // btnCerrar
            // 
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(322, 604);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(270, 30);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // Permisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(53)))));
            this.ClientSize = new System.Drawing.Size(908, 644);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Permisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Permisos";
            this.Load += new System.EventHandler(this.Permisos_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tvPermisos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRemoverRol;
        private System.Windows.Forms.Button btnAgregarPermiso;
        private System.Windows.Forms.TreeView tvMenus;
        private System.Windows.Forms.Button btnRemoverPermiso;
        private System.Windows.Forms.TreeView tvPermisosHabilitados;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtRol;
        private System.Windows.Forms.Button btnAgregarRol;
        private System.Windows.Forms.Button btnCerrar;
    }
}