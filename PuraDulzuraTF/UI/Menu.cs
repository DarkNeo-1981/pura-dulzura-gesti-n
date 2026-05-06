using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI
{
    public partial class Menu : Form
    {
        Form f;
        Dictionary<string, OpcionesDeMenu> DiccionarioMenu = new Dictionary<string, OpcionesDeMenu>();
        internal ENTITY.Usuarios usuario;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParams = base.CreateParams;
                handleParams.ExStyle |= 0x02000000;
                return handleParams;
            }
        }

        public Menu()
        {
            this.InitializeComponent();
            ssUsuario.ForeColor = ColorTranslator.FromHtml("#F8E9DA");
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            tsMenu.Visible = true;
            f = new Login();
            f.MdiParent = this;
            f.FormClosed += ResetForm;
            f.Show();
        }

        public void ResetForm(object o, EventArgs e)
        {
            f = null;
            tsMenu.Enabled = true;
        }

        public void MostrarMenu()
        {            
            List<ENTITY.Permiso_Menu> ListaDeMenusHabilitados = null; // Inicializar a null
            try
            {
                BLL.OpcionesDeMenuBLL menu = new BLL.OpcionesDeMenuBLL();
                List<ENTITY.OpcionesDeMenu> listaMenu = new List<ENTITY.OpcionesDeMenu>();
                listaMenu = menu.TraerTodo();
                BLL.PermisoBLL permiso = new BLL.PermisoBLL();
                usuario.Permiso = permiso.BuscarUno(usuario.Permiso.Id);
                this.ssUsuario.Items.Add($"Usuario: {usuario.Usuario} - {usuario.Permiso.Detalle}");
                tsMenu.Visible = true;
                BLL.PermisoMenuBLL p_bll = new BLL.PermisoMenuBLL();
                ListaDeMenusHabilitados = p_bll.BuscarUno(usuario.Permiso.Id); 
                menu = null;
                permiso = null;
                p_bll = null;
                ConvertirListaEnDiccionario(listaMenu);
                RecorrerMenu(ListaDeMenusHabilitados);
                ListaDeMenusHabilitados = null;
                listaMenu = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al mostrar el menu: " + ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.ToString()); 
            }
        }

        private void ConvertirListaEnDiccionario(List<ENTITY.OpcionesDeMenu> pLista)
        {
            try
            {
                if (pLista != null)
                {
                    foreach (ENTITY.OpcionesDeMenu opciones in pLista)
                    {
                        DiccionarioMenu.Add(opciones.Detalle, opciones);
                        // ¡Aca se aplica el Composite!
                        if (opciones.GetType() == typeof(ENTITY.Compuesto))
                        {
                            // Si la opción es un "Compuesto", procesa a sus hijos recursivamente.
                            BuscarHijosEnListaDeMenu(((ENTITY.Compuesto)opciones).Hijos.ToList()); // .ToList() para que coincida con el tipo List<ENTITY.OpcionesDeMenu>
                        }
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al convertir la lista en diccionario"); }
        }

        private void RecorrerMenu(List<ENTITY.Permiso_Menu> lp)
        {
            try
            {
                // Llama a la función recursiva para los elementos principales
                foreach (ToolStripMenuItem item in this.tsMenu.Items)
                {
                    RecorrerMenuItemRecursivo(item, lp);
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al recorrer el menu"); }
        }

        private void RecorrerMenuItemRecursivo(ToolStripMenuItem item, List<ENTITY.Permiso_Menu> lp)
        {
            // 1. Habilitar o Deshabilitar el elemento actual
            if (HabilitarMenu(item, lp)) 
            {
                // 2. Llamar recursivamente a todos sus sub-elementos (hijos)
                foreach (ToolStripMenuItem subItem in item.DropDownItems)
                {                   
                    if (subItem != null)
                    {
                        RecorrerMenuItemRecursivo(subItem, lp);
                    }
                }
            }
            // Si un padre no está habilitado, no hay necesidad de revisar a sus hijos.
        }

        private bool HabilitarMenu(ToolStripMenuItem t, List<ENTITY.Permiso_Menu> lp)
        {
            try
            {                
                t.Visible = false;

                if (DiccionarioMenu.ContainsKey(t.Text))
                {                    
                    bool tienePermiso = lp.Any(p => p.MenuId == DiccionarioMenu[t.Text].Id);

                    if (tienePermiso)
                    {
                        t.Visible = true;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al habilitar un menu"); return false; }
        }

        private void BuscarHijosEnListaDeMenu(List<ENTITY.OpcionesDeMenu> pLista)
        {
            try
            {
                if (pLista != null)
                {
                    foreach (ENTITY.OpcionesDeMenu opciones in pLista)
                    {
                        DiccionarioMenu.Add(opciones.Detalle, opciones);
                        if (opciones.GetType() == typeof(ENTITY.Compuesto))
                        {
                            BuscarHijosEnListaDeMenu(((ENTITY.Compuesto)opciones).Hijos.ToList());
                        }
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al buscar sub menus"); }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarALaBitacora($"Salió del sistema el operador {usuario.Usuario}");
                DiccionarioMenu.Clear();
                usuario = null;
                tsMenu.Visible = false;
                ssUsuario.Items.Clear();
                if (f != null && !f.IsDisposed)
                {
                    f.Close();
                    f = null;
                }

                f = new UI.Login();
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al intentar desloguearse del sistema: " + ex.Message);
            }
        }

        public void AgregarALaBitacora(string pDetalle)
        {
            try
            {
                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                if (usuario != null)
                {
                    b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", pDetalle);
                }
                else
                {
                    b.AgregarUnoSinOperador(pDetalle);
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar escribir en la bitacora"); }
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new Permisos();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void bitácoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new Bitacora();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new Empleados();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un problema al intentar abrir el formulario");
            }
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new FormUsuarios();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un problema al intentar abrir el formulario");
            }
        }

        private void realizarBackUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\DB\\");
                DirectoryInfo[] dirs = dir.GetDirectories();

                if (!Directory.Exists(Environment.CurrentDirectory + "\\BackUp\\"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\BackUp\\");
                }
                string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string directorio = Path.Combine(Environment.CurrentDirectory + "\\BackUp\\", fecha);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(directorio, file.Name);
                    file.CopyTo(temppath, false);
                }

                BLL.BitacoraBLL b = new BLL.BitacoraBLL();
                b.AgregarUno($"{usuario.Id} - {usuario.Usuario}", $"Se realizó un BackUp de la base de datos ({fecha})");
                MessageBox.Show("Se realizó el BackUp correctamente.");
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al realizar el backup"); }
        }

        private void restaurarBackUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new RestaurarBackUp();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void nuevoProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new Productos();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void aBMClasificaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (f == null)
                {
                    f = new ClasificacionProductos();
                    f.MdiParent = this;
                    f.FormClosed += ResetForm;
                    f.Show();
                    tsMenu.Enabled = false;
                }
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void verProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new VerProductos();
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void abrirOrdenPedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new AbrirOrdenDePedido();
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void registrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new RegistrarCliente(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void consultarClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new VerClientes(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void buscarOrdenPedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new BuscarOrdenDePedido();
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void dashBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new Dashboard();
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void novedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new NovedadesLiquidacionSueldos(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void liquidaciónSueldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new LiquidacionSueldos(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void configuraciónMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new ConfiguracionMail();
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un problema al intentar abrir el formulario");
            }
        }

        private void registrarProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new GestionProveedores(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void órdenesDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new DetalleOrdenCompra(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }

        private void consultarPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                f = new GestionPagosOC(this.usuario);
                f.MdiParent = this;
                f.FormClosed += ResetForm;
                f.Show();
                tsMenu.Enabled = false;
            }
            catch (Exception) { MessageBox.Show("Ocurrió un problema al intentar abrir el formulario"); }
        }
    }    
}
