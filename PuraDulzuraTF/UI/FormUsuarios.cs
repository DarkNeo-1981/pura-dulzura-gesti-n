using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using svc = SERVICIOS;

namespace UI
{
    public partial class FormUsuarios : Form
    {
        List<ENTITY.Usuarios> ListaUsuarios;
        List<ENTITY.Vendedor> ListaVendedores;
        List<ENTITY.Supervisor> ListaSupervisores;
        List<ENTITY.Administrativo> ListaAdministrativos;
        List<ENTITY.Gerente> ListaGerentes;
        List<ENTITY.Jefe> ListaJefes;
        List<ENTITY.Encargado> ListaEncargados;
        List<ENTITY.Permiso> ListaPermisos;
        DataTable permisos;
        DataTable personas;
        List<string[]> ListaPersonas;
        UI.Menu Principal;

        public FormUsuarios()
        {
            InitializeComponent();
            permisos = new DataTable();
            permisos.Columns.Add("Id");
            permisos.Columns.Add("Descripcion");
            personas = new DataTable();
            personas.Columns.Add("DNI");
            personas.Columns.Add("Nombre");
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;
            ListaPersonas = new List<string[]>();
            dgvUsuarios.EnableHeadersVisualStyles = false; // Importante
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.SaddleBrown;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 90, 53);
            dgvUsuarios.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);            
            CargarListaUsuarios();
            CargarListaVendedores();
            CargarListaSupervisores();
            CargarListaAdministrativos();
            CargarListaJefes();
            CargarListaGerentes();
            CargarListaEncargados();
            CargarListaPermisos();
            CargarComboPersonas();
            if (cmbPersona.Items.Count == 0) { btnAgregar.Enabled = false; }
            CargarComboPermisos();
            ActualizarGrilla();            
        }

        private void dgvUsuarios_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count > 0)
                {
                    var fila = dgvUsuarios.SelectedRows[0];

                    txtUsuario.Text = fila.Cells[1].Value?.ToString() ?? "";

                    if (fila.Cells[2].Value != null && cmbPersona.DataSource != null)
                        cmbPersona.SelectedValue = fila.Cells[2].Value;
                    else
                        cmbPersona.SelectedIndex = -1;

                    cmbPermiso.Text = fila.Cells[4].Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema con la grilla de Usuarios\n" + ex.Message);
            }
        }

        private void CargarListaVendedores()
        {
            try
            {
                BLL.VendedorBLL mapT = new BLL.VendedorBLL();
                ListaVendedores = mapT.BuscarTodos(false);
                mapT = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de Vendedores"); }
        }

        private void CargarListaUsuarios()
        {
            try
            {
                BLL.UsuariosBLL mapO = new BLL.UsuariosBLL();
                ListaUsuarios = mapO.BuscarTodos();
                mapO = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de usuarios"); }
        }

        private void CargarListaGerentes()
        {
            try
            {
                BLL.GerenteBLL mapJ = new BLL.GerenteBLL();
                ListaGerentes = mapJ.BuscarTodos(false);
                mapJ = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de gerentes"); }
        }

        private void CargarListaJefes()
        {
            try
            {
                BLL.JefeBLL mapJ = new BLL.JefeBLL();
                ListaJefes = mapJ.BuscarTodos(false);
                mapJ = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de jefes"); }
        }

        private void CargarListaSupervisores()
        {
            try
            {
                BLL.SupervisorBLL mapS = new BLL.SupervisorBLL();
                ListaSupervisores = mapS.BuscarTodos(false);
                mapS = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de supervisores"); }
        }
        private void CargarListaVendedors()
        {
            try
            {
                BLL.VendedorBLL mapT = new BLL.VendedorBLL();
                ListaVendedores = mapT.BuscarTodos(false);
                mapT = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de Vendedors"); }
        }
        private void CargarListaAdministrativos()
        {
            try
            {
                BLL.AdministrativoBLL mapA = new BLL.AdministrativoBLL();
                ListaAdministrativos = mapA.BuscarTodos(false);
                mapA = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de administrativos"); }
        }
        private void CargarListaEncargados()
        {
            try
            {
                BLL.EncargadoBLL mapE = new BLL.EncargadoBLL();
                ListaEncargados = mapE.BuscarTodos(false);
                mapE = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de encargados"); }
        }
        private void CargarListaPermisos()
        {
            try
            {
                BLL.PermisoBLL mapP = new BLL.PermisoBLL();
                ListaPermisos = mapP.BuscarTodos();
                mapP = null;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar la lista de permisos"); }
        }
        private void CargarComboPermisos()
        {
            try
            {
                if (ListaPermisos != null)
                {
                    cmbPermiso.DataSource = null;
                    permisos.Clear();
                    foreach (ENTITY.Permiso ent in ListaPermisos)
                    {
                        DataRow dr = permisos.NewRow();
                        dr["Id"] = ent.Id;
                        dr["Descripcion"] = ent.Detalle;
                        permisos.Rows.Add(dr);
                    }
                    cmbPermiso.DataSource = permisos;
                    cmbPermiso.DisplayMember = "Descripcion";
                    cmbPermiso.ValueMember = "Id";
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar el combo de permisos"); }
        }
        private void CargarComboPersonas()
        {
            try
            {
                cmbPersona.DataSource = null;
                personas.Clear();
                if (ListaGerentes != null)
                {
                    foreach (ENTITY.Gerente ent in ListaGerentes)
                    {
                        ENTITY.Usuarios usuario = ListaUsuarios.Find(x => x.DNI == ent.DNI);
                        if (usuario == null)
                        {
                            DataRow dr = personas.NewRow();
                            dr["DNI"] = ent.DNI;
                            dr["Nombre"] = ent.Nombre + " " + ent.Apellido;
                            personas.Rows.Add(dr);
                        }
                    }
                }
                if (ListaJefes != null)
                {
                    foreach (ENTITY.Jefe ent in ListaJefes)
                    {
                        ENTITY.Usuarios usuario = ListaUsuarios.Find(x => x.DNI == ent.DNI);
                        if (usuario == null)
                        {
                            DataRow dr = personas.NewRow();
                            dr["DNI"] = ent.DNI;
                            dr["Nombre"] = ent.Nombre + " " + ent.Apellido;
                            personas.Rows.Add(dr);
                        }
                    }
                }
                if (ListaSupervisores != null)
                {
                    foreach (ENTITY.Supervisor ent in ListaSupervisores)
                    {
                        ENTITY.Usuarios usuario = ListaUsuarios.Find(x => x.DNI == ent.DNI);
                        if (usuario == null)
                        {
                            DataRow dr = personas.NewRow();
                            dr["DNI"] = ent.DNI;
                            dr["Nombre"] = ent.Nombre + " " + ent.Apellido;
                            personas.Rows.Add(dr);
                        }
                    }
                }
                if (ListaVendedores != null)
                {
                    foreach (ENTITY.Vendedor ent in ListaVendedores)
                    {
                        ENTITY.Usuarios usuario = ListaUsuarios.Find(x => x.DNI == ent.DNI);
                        if (usuario == null)
                        {
                            DataRow dr = personas.NewRow();
                            dr["DNI"] = ent.DNI;
                            dr["Nombre"] = ent.Nombre + " " + ent.Apellido;
                            personas.Rows.Add(dr);
                        }
                    }
                }
                if (ListaAdministrativos != null)
                {
                    foreach (ENTITY.Administrativo ent in ListaAdministrativos)
                    {
                        ENTITY.Usuarios usuario = ListaUsuarios.Find(x => x.DNI == ent.DNI);
                        if (usuario == null)
                        {
                            DataRow dr = personas.NewRow();
                            dr["DNI"] = ent.DNI;
                            dr["Nombre"] = ent.Nombre + " " + ent.Apellido;
                            personas.Rows.Add(dr);
                        }
                    }
                }
                if (ListaEncargados != null)
                {
                    foreach (ENTITY.Encargado ent in ListaEncargados)
                    {
                        ENTITY.Usuarios usuario = ListaUsuarios.Find(x => x.DNI == ent.Id);
                        if (usuario == null)
                        {
                            DataRow dr = personas.NewRow();
                            dr["DNI"] = ent.Id;
                            dr["Nombre"] = ent.Nombre + " " + ent.Apellido;
                            personas.Rows.Add(dr);
                        }
                    }
                }
                cmbPersona.DataSource = personas;
                cmbPersona.DisplayMember = "Nombre";
                cmbPersona.ValueMember = "DNI";
                cmbPersona.SelectedValue = "";
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al cargar el combo de personas"); }
        }
        private void ActualizarGrilla()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("Usuario");
                dt.Columns.Add("Clave");
                dt.Columns.Add("Dni / Id");
                dt.Columns.Add("Permiso");
                if (ListaUsuarios != null)
                {
                    foreach (ENTITY.Usuarios o in ListaUsuarios)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = o.Id.ToString();
                        dr[1] = o.Usuario.ToString();
                        dr[2] = o.Clave.ToString();
                        dr[3] = o.DNI.ToString();
                        dr[4] = o.Permiso.Detalle.ToString();
                        dt.Rows.Add(dr);
                    }
                }
                dgvUsuarios.DataSource = dt;
                dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al actualizar la grilla de usuarios"); }
        }
        private void ResetFormulario()
        {
            try
            {
                txtUsuario.Text = string.Empty;
                CargarComboPersonas();
                if (cmbPermiso.Items.Count > 0) { cmbPermiso.SelectedValue = ""; }
                if (cmbPersona.Items.Count == 0) { btnAgregar.Enabled = false; }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al restablecer el formulario"); }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count > 0)
                {
                    int idSeleccionado = int.Parse(dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString());
                    string permisoSeleccionado = dgvUsuarios.SelectedRows[0].Cells[4].Value.ToString().ToLower();

                    // Accedo al formulario padre (Menu) para obtener el usuario logueado
                    if (Principal == null)
                        Principal = (Menu)this.MdiParent;

                    // Validación 1: evitar autodestrucción
                    if (Principal.usuario != null && idSeleccionado == Principal.usuario.Id)
                    {
                        MessageBox.Show("No se puede eliminar el usuario que está actualmente logueado.");
                        return;
                    }

                    // Validación 2: evitar eliminar al administrador
                    if (permisoSeleccionado.Contains("administrador") || permisoSeleccionado == "0")
                    {
                        MessageBox.Show("No se puede eliminar al usuario administrador.");
                        return;
                    }

                    // Si pasó las validaciones, se procede a eliminar
                    BLL.UsuariosBLL o = new BLL.UsuariosBLL();
                    int resultado = o.Borrar(idSeleccionado);

                    if (resultado == 1)
                    {
                        MessageBox.Show("Usuario borrado exitosamente");
                        Principal.AgregarALaBitacora($"Se eliminó el usuario con ID {idSeleccionado}");
                    }
                    else
                    {
                        MessageBox.Show("Ocurrió un problema durante el borrado, intente nuevamente");
                    }

                    CargarListaUsuarios();
                    ActualizarGrilla();
                    ResetFormulario();
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un usuario para borrar.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un problema al borrar un usuario");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count > 0)
                {
                    if (txtUsuario.Text != string.Empty && cmbPermiso.Text != string.Empty)
                    {
                        BLL.UsuariosBLL o = new BLL.UsuariosBLL();
                        int resultado = o.ModificarUno(int.Parse(dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString()), txtUsuario.Text, int.Parse(cmbPermiso.SelectedValue.ToString()));
                        if (resultado == 1) 
                        { 
                            MessageBox.Show("Usuario modificado excitosamente"); 
                            Principal.AgregarALaBitacora($"Se modificó el usuario {dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString()}"); 
                        }
                        else { MessageBox.Show("Ocurrió un problema, intente nuevamente"); }
                        CargarListaUsuarios();
                        ActualizarGrilla();
                        ResetFormulario();
                    }
                    else { MessageBox.Show("El usuario debe tener un usuario y un permiso"); }
                }
                else { MessageBox.Show("Debe seleccionar un usuario primero"); }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al modificar un usuario"); }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsuario.Text != string.Empty)
                {
                    BLL.UsuariosBLL o = new BLL.UsuariosBLL();
                    int resultado = o.AgregarUno(txtUsuario.Text, "cambiar", int.Parse(cmbPersona.SelectedValue.ToString()), int.Parse(cmbPermiso.SelectedValue.ToString()));
                    if (resultado == 1) 
                    { 
                        MessageBox.Show("Usuario agregado excitosamente"); 
                        Principal.AgregarALaBitacora($"Se agregó el usuario con usuario {txtUsuario.Text}"); 
                    }
                    else { MessageBox.Show("Ocurrió un problema, intente nuevamente"); }

                    CargarListaUsuarios();
                    ActualizarGrilla();
                    ResetFormulario();
                }
                else { MessageBox.Show("El cuadro de texto Usuario no puede estar vacio"); }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al agregar un usuario"); }
        }

        private void btnVerClave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count > 0)
                {
                    string claveDesencriptada = svc.Encriptacion.Desencriptar(dgvUsuarios.SelectedRows[0].Cells[2].Value.ToString());
                    MessageBox.Show($"La clave del usuario seleccionado es {claveDesencriptada}");
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar revelar la clave"); }
        }

        private void btnResetClave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count > 0)
                {
                    BLL.UsuariosBLL o = new BLL.UsuariosBLL();
                    int resultado = o.ReestablecerClave(int.Parse(dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString()));
                    if (resultado == 1) 
                    { 
                        MessageBox.Show("Se reestableció la clave correctamente."); 
                        Principal.AgregarALaBitacora($"Se reestableció la clave del usuario {dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString()}"); 
                    }
                    else { MessageBox.Show("Ocurrió un problema, intente nuevamente"); }
                    CargarListaUsuarios();
                    ActualizarGrilla();
                    ResetFormulario();
                }
                else { MessageBox.Show("Debe seleccionar un usuario primero."); }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al restablecer la clave"); }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ListaUsuarios = null;
            ListaVendedores = null;
            ListaSupervisores = null;
            ListaAdministrativos = null;
            ListaGerentes = null;
            ListaJefes = null;
            ListaEncargados = null;
            ListaPermisos = null;
            ListaPersonas = null;
            this.Close();
        }

        public static implicit operator FormUsuarios(ENTITY.Usuarios v)
        {
            throw new NotImplementedException();
        }

        private void cmbPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPersona.SelectedValue != null)
            {
                string nombreCompleto = cmbPersona.Text;
                string[] partesNombre = nombreCompleto.Split(' ');
                if (partesNombre.Length >= 2)
                {
                    string nombre = partesNombre[0];
                    string apellido = partesNombre[partesNombre.Length - 1]; // Asegura tomar el último como apellido

                    string usuarioBase = (nombre.Substring(0, 1) + apellido).ToLower();
                    string usuarioSugerido = GenerarUsuarioUnico(usuarioBase);
                    txtUsuario.Text = usuarioSugerido;
                }
            }
            else
            {
                txtUsuario.Text = string.Empty; // Limpiar si no hay selección
            }
        }

        private string GenerarUsuarioUnico(string usuarioBase)
        {
            string usuarioPropuesto = usuarioBase;
            int contador = 0;
                        
            CargarListaUsuarios(); 
            // Verificar si el usuario ya existe
            while (ListaUsuarios.Any(u => u.Usuario.ToLower() == usuarioPropuesto))
            {
                contador++;
                usuarioPropuesto = $"{usuarioBase}{contador}";
            }
            return usuarioPropuesto;
        }
    }
}
