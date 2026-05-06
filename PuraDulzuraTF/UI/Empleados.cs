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
    public partial class Empleados : Form
    {
        List<ENTITY.Vendedor> ListaVendedores;
        List<ENTITY.Supervisor> ListaSupervisores;
        List<ENTITY.Administrativo> ListaAdministrativos;
        List<ENTITY.Jefe> ListaJefes;
        List<ENTITY.Gerente> ListaGerentes;
        List<string[]> ListaPersonas;
        List<BLL.IPersona> ListaIPersonas;
        UI.Menu Principal;
        DataTable Superiores;
        private bool esNuevoRegistro = false;

        public Empleados()
        {
            InitializeComponent();
            Superiores = new DataTable();
            Superiores.Columns.Add("Id");
            Superiores.Columns.Add("Nombre");
            dgvEmpleados.EnableHeadersVisualStyles = false; // Importante
            dgvEmpleados.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmpleados.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvEmpleados.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmpleados.EnableHeadersVisualStyles = false;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            
        }

        private void Empleados_Load(object sender, EventArgs e)
        {
            rbEmpActivos.Checked = true;
            Principal = (Menu)this.MdiParent;
            ListaPersonas = new List<string[]>();
            ListaIPersonas = new List<BLL.IPersona>();
            CargarListaVendedores();
            CargarListaSupervisores();
            CargarListaAdministrativos();
            CargarListaGerentes();
            CargarListaJefes();
            CargarListaPersonas();
            ActualizarGrilla(false);
            CargarComboBoxCargo();

            dgvEmpleados.EnableHeadersVisualStyles = false; // Importante
            dgvEmpleados.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmpleados.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvEmpleados.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmpleados.EnableHeadersVisualStyles = false;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }
        
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ListaVendedores = null;
            ListaSupervisores = null;
            ListaAdministrativos = null;
            ListaJefes = null;
            ListaPersonas = null;
            ListaIPersonas = null;
            this.Close();
        }

        private void dgvEmpleados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow filaSeleccionada = dgvEmpleados.Rows[e.RowIndex];

                    if (filaSeleccionada.IsNewRow)
                    {
                        // MODO AGREGAR
                        esNuevoRegistro = true;
                        dgvEmpleados.ClearSelection();

                        btnAgregar.Enabled = true;
                        btnModificar.Enabled = false;
                        btnBorrar.Enabled = false;

                        // SOLUCIÓN CRÍTICA: Retrasar la limpieza y el foco.
                        // Esto fuerza al DGV a dejar de apuntar a la fila vacía antes de que el focus se mueva.
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            ResetFormulario();
                            txtDni.Focus();
                        });
                    }
                    else
                    {
                        // MODO MODIFICAR / CARGAR DATOS
                        esNuevoRegistro = false;
                        btnAgregar.Enabled = false;
                        btnModificar.Enabled = true;
                        btnBorrar.Enabled = true;

                        // Lógica COMPLETA para cargar datos del empleado seleccionado
                        txtDni.Text = filaSeleccionada.Cells[0].Value?.ToString() ?? string.Empty;
                        txtLegajo.Text = filaSeleccionada.Cells[1].Value?.ToString() ?? string.Empty;
                        txtNombre.Text = filaSeleccionada.Cells[2].Value?.ToString() ?? string.Empty;
                        txtApellido.Text = filaSeleccionada.Cells[3].Value?.ToString() ?? string.Empty;

                        string sexo = filaSeleccionada.Cells[4].Value?.ToString().ToUpper().Trim() ?? string.Empty;
                        rbSexoMasculino.Checked = (sexo == "M" || sexo == "MASCULINO");
                        rbSexoFemenino.Checked = (sexo == "F" || sexo == "FEMENINO");

                        txtEmail.Text = filaSeleccionada.Cells[5].Value?.ToString() ?? string.Empty;
                        txtCuil.Text = filaSeleccionada.Cells[6].Value?.ToString() ?? string.Empty;
                        txtSueldoBasico.Text = filaSeleccionada.Cells[7].Value?.ToString() ?? string.Empty;

                        if (filaSeleccionada.Cells[8].Value != null &&
                            DateTime.TryParse(filaSeleccionada.Cells[8].Value.ToString(), out DateTime fecha))
                        {
                            dtpFechaIngreso.Value = fecha;
                        }
                        else
                        {
                            dtpFechaIngreso.Value = DateTime.Now;
                        }

                        if (filaSeleccionada.Cells[9].Value != null && cmbSuperior.Items.Count > 0)
                        {
                            cmbSuperior.SelectedValue = filaSeleccionada.Cells[9].Value;
                        }
                        else
                        {
                            cmbSuperior.SelectedIndex = -1;
                        }

                        cmbCargo.Text = filaSeleccionada.Cells[10].Value?.ToString() ?? string.Empty;

                        if (filaSeleccionada.Cells[11].Value != null)
                        {
                            if (bool.TryParse(filaSeleccionada.Cells[11].Value.ToString(), out bool eliminado) && eliminado)
                            {
                                btnBorrar.Text = "Habilitar";
                            }
                            else
                            {
                                btnBorrar.Text = "Borrar";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del empleado. Error: {ex.Message}");
            }
        }

        private void CargarListaGerentes()
        {
            try
            {
                BLL.GerenteBLL mapJ = new BLL.GerenteBLL();
                ListaGerentes = mapJ.BuscarTodos(true);
                mapJ = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la carga del listado de gerentes"); }
        }

        private void CargarListaJefes()
        {
            try
            {
                BLL.JefeBLL mapJ = new BLL.JefeBLL();
                ListaJefes = mapJ.BuscarTodos(true);
                mapJ = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la carga del listado de jefes"); }
        }
        private void CargarListaSupervisores()
        {
            try
            {
                BLL.SupervisorBLL mapS = new BLL.SupervisorBLL();
                ListaSupervisores = mapS.BuscarTodos(true);
                mapS = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la carga del listado de supervisores"); }
        }
        private void CargarListaVendedores()
        {
            try
            {
                BLL.VendedorBLL mapT = new BLL.VendedorBLL();
                ListaVendedores = mapT.BuscarTodos(true);
                mapT = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la carga del listado de Vendedors"); }
        }
        private void CargarListaAdministrativos()
        {
            try
            {
                BLL.AdministrativoBLL mapA = new BLL.AdministrativoBLL();
                ListaAdministrativos = mapA.BuscarTodos(true);
                mapA = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la carga del listado de administrativos"); }
        }
        private void CargarListaPersonas()
        {
            try
            {
                ListaPersonas.Clear();
                ListaIPersonas.Clear();
                if (ListaGerentes != null)
                {
                    foreach (ENTITY.Gerente ent in ListaGerentes)
                    {
                        BLL.GerenteBLL j = new BLL.GerenteBLL();
                        ListaPersonas.Add(j.DevolverDatos(ent.DNI));
                        ListaIPersonas.Add(new BLL.GerenteBLL());
                    }
                }
                if (ListaJefes != null)
                {
                    foreach (ENTITY.Jefe ent in ListaJefes)
                    {
                        BLL.JefeBLL j = new BLL.JefeBLL();
                        ListaPersonas.Add(j.DevolverDatos(ent.DNI));
                        ListaIPersonas.Add(new BLL.JefeBLL());

                    }
                }
                if (ListaSupervisores != null)
                {
                    foreach (ENTITY.Supervisor ent in ListaSupervisores)
                    {
                        BLL.SupervisorBLL s = new BLL.SupervisorBLL();
                        ListaPersonas.Add(s.DevolverDatos(ent.DNI));
                        ListaIPersonas.Add(new BLL.SupervisorBLL());
                    }
                }
                if (ListaVendedores != null)
                {
                    foreach (ENTITY.Vendedor ent in ListaVendedores)
                    {
                        BLL.VendedorBLL t = new BLL.VendedorBLL();
                        ListaPersonas.Add(t.DevolverDatos(ent.DNI));
                        ListaIPersonas.Add(new BLL.VendedorBLL());
                    }
                }
                if (ListaAdministrativos != null)
                {
                    foreach (ENTITY.Administrativo ent in ListaAdministrativos)
                    {
                        BLL.AdministrativoBLL a = new BLL.AdministrativoBLL();
                        ListaPersonas.Add(a.DevolverDatos(ent.DNI));
                        ListaIPersonas.Add(new BLL.AdministrativoBLL());
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema con la carga del listado de personas"); }
        }
        private void ActualizarGrilla(bool mostrarInactivos)
        {
            try
            {
                // 1. Definición de Columnas (¡12 columnas definidas!)
                DataTable dt = new DataTable();
                dt.Columns.Add("DNI");
                dt.Columns.Add("Legajo", typeof(int));
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Apellido");
                dt.Columns.Add("Sexo");
                dt.Columns.Add("Email");
                dt.Columns.Add("Cuil");
                dt.Columns.Add("SueldoBasico");
                dt.Columns.Add("FechaIngreso");
                dt.Columns.Add("Superior");
                dt.Columns.Add("Cargo");
                dt.Columns.Add("Eliminado", typeof(bool)); // Columna 12 (índice 11)

                if (ListaPersonas != null)
                {
                    foreach (string[] s in ListaPersonas)
                    {                        
                        if (s.Length < 12) continue;
                        bool esEliminado = false;
                        if (!bool.TryParse(s[11], out esEliminado))
                        {                            
                            esEliminado = false;
                        }

                        if (mostrarInactivos == esEliminado)
                        {
                            DataRow dr = dt.NewRow();

                            // Mapeo (sigue el orden de 0 a 11):
                            dr["DNI"] = s[0];
                            dr["Legajo"] = int.Parse(s[1]); 
                            dr["Nombre"] = s[2];
                            dr["Apellido"] = s[3];
                            dr["Sexo"] = s[4];
                            dr["Email"] = s[5];
                            dr["Cuil"] = s[6];
                            dr["SueldoBasico"] = s[7];
                            dr["FechaIngreso"] = s[8];
                            dr["Superior"] = s[9];
                            dr["Cargo"] = s[10];
                            dr["Eliminado"] = esEliminado; 

                            dt.Rows.Add(dr);
                        }
                    }
                }
                dgvEmpleados.DataSource = dt;
                dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvEmpleados.Columns.Contains("Legajo"))
                {                    
                    dgvEmpleados.Sort(dgvEmpleados.Columns["Legajo"], ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show($"Ocurrió un problema al actualizar la grilla de empleados. Revise los tipos de datos (Ej: Legajo, Sueldo). Error: {ex.Message}");
            }
        }

        private void CargarComboBoxCargo()
        {
            cmbCargo.Items.Add("Gerente");
            cmbCargo.Items.Add("Jefe");
            cmbCargo.Items.Add("Supervisor");
            cmbCargo.Items.Add("Administrativo");
            cmbCargo.Items.Add("Vendedor");
        }

        private void cmbCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCargo.Text == "Administrativo")
                {
                    if (ListaPersonas != null)
                    {
                        Superiores.Clear();
                        foreach (string[] s in ListaPersonas)
                        {
                            if (s[10] == "Supervisor")
                            {
                                DataRow dr = Superiores.NewRow();
                                dr["Id"] = s[0];
                                dr["Nombre"] = s[2] + " " + s[3];
                                Superiores.Rows.Add(dr);
                            }
                        }
                        cmbSuperior.DataSource = Superiores;
                        cmbSuperior.DisplayMember = "Nombre";
                        cmbSuperior.ValueMember = "Id";
                        if (cmbSuperior.Items.Count > 0) { cmbSuperior.Enabled = true; }
                    }
                }
                else if (cmbCargo.Text == "Gerente")
                {
                    Superiores.Clear();
                    cmbSuperior.Enabled = false;
                }
                else if (cmbCargo.Text == "Jefe")
                {   
                    if (ListaPersonas != null)
                    {
                        Superiores.Clear();
                        foreach (string[] s in ListaPersonas)
                        {
                            if (s[10] == "Gerente")
                            {
                                DataRow dr = Superiores.NewRow();
                                dr["Id"] = s[0];
                                dr["Nombre"] = s[2] + " " + s[3];
                                Superiores.Rows.Add(dr);
                            }
                        }
                        cmbSuperior.DataSource = Superiores;
                        cmbSuperior.DisplayMember = "Nombre";
                        cmbSuperior.ValueMember = "Id";
                        if (cmbSuperior.Items.Count > 0) { cmbSuperior.Enabled = true; }
                    }
                }
                else if (cmbCargo.Text == "Supervisor")
                {
                    if (ListaPersonas != null)
                    {
                        Superiores.Clear();
                        foreach (string[] s in ListaPersonas)
                        {
                            if (s[10] == "Jefe")
                            {
                                DataRow dr = Superiores.NewRow();
                                dr["Id"] = s[0];
                                dr["Nombre"] = s[2] + " " + s[3];
                                Superiores.Rows.Add(dr);
                            }
                        }
                        cmbSuperior.DataSource = Superiores;
                        cmbSuperior.DisplayMember = "Nombre";
                        cmbSuperior.ValueMember = "Id";
                        if (cmbSuperior.Items.Count > 0) { cmbSuperior.Enabled = true; }
                    }
                }
                else if (cmbCargo.Text == "Vendedor")
                {
                    if (ListaPersonas != null)
                    {
                        Superiores.Clear();
                        foreach (string[] s in ListaPersonas)
                        {
                            if (s[10] == "Supervisor")
                            {
                                DataRow dr = Superiores.NewRow();
                                dr["Id"] = s[0];
                                dr["Nombre"] = s[2] + " " + s[3];
                                Superiores.Rows.Add(dr);
                            }
                        }
                        cmbSuperior.DataSource = Superiores;
                        cmbSuperior.DisplayMember = "Nombre";
                        cmbSuperior.ValueMember = "Id";
                        if (cmbSuperior.Items.Count > 0) { cmbSuperior.Enabled = true; }
                    }
                }
                if (cmbSuperior.Items.Count > 0) { cmbSuperior.SelectedIndex = 0; }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema durante la selección del cargo"); }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar campos esenciales
                if (string.IsNullOrEmpty(txtDni.Text) || string.IsNullOrEmpty(txtLegajo.Text) ||
                    string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellido.Text) ||
                    string.IsNullOrEmpty(txtCuil.Text) || string.IsNullOrEmpty(txtSueldoBasico.Text) ||
                    cmbCargo.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe completar todos los campos obligatorios: DNI, CUIL, Legajo, Nombre, Apellido, Sueldo Básico y Cargo.", "Error de Validación");
                    return;
                }

                // 2. Conversión de datos
                int dni, legajo;
                decimal sueldoBasico;

                if (!int.TryParse(txtDni.Text, out dni)) { MessageBox.Show("El DNI debe ser un número entero válido.", "Error de Conversión"); return; }
                if (!int.TryParse(txtLegajo.Text, out legajo)) { MessageBox.Show("El Legajo debe ser un número entero válido.", "Error de Conversión"); return; }

                // Conversión de Sueldo Básico (usamos InvariantCulture para evitar errores de formato regional)
                if (!decimal.TryParse(txtSueldoBasico.Text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture, out sueldoBasico))
                {
                    MessageBox.Show("El Sueldo Básico debe ser un valor monetario válido. Use punto o coma decimal según su configuración.", "Error de Conversión");
                    return;
                }

                // 3. Recopilar datos
                string cuil = txtCuil.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();
                string sexo;
                if (rbSexoMasculino.Checked) { sexo = "Masculino"; } else { sexo = "Femenino"; }
                string email = txtEmail.Text.Trim();
                DateTime fechaIngreso = dtpFechaIngreso.Value.Date;
                string categoria = cmbCargo.Text;
                bool eliminado = false; // Siempre 'false' al agregar
                // Superior DNI
                int dniSuperior = 0; // 0 para el Gerente o si no se aplica
                if (cmbSuperior.Enabled && cmbSuperior.SelectedValue != null)
                {
                    if (!int.TryParse(cmbSuperior.SelectedValue.ToString(), out dniSuperior))
                    {
                        MessageBox.Show("Error al obtener el DNI del Superior. Debe ser un número válido.", "Error de Conversión");
                        return;
                    }
                }

                // 4. Invocar el método de la BLL (Completo para las 5 clases)
                int resultado = 0;                
                if (categoria == "Gerente")
                {
                    BLL.GerenteBLL g = new BLL.GerenteBLL();
                    resultado = g.AgregarUno(dni, legajo, nombre, apellido, sexo, email, cuil, sueldoBasico, fechaIngreso, 0, eliminado); // Gerente no tiene superior, usamos 0
                }
                else if (categoria == "Jefe")
                {
                    BLL.JefeBLL j = new BLL.JefeBLL();
                    resultado = j.AgregarUno(dni, legajo, nombre, apellido, sexo, email, cuil, sueldoBasico, fechaIngreso, dniSuperior, eliminado);
                }
                else if (categoria == "Supervisor")
                {
                    BLL.SupervisorBLL s = new BLL.SupervisorBLL();
                    resultado = s.AgregarUno(dni, legajo, nombre, apellido, sexo, email, cuil, sueldoBasico, fechaIngreso, dniSuperior, eliminado);
                }
                else if (categoria == "Vendedor")
                {
                    BLL.VendedorBLL v = new BLL.VendedorBLL();
                    resultado = v.AgregarUno(dni, legajo, nombre, apellido, sexo, email, cuil, sueldoBasico, fechaIngreso, dniSuperior, eliminado);
                }
                else if (categoria == "Administrativo")
                {
                    BLL.AdministrativoBLL a = new BLL.AdministrativoBLL();
                    resultado = a.AgregarUno(dni, legajo, nombre, apellido, sexo, email, cuil, sueldoBasico, fechaIngreso, dniSuperior, eliminado);
                }
                else
                {
                    MessageBox.Show("Cargo no reconocido.", "Error de Lógica");
                    return;
                }

                // 5. Manejar el resultado
                if (resultado == 1)
                {
                    MessageBox.Show($"Empleado ({categoria}) agregado exitosamente", "Éxito");                    
                    Principal.AgregarALaBitacora($"Se agregó un nuevo {categoria}");
                }
                else
                {
                    MessageBox.Show("Ocurrió un problema, intente nuevamente (Posible DNI o Legajo duplicado).", "Error de Operación");
                }

                // 6. Recargar y limpiar
                CargarListaVendedores();
                CargarListaSupervisores();
                CargarListaAdministrativos();
                CargarListaGerentes(); 
                CargarListaJefes();
                CargarListaPersonas();
                ActualizarGrilla(false);
                ResetFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un problema al intentar agregar un empleado: {ex.Message}", "Error Inesperado");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleados.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un empleado primero de la grilla para modificar.", "Error de Validación");
                    return;
                }

                // 1. Validar y Convertir (Misma lógica que en btnAgregar_Click)
                if (string.IsNullOrEmpty(txtDni.Text) || string.IsNullOrEmpty(txtLegajo.Text) ||
                    string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellido.Text) ||
                    string.IsNullOrEmpty(txtCuil.Text) || string.IsNullOrEmpty(txtSueldoBasico.Text) ||
                    cmbCargo.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe completar todos los campos obligatorios.", "Error de Validación");
                    return;
                }

                int dni, legajo;
                decimal sueldoBasico;

                if (!int.TryParse(txtDni.Text, out dni)) { MessageBox.Show("El DNI debe ser un número entero válido.", "Error de Conversión"); return; }
                if (!int.TryParse(txtLegajo.Text, out legajo)) { MessageBox.Show("El Legajo debe ser un número entero válido.", "Error de Conversión"); return; }
                if (!decimal.TryParse(txtSueldoBasico.Text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture, out sueldoBasico))
                {
                    MessageBox.Show("El Sueldo Básico debe ser un valor monetario válido.", "Error de Conversión");
                    return;
                }

                // 2. Recopilar datos
                string cuil = txtCuil.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();
                string sexo;
                if (rbSexoMasculino.Checked) { sexo = "Masculino"; } else { sexo = "Femenino"; }
                string email = txtEmail.Text.Trim();
                DateTime fechaIngreso = dtpFechaIngreso.Value.Date;
                // El estado 'Eliminado' debe obtenerse de la fila seleccionada, ya que podría estar borrado lógicamente
                bool eliminado = bool.Parse(dgvEmpleados.SelectedRows[0].Cells["Eliminado"].Value.ToString());

                // Superior DNI
                int dniSuperior = 0;
                if (cmbSuperior.Enabled && cmbSuperior.SelectedValue != null)
                {
                    if (!int.TryParse(cmbSuperior.SelectedValue.ToString(), out dniSuperior))
                    {
                        MessageBox.Show("Error al obtener el DNI del Superior.", "Error de Conversión");
                        return;
                    }
                }

                // 3. Encontrar la instancia BLL
                // DNI original de la grilla para encontrar la instancia BLL
                int dniOriginal = int.Parse(dgvEmpleados.SelectedRows[0].Cells["DNI"].Value.ToString());
                int indexBLL = -1;

                // Asumo que ListaPersonas guarda la información de DNI en la primera posición (índice 0)
                for (int k = 0; k < ListaPersonas.Count; k++)
                {
                    if (int.Parse(ListaPersonas[k][0].ToString()) == dniOriginal)
                    {
                        indexBLL = k;
                        break;
                    }
                }

                if (indexBLL != -1)
                {
                    // 4. Llamar a ModificarUno con todos los parámetros
                    int resultado = ListaIPersonas[indexBLL].ModificarUno(
                        dni,
                        legajo,
                        nombre,
                        apellido,
                        sexo,
                        email,
                        cuil,
                        sueldoBasico,
                        fechaIngreso,
                        dniSuperior,
                        eliminado
                    );

                    if (resultado == 1)
                    {
                        MessageBox.Show("Empleado modificado exitosamente", "Éxito");
                        Principal.AgregarALaBitacora($"Se modificó el empleado {txtDni.Text}"); 
                    }
                    else
                    {
                        MessageBox.Show("Ocurrió un problema, intente nuevamente", "Error de Operación");
                    }

                    // 5. Recargar y limpiar
                    CargarListaVendedores();
                    CargarListaSupervisores();
                    CargarListaAdministrativos();
                    CargarListaGerentes(); 
                    CargarListaJefes();
                    CargarListaPersonas();
                    ActualizarGrilla(false);
                    ResetFormulario();
                }
                else
                {
                    MessageBox.Show("No se encontró la instancia de BLL para el empleado seleccionado.", "Error de Lógica");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un problema al intentar modificar un empleado: {ex.Message}", "Error Inesperado");
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleados.SelectedRows.Count > 0)
                {
                    bool geerenteTieneSupervisorACargo = false;
                    bool personaTieneOperador = false;
                    bool supervisorTieneVenACargo = false;
                    bool supervisorTieneAdmACargo = false;
                    bool jefeTieneSupACargo = false;
                    bool VendedorTieneOPAbiertaACargo = false;
                    bool sePuedeBorrar = false;
                    int i = int.Parse(dgvEmpleados.SelectedRows[0].Cells[0].Value.ToString());
                    int j = 0;
                    for (int k = 0; k < ListaIPersonas.Count; k++)
                    {
                        if (int.Parse(ListaPersonas[k][0].ToString()) == i) { j = k; break; }
                    }
                    BLL.UsuariosBLL usuario = new BLL.UsuariosBLL();
                    List<ENTITY.Usuarios> operadores = usuario.BuscarTodos();
                    if (operadores != null)
                    {
                        foreach (ENTITY.Usuarios o in operadores)
                        {
                            if (o.DNI == int.Parse(ListaPersonas[j][0]) && btnBorrar.Text == "Borrar") { personaTieneOperador = true; MessageBox.Show("No se puede eliminar el empleado ya que tiene un operador del sistema asignado"); break; }
                        }
                    }
                    if (ListaIPersonas[j].GetType().Name == "Gerente")
                    {
                        BLL.JefeBLL s = new BLL.JefeBLL();
                        List<ENTITY.Jefe> jefes = s.BuscarTodos(true);
                        if (jefes != null)
                        {
                            foreach (ENTITY.Jefe jefe in jefes)
                            {
                                if (jefe.DNI == int.Parse(ListaPersonas[j][0])) { geerenteTieneSupervisorACargo = true; MessageBox.Show("No se puede eliminar el empleado ya que tiene jefe a cargo"); break; }
                            }
                        }
                    }
                    else if (ListaIPersonas[j].GetType().Name == "Jefe")
                    {
                        BLL.SupervisorBLL s = new BLL.SupervisorBLL();
                        List<ENTITY.Supervisor> supervisores = s.BuscarTodos(true);
                        if (supervisores != null)
                        {
                            foreach (ENTITY.Supervisor sup in supervisores)
                            {
                                if (sup.DNI_Supervisor == int.Parse(ListaPersonas[j][0])) { jefeTieneSupACargo = true; MessageBox.Show("No se puede eliminar el empleado ya que tiene supervisores a cargo"); break; }
                            }
                        }
                    }
                    else if (ListaIPersonas[j].GetType().Name == "Supervisor")
                    {
                        BLL.VendedorBLL t = new BLL.VendedorBLL();
                        List<ENTITY.Vendedor> vendedores = t.BuscarTodos(true);
                        if (vendedores != null)
                        {
                            foreach (ENTITY.Vendedor ven in vendedores)
                            {
                                if (ven.DNI_Supervisor == int.Parse(ListaPersonas[j][0])) { supervisorTieneVenACargo = true; MessageBox.Show("No se puede eliminar el empleado ya que tiene Vendedors a cargo"); break; }
                            }
                        }
                        BLL.AdministrativoBLL a = new BLL.AdministrativoBLL();
                        List<ENTITY.Administrativo> administrativos = a.BuscarTodos(true);
                        if (administrativos != null)
                        {
                            foreach (ENTITY.Administrativo adm in administrativos)
                            {
                                if (adm.DNI_Supervisor == int.Parse(ListaPersonas[j][0])) { supervisorTieneAdmACargo = true; MessageBox.Show("No se puede eliminar el empleado ya que tiene administrativos a cargo"); break; }
                            }
                        }
                    }
                    else if (ListaIPersonas[j].GetType().Name == "Vendedor")
                    {
                        BLL.OrdenDePedidoBLL os = new BLL.OrdenDePedidoBLL();
                        List<ENTITY.OrdenDePedido> ordenesDePedidos = os.ObtenerTodos();
                        if (ordenesDePedidos != null)
                        {
                            foreach (ENTITY.OrdenDePedido o in ordenesDePedidos)
                            {
                                if (o.DNI_Vendedor == int.Parse(ListaPersonas[j][0]) && o.FechaDeVenta == string.Empty) { VendedorTieneOPAbiertaACargo = true; MessageBox.Show("No se puede eliminar el empleado ya que tiene órdenes de servicio abiertas a su cargo"); break; }
                            }
                        }
                    }
                    if (!personaTieneOperador && !supervisorTieneVenACargo && !supervisorTieneAdmACargo && !VendedorTieneOPAbiertaACargo && !jefeTieneSupACargo) { sePuedeBorrar = true; }
                    if (btnBorrar.Text == "Borrar")
                    {
                        if (sePuedeBorrar)
                        {
                            int resultado = ListaIPersonas[j].BorrarUno(i);                            
                            if (resultado == 1) 
                            { 
                                MessageBox.Show("Empleado borrado excitosamente"); 
                                Principal.AgregarALaBitacora($"Se eliminó el empleado {txtDni.Text}"); 
                            }
                            else { MessageBox.Show("Ocurrió un problema durante el borrado, intente nuevamente"); }
                        }
                    }
                    else
                    {
                        int resultado = ListaIPersonas[j].DeshacerBorrarUno(i);
                        if (resultado == 1) 
                        { 
                            MessageBox.Show("Empleado habilitado excitosamente"); Principal.AgregarALaBitacora($"Se habilitó el empleado {txtDni.Text}");
                            rbEmpActivos.Checked = true;
                            btnBorrar.Text = "Borrar";
                        }
                        else { MessageBox.Show("Ocurrió un problema durante la habilitación, intente nuevamente"); }
                    }
                    CargarListaVendedores();
                    CargarListaSupervisores();
                    CargarListaAdministrativos();
                    CargarListaJefes();
                    CargarListaPersonas();
                    ActualizarGrilla(false);
                    ResetFormulario();
                }
                else { MessageBox.Show("Debe seleccionar un empleado primero"); }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar borrar un empleado"); }
        }

        private void ResetFormulario()
        {
            try
            {
                txtDni.Text = string.Empty;
                txtLegajo.Text = string.Empty;
                txtNombre.Text = string.Empty;
                txtApellido.Text = string.Empty;
                txtCuil.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtSueldoBasico.Text = string.Empty;

                rbSexoMasculino.Checked = false;
                rbSexoFemenino.Checked = false;

                cmbCargo.SelectedIndex = -1;
                cmbSuperior.SelectedIndex = -1;

                dtpFechaIngreso.Value = DateTime.Now;

                btnBorrar.Text = "Borrar";
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un problema al intentar restablecer el formulario");
            }
        }

        private void rbEmpActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEmpActivos.Checked)
            {                
                ActualizarGrilla(false);
            }
        }

        private void rbEmpInactivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEmpInactivos.Checked)
            {                
                ActualizarGrilla(true);
            }
        }

        private void dgvEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtDni_Validated(object sender, EventArgs e)
        {
            CalcularCuil();
        }

        private void CalcularCuil()
        {
            string dni = txtDni.Text;
            string sexo = "";

            if (rbSexoMasculino.Checked)
            {
                sexo = "MASCULINO";
            }
            else if (rbSexoFemenino.Checked)
            {
                sexo = "FEMENINO";
            }
            else
            {
                txtCuil.Text = "SELECCIONE SEXO";
                return; // Salir si falta el sexo
            }

            if (string.IsNullOrWhiteSpace(dni))
            {
                txtCuil.Text = "";
                return; // Salir si falta el DNI
            }

            try
            {
                SERVICIOS.CalculoDelCuil calculador = new SERVICIOS.CalculoDelCuil();
                string cuil = calculador.CalcularCUIL(dni, sexo);
                txtCuil.Text = cuil;
            }
            catch (Exception)
            {
                // Manejar errores de cálculo, si los hay
                txtCuil.Text = "ERROR CÁLCULO";
            }
        }

        private void rbSexoFemenino_CheckedChanged(object sender, EventArgs e)
        {
            CalcularCuil();
        }

        private void rbSexoMasculino_CheckedChanged(object sender, EventArgs e)
        {
            CalcularCuil();
        }

        private void dgvEmpleados_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Solo si estamos haciendo clic en la fila de nuevo registro (IsNewRow)
            if (e.RowIndex >= 0 && dgvEmpleados.Rows[e.RowIndex].IsNewRow)
            {
                // Reforzamos la limpieza y la configuración de modo
                esNuevoRegistro = true;
                ResetFormulario();
                btnAgregar.Enabled = true;
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
            }
        }

        private void dgvEmpleados_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.RowIndex == dgvEmpleados.NewRowIndex)
            {
                e.Cancel = true;
            }
        }
    }
}
