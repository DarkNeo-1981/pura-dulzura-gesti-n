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
    public partial class Permisos : Form
    {
        List<ENTITY.OpcionesDeMenu> ListaPermisos;
        List<ENTITY.OpcionesDeMenu> ListaMenus;
        List<ENTITY.OpcionesDeMenu> ListaMenusSinComposite;

        List<ENTITY.OpcionesDeMenu> ListaPermisosHabilitados;

        UI.Menu Principal; // Asegúrate de que tu clase Menu sea pública y accesible

        public Permisos()
        {
            InitializeComponent();
        }

        private void Permisos_Load(object sender, EventArgs e)
        {
            // Asegúrate de que el formulario principal sea realmente de tipo UI.Menu
            // if (this.MdiParent is UI.Menu)
            // {
            //    Principal = (UI.Menu)this.MdiParent;
            // }
            // else
            // {
            //    MessageBox.Show("El formulario padre no es del tipo UI.Menu.");
            //    // Puedes decidir qué hacer si el padre no es el esperado
            // }
            ResetFormulario();
        }

        private void ResetFormulario()
        {
            try
            {
                CargarListaPermisos();
                CargarListaMenus();
                CargarListaMenusSinComposite();

                CargarTreeViewRoles(tvPermisos, 0, "Permisos", ListaPermisos);
                CargarTreeViewMenus(tvMenus, 0, "Menus", ListaMenus);

                tvPermisosHabilitados.Nodes.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al restablecer el formulario: " + ex.Message);
            }
        }

        private void CargarListaPermisos()
        {
            try
            {
                BLL.PermisoBLL mapP = new BLL.PermisoBLL();
                ListaPermisos = mapP.BuscarTodos().Cast<ENTITY.OpcionesDeMenu>().ToList();
                mapP = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar la lista de permisos: " + ex.Message);
            }
        }

        private void CargarListaMenus()
        {
            try
            {
                BLL.OpcionesDeMenuBLL mapM = new BLL.OpcionesDeMenuBLL();
                ListaMenus = mapM.TraerTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar la lista de menus: " + ex.Message);
            }
        }

        private void CargarListaMenusSinComposite()
        {
            try
            {
                BLL.OpcionesDeMenuBLL mapM = new BLL.OpcionesDeMenuBLL();
                ListaMenusSinComposite = mapM.TraerTodoSinComposite();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar la lista de menus sin composite: " + ex.Message);
            }
        }

        private void CargarListaPermisosHabilitados(int permisoId)
        {
            try
            {
                BLL.PermisoMenuBLL mapP = new BLL.PermisoMenuBLL();
                List<ENTITY.Permiso_Menu> lista_per_menu = mapP.BuscarUno(permisoId);
                ListaPermisosHabilitados = new List<ENTITY.OpcionesDeMenu>();

                if (lista_per_menu != null)
                {
                    foreach (ENTITY.Permiso_Menu ent in lista_per_menu)
                    {
                        ENTITY.OpcionesDeMenu menuHabilitado = ListaMenusSinComposite.Find(x => x.Id == ent.MenuId);
                        if (menuHabilitado != null && !ListaPermisosHabilitados.Any(x => x.Id == menuHabilitado.Id))
                        {
                            ListaPermisosHabilitados.Add(menuHabilitado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar la lista de permisos habilitados: " + ex.Message);
            }
        }

        private void CargarTreeViewRoles(TreeView tv, int pId, string pDetalle, List<ENTITY.OpcionesDeMenu> l)
        {
            try
            {
                tv.Nodes.Clear();
                TreeNode nodoRaiz = new TreeNode();
                nodoRaiz.Text = pDetalle;
                nodoRaiz.Name = pId.ToString();

                if (l != null)
                {
                    foreach (ENTITY.OpcionesDeMenu opcion in l)
                    {
                        TreeNode rolNode = new TreeNode(opcion.Detalle);
                        rolNode.Name = opcion.Id.ToString();
                        nodoRaiz.Nodes.Add(rolNode);
                    }
                }
                tv.Nodes.Add(nodoRaiz);
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar el treeview de roles: " + ex.Message);
            }
        }

        private void CargarTreeViewMenus(TreeView tv, int pId, string pDetalle, List<ENTITY.OpcionesDeMenu> l)
        {
            try
            {
                tv.Nodes.Clear();
                TreeNode nodoRaiz = new TreeNode(pDetalle);
                nodoRaiz.Name = pId.ToString();

                if (l != null)
                {
                    foreach (ENTITY.OpcionesDeMenu menu in l)
                    {
                        TreeNode menuNode = CargarSubNodoJerarquico(menu, null);
                        if (menuNode != null)
                        {
                            nodoRaiz.Nodes.Add(menuNode);
                        }
                    }
                }

                tv.Nodes.Add(nodoRaiz);
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar el treeview de menus: " + ex.Message);
            }
        }

        private void CargarTreeViewPermisosHabilitados(TreeView tv, int pId, string pDetalle, List<ENTITY.OpcionesDeMenu> l, List<ENTITY.OpcionesDeMenu> permisosHabilitados)
        {
            try
            {
                tv.Nodes.Clear();
                TreeNode nodoRaiz = new TreeNode(pDetalle);
                nodoRaiz.Name = pId.ToString();

                if (l != null && permisosHabilitados != null)
                {
                    foreach (ENTITY.OpcionesDeMenu menu in l)
                    {
                        TreeNode menuNode = CargarSubNodoJerarquico(menu, permisosHabilitados);
                        if (menuNode != null)
                        {
                            nodoRaiz.Nodes.Add(menuNode);
                        }
                    }
                }
                tv.Nodes.Add(nodoRaiz);
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al cargar el treeview de permisos habilitados: " + ex.Message);
            }
        }

        private TreeNode CargarSubNodoJerarquico(ENTITY.OpcionesDeMenu menu, List<ENTITY.OpcionesDeMenu> permisosHabilitados)
        {
            if (menu == null) return null;

            TreeNode nodo = new TreeNode(menu.Detalle);
            nodo.Name = menu.Id.ToString();

            bool isEnabled = (permisosHabilitados == null) || permisosHabilitados.Any(ph => ph.Id == menu.Id);
            bool hasEnabledChildren = false; // Solo relevante para Compuestos

            if (menu is ENTITY.Compuesto compuestoActual)
            {
                foreach (ENTITY.OpcionesDeMenu hijo in compuestoActual.Hijos)
                {
                    TreeNode childNode = CargarSubNodoJerarquico(hijo, permisosHabilitados);
                    if (childNode != null)
                    {
                        nodo.Nodes.Add(childNode);
                        hasEnabledChildren = true;
                    }
                }

                // Un nodo compuesto se muestra si está habilitado O si tiene hijos habilitados
                if (permisosHabilitados != null && !isEnabled && !hasEnabledChildren)
                {
                    return null;
                }
            }
            else // Es una Patente (hoja)
            {
                // Si es una patente y estamos filtrando, y no está habilitada, no la mostramos.
                if (permisosHabilitados != null && !isEnabled)
                {
                    return null;
                }
            }
            return nodo;
        }


        private void tvPermisos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (tvPermisos.SelectedNode != null && tvPermisos.SelectedNode.Name != "0")
                {
                    ENTITY.OpcionesDeMenu permisoSeleccionado = ListaPermisos.Find(x => x.Id == int.Parse(tvPermisos.SelectedNode.Name));
                    if (permisoSeleccionado != null)
                    {
                        CargarListaPermisosHabilitados(permisoSeleccionado.Id);
                    }
                    CargarTreeViewPermisosHabilitados(tvPermisosHabilitados, 0, "Menus", ListaMenus, ListaPermisosHabilitados);
                }
                else
                {
                    tvPermisosHabilitados.Nodes.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema actualizando el treeview: " + ex.Message);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvMenus.SelectedNode != null && tvPermisos.SelectedNode != null)
                {
                    if (tvMenus.SelectedNode.Name != "0" && tvPermisos.SelectedNode.Name != "0")
                    {
                        ENTITY.OpcionesDeMenu permisoSeleccionado = ListaPermisos.Find(x => x.Id == int.Parse(tvPermisos.SelectedNode.Name));
                        ENTITY.OpcionesDeMenu menuSeleccionadoParaAsignar = ListaMenusSinComposite.Find(x => x.Id == int.Parse(tvMenus.SelectedNode.Name));

                        BLL.PermisoMenuBLL pm = new BLL.PermisoMenuBLL();

                        if (permisoSeleccionado != null && menuSeleccionadoParaAsignar != null)
                        {
                            bool seAgregoAlgoNuevo = false; // Bandera para saber si algo se insertó realmente

                            // Recargar ListaPermisosHabilitados para tener el estado más actual antes de filtrar
                            CargarListaPermisosHabilitados(permisoSeleccionado.Id);

                            // Buscar los padres necesarios (todos, luego filtraremos los que ya existen)
                            List<ENTITY.OpcionesDeMenu> todosLosPadresNecesarios = new List<ENTITY.OpcionesDeMenu>();
                            BuscarPadre(menuSeleccionadoParaAsignar.Id, todosLosPadresNecesarios);

                            // Agrega los padres necesarios que AÚN NO ESTÁN asignados
                            foreach (ENTITY.OpcionesDeMenu padreNecesario in todosLosPadresNecesarios)
                            {
                                // Solo intentar agregar si no está ya en la lista de permisos habilitados
                                if (!ListaPermisosHabilitados.Any(p => p.Id == padreNecesario.Id))
                                {
                                    int resPadre = pm.AgregarUno(permisoSeleccionado.Id, padreNecesario.Id);
                                    if (resPadre == 1) // Éxito en la adición del padre
                                    {
                                        seAgregoAlgoNuevo = true;
                                    }
                                    else if (resPadre == 0) // Fallo al agregar un padre
                                    {
                                        MessageBox.Show($"Ocurrió un problema al agregar el padre '{padreNecesario.Detalle}'. Contacte al administrador. La operación ha sido cancelada.");
                                        return; // Salir si hay un fallo en los padres
                                    }
                                    // Si resPadre es 2 (ya existe), no se marca como nueva adición y se continúa
                                }
                            }

                            // Asignar el permiso seleccionado y sus descendientes (si es un compuesto)
                            int resultadoAsignacionPrincipal = pm.AsignarPermisoYDescendientes(permisoSeleccionado.Id, menuSeleccionadoParaAsignar.Id);

                            if (resultadoAsignacionPrincipal == 1) // Se agregó correctamente el permiso principal o sus descendientes
                            {
                                MessageBox.Show("Permiso asignado correctamente.");
                                seAgregoAlgoNuevo = true; // Confirma que hubo adiciones
                            }
                            else if (resultadoAsignacionPrincipal == 2) // La BLL indicó que todo ya existía (principal y sus descendientes)
                            {
                                // Este mensaje solo se muestra si NO se agregó nada nuevo (ni el principal, ni padres ni hijos)
                                if (!seAgregoAlgoNuevo) // Si la bandera de "seAgregoAlgoNuevo" es false
                                {
                                    MessageBox.Show("El permiso o uno de sus componentes necesarios ya se encuentra asignado.");
                                }
                                else // Si se agregaron padres pero el principal y sus hijos ya existían
                                {
                                    MessageBox.Show("Permiso asignado correctamente (algunos componentes ya existían).");
                                }
                            }
                            else // resultadoAsignacionPrincipal == 0 (u otro código de error)
                            {
                                MessageBox.Show("Ocurrió un problema al agregar el permiso principal. Contacte al administrador.");
                            }

                            // Solo recargar y refrescar la UI si realmente hubo algún cambio
                            if (seAgregoAlgoNuevo || resultadoAsignacionPrincipal == 1) // Se recarga si algo se insertó
                            {
                                CargarListaPermisosHabilitados(permisoSeleccionado.Id);
                                CargarTreeViewPermisosHabilitados(tvPermisosHabilitados, 0, "Menus", ListaMenus, ListaPermisosHabilitados);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar la definición del menú/patente seleccionado. Asegúrese de que la lista 'sin composite' esté completa.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un Rol y un Menú/Patente válido (no el nodo raíz).");
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un Rol y un Menú/Patente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al agregar permisos: " + ex.Message);
            }
        }

        private void btnAgregarRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRol.Text != string.Empty)
                {
                    BLL.PermisoBLL permiso = new BLL.PermisoBLL();
                    int resultado = permiso.Agregar(txtRol.Text);
                    if (resultado == 1)
                    {
                        MessageBox.Show("Se agregó el rol correctamente.");
                    }
                    else { MessageBox.Show("Ocurrió un problema, intente nuevamente."); }
                }
                else { MessageBox.Show("Debe ingresar primero la descripción del nuevo Rol."); }
                ResetFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al agregar un nuevo rol: " + ex.Message);
            }
        }

        private void BuscarPadre(int idHijo, List<ENTITY.OpcionesDeMenu> listaNecesarios)
        {
            try
            {
                BLL.OpcionesDeMenuBLL opm = new BLL.OpcionesDeMenuBLL();
                int padreId = opm.BuscarPadre(idHijo);

                if (padreId != 0)
                {
                    ENTITY.OpcionesDeMenu padreObjeto = ListaMenusSinComposite.Find(x => x.Id == padreId);
                    if (padreObjeto != null && !listaNecesarios.Any(x => x.Id == padreObjeto.Id))
                    {
                        listaNecesarios.Add(padreObjeto);
                    }
                    BuscarPadre(padreId, listaNecesarios);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al recorrer las opciones del menu (BuscarPadre): " + ex.Message);
            }
        }

        private void btnRemoverRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvPermisos.SelectedNode != null && tvPermisos.SelectedNode.Name != "0")
                {
                    ENTITY.OpcionesDeMenu permisoSeleccionado = ListaPermisos.Find(x => x.Id == int.Parse(tvPermisos.SelectedNode.Name));
                    BLL.PermisoBLL permisoBLL = new BLL.PermisoBLL();
                    int resultado = permisoBLL.Borrar(permisoSeleccionado.Id);
                    if (resultado == 1)
                    {
                        MessageBox.Show("Rol eliminado correctamente.");
                    }
                    else { MessageBox.Show("Ocurrió un problema, verifique que no haya un operador con el rol asignado e intente nuevamente."); }
                }
                ResetFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al eliminar el rol: " + ex.Message);
            }
        }

        private void btnRemoverPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvPermisosHabilitados.SelectedNode != null && tvPermisos.SelectedNode != null)
                {
                    if (tvPermisosHabilitados.SelectedNode.Name != "0" && tvPermisos.SelectedNode.Name != "0")
                    {
                        ENTITY.OpcionesDeMenu permisoSeleccionado = ListaPermisos.Find(x => x.Id == int.Parse(tvPermisos.SelectedNode.Name));
                        ENTITY.OpcionesDeMenu menuARemover = ListaMenusSinComposite.Find(x => x.Id == int.Parse(tvPermisosHabilitados.SelectedNode.Name));

                        BLL.PermisoMenuBLL pm = new BLL.PermisoMenuBLL();

                        if (permisoSeleccionado != null && menuARemover != null)
                        {
                            int resultadoBorrado = pm.BorrarPermisoYDescendientes(permisoSeleccionado.Id, menuARemover.Id);

                            if (resultadoBorrado == 1) // Se borró algo
                            {
                                MessageBox.Show("Permiso(s) eliminado(s) correctamente.");
                            }
                            else if (resultadoBorrado == 2) // No se encontró nada que borrar
                            {
                                MessageBox.Show("El permiso o uno de sus componentes no se encuentra asignado.");
                            }
                            else // resultadoBorrado == 0 (u otro error)
                            {
                                MessageBox.Show("Ocurrió un problema al remover el permiso. Contacte al administrador.");
                            }

                            // Siempre recargamos después de intentar borrar, ya que el estado puede haber cambiado
                            // (se borró una rama o solo un elemento, etc.)
                            CargarListaPermisosHabilitados(permisoSeleccionado.Id);
                            CargarTreeViewPermisosHabilitados(tvPermisosHabilitados, 0, "Menus", ListaMenus, ListaPermisosHabilitados);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar la definición del menú/patente seleccionado para remover.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un Rol y un Menú/Patente válido (no el nodo raíz).");
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un Rol y un Menú/Patente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema al remover permisos: " + ex.Message);
            }
        }
    }
}