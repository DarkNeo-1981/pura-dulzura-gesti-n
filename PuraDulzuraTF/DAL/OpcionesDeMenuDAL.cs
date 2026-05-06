using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DAL
{
    public class OpcionesDeMenuDAL
    {
        // Rutas de los archivos XML
        private readonly string _menuXmlPath = AppDomain.CurrentDomain.BaseDirectory + @"DB\Menus.xml";
        private readonly string _padresHijosXmlPath = AppDomain.CurrentDomain.BaseDirectory + @"DB\Padres_Hijos.xml";
        private readonly string _permisosMenuXmlPath = AppDomain.CurrentDomain.BaseDirectory + @"DB\Permisos_Menu.xml";

        public OpcionesDeMenuDAL()
        {
            // 1. Crear el directorio DB si no existe
            string dbDirectory = Path.GetDirectoryName(_menuXmlPath);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            // 2. Inicializar Menus.xml (Lista de todos los ítems de menú)
            if (!File.Exists(_menuXmlPath))
            {
                var doc = new XDocument(
                    new XElement("Menus",
                        // --- Menús Principales (ID 1-10) ---
                        new XElement("Menu", new XAttribute("Id", 1), new XAttribute("Detalle", "Menu")),
                        new XElement("Menu", new XAttribute("Id", 2), new XAttribute("Detalle", "Administrar")),
                        new XElement("Menu", new XAttribute("Id", 3), new XAttribute("Detalle", "RRHH")),
                        new XElement("Menu", new XAttribute("Id", 4), new XAttribute("Detalle", "Proveedores")),
                        new XElement("Menu", new XAttribute("Id", 5), new XAttribute("Detalle", "Productos")),
                        new XElement("Menu", new XAttribute("Id", 6), new XAttribute("Detalle", "Clientes")),
                        new XElement("Menu", new XAttribute("Id", 7), new XAttribute("Detalle", "Pedidos")),
                        new XElement("Menu", new XAttribute("Id", 8), new XAttribute("Detalle", "Informes")),
                        new XElement("Menu", new XAttribute("Id", 9), new XAttribute("Detalle", "Bitácora")),
                        new XElement("Menu", new XAttribute("Id", 10), new XAttribute("Detalle", "BackUp")),

                        // --- Submenús Secuenciales (A partir de ID 11) ---
                        new XElement("Menu", new XAttribute("Id", 11), new XAttribute("Detalle", "Permisos")),
                        new XElement("Menu", new XAttribute("Id", 12), new XAttribute("Detalle", "Usuarios")),
                        new XElement("Menu", new XAttribute("Id", 13), new XAttribute("Detalle", "Empleados")),
                        new XElement("Menu", new XAttribute("Id", 14), new XAttribute("Detalle", "Novedades")),
                        new XElement("Menu", new XAttribute("Id", 15), new XAttribute("Detalle", "Liquidación Sueldos")),
                        new XElement("Menu", new XAttribute("Id", 16), new XAttribute("Detalle", "Configuración Mail")),

                        // --- Submenús de Proveedores (ID 17-19) ---
                        new XElement("Menu", new XAttribute("Id", 17), new XAttribute("Detalle", "Registrar Proveedor")),
                        new XElement("Menu", new XAttribute("Id", 18), new XAttribute("Detalle", "Órdenes de Compra")),
                        new XElement("Menu", new XAttribute("Id", 19), new XAttribute("Detalle", "Gestionar Pagos")),

                        new XElement("Menu", new XAttribute("Id", 20), new XAttribute("Detalle", "Nuevo Producto")),
                        new XElement("Menu", new XAttribute("Id", 21), new XAttribute("Detalle", "Ver Productos")),
                        new XElement("Menu", new XAttribute("Id", 22), new XAttribute("Detalle", "ABM Clasificación")),
                        new XElement("Menu", new XAttribute("Id", 23), new XAttribute("Detalle", "Registrar cliente")),
                        new XElement("Menu", new XAttribute("Id", 24), new XAttribute("Detalle", "Consultar clientes")),
                        new XElement("Menu", new XAttribute("Id", 25), new XAttribute("Detalle", "Registrar Pedidos")),
                        new XElement("Menu", new XAttribute("Id", 26), new XAttribute("Detalle", "Buscar Pedidos")),
                        new XElement("Menu", new XAttribute("Id", 27), new XAttribute("Detalle", "DashBoard")),
                        new XElement("Menu", new XAttribute("Id", 28), new XAttribute("Detalle", "Realizar BackUp")),
                        new XElement("Menu", new XAttribute("Id", 29), new XAttribute("Detalle", "Restaurar BackUp")),

                        new XElement("Menu", new XAttribute("Id", 30), new XAttribute("Detalle", "Desloguearse"))         // Ítem de Desloguearse al final
                    )
                );
                doc.Save(_menuXmlPath);
            }

            // 3. Inicializar Padres_Hijos.xml (Relaciones jerárquicas)
            if (!File.Exists(_padresHijosXmlPath))
            {
                var doc = new XDocument(
                    new XElement("Relaciones",
                        // *** Relación Menu (Id=1) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 1), new XAttribute("IdHijo", 30)), // Menu -> Desloguearse (Id 30)

                        // *** Administrar (Id 2) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 2), new XAttribute("IdHijo", 11)), // Administrar -> Permisos (Id 11)
                        new XElement("Relacion", new XAttribute("IdPadre", 2), new XAttribute("IdHijo", 12)), // Administrar -> Usuarios (Id 12)

                        // *** RRHH (Id 3) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 3), new XAttribute("IdHijo", 13)), // RRHH -> Empleados (Id 13)
                        new XElement("Relacion", new XAttribute("IdPadre", 3), new XAttribute("IdHijo", 14)), // RRHH -> Novedades (Id 14)
                        new XElement("Relacion", new XAttribute("IdPadre", 3), new XAttribute("IdHijo", 15)), // RRHH -> Liquidación Sueldos (Id 15)
                        new XElement("Relacion", new XAttribute("IdPadre", 3), new XAttribute("IdHijo", 16)), // RRHH -> Configuración Mail (Id 16)

                        // *** Proveedores (Id 4) - NUEVO ***
                        new XElement("Relacion", new XAttribute("IdPadre", 4), new XAttribute("IdHijo", 17)), // Proveedores -> Registrar Proveedor (Id 17)
                        new XElement("Relacion", new XAttribute("IdPadre", 4), new XAttribute("IdHijo", 18)), // Proveedores -> Órdenes de Compra (Id 18)
                        new XElement("Relacion", new XAttribute("IdPadre", 4), new XAttribute("IdHijo", 19)), // Proveedores -> Consultar Pagos (Id 19)

                        // *** Productos (Id 5) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 5), new XAttribute("IdHijo", 20)), // Productos -> Nuevo Producto (Id 20)
                        new XElement("Relacion", new XAttribute("IdPadre", 5), new XAttribute("IdHijo", 21)), // Productos -> Ver Productos (Id 21)
                        new XElement("Relacion", new XAttribute("IdPadre", 5), new XAttribute("IdHijo", 22)), // Productos -> ABM Clasificación (Id 22)

                        // *** Clientes (Id 6) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 6), new XAttribute("IdHijo", 23)), // Clientes -> Registrar cliente (Id 23)
                        new XElement("Relacion", new XAttribute("IdPadre", 6), new XAttribute("IdHijo", 24)), // Clientes -> Consultar clientes (Id 24)

                        // *** Pedidos (Id 7) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 7), new XAttribute("IdHijo", 25)), // Pedidos -> Registrar Pedidos (Id 25)
                        new XElement("Relacion", new XAttribute("IdPadre", 7), new XAttribute("IdHijo", 26)), // Pedidos -> Buscar Pedidos (Id 26)

                        // *** Informes (Id 8) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 8), new XAttribute("IdHijo", 27)), // Informes -> DashBoard (Id 27)

                        // Bitácora (Id 9) no tiene hijos, solo es una patente.

                        // *** BackUp (Id 10) ***
                        new XElement("Relacion", new XAttribute("IdPadre", 10), new XAttribute("IdHijo", 28)), // BackUp -> Realizar BackUp (Id 28)
                        new XElement("Relacion", new XAttribute("IdPadre", 10), new XAttribute("IdHijo", 29))  // BackUp -> Restaurar BackUp (Id 29)
                    )
                );
                doc.Save(_padresHijosXmlPath);
            }

            // 4. Inicializar Permisos_Menu.xml (Permisos por defecto)
            // Se mantiene el contenido original más las nuevas opciones de RRHH para el Admin (Id 1)
            if (!File.Exists(_permisosMenuXmlPath))
            {
                var doc = new XDocument(
                    new XElement("Permisos_Menu",
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 1)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 2)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 3)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 4)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 5)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 6)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 10)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 11)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 12)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 13)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 14)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 15)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 16)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 17)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 18)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 20)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 21)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 22)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 23)), // RRHH
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 24)), // Novedades
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 25)), // Liquidación
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 26)), // Mail
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 27)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 28)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 29)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 30)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 1)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 3)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 10)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 11)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 1)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 3)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 7)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 8)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 10)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 11)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 12)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 7)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 9)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 12)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 6)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 4), new XAttribute("MenuId", 6)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 2)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 3)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 6)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 12)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 22)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 1)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 6)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 7)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 9)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 10)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 12)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 13)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 14)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 20)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 22)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 8)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 2), new XAttribute("MenuId", 19)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 20)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 21)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 3), new XAttribute("MenuId", 22)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 9)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 1)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 20)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 5), new XAttribute("MenuId", 21)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 7)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 8)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 9)),
                        new XElement("Permiso_Menu", new XAttribute("PermisoId", 1), new XAttribute("MenuId", 19))
                    )
                );
                doc.Save(_permisosMenuXmlPath);
            }
        }

        private DataTable CargarXmlEnDataTable(string xmlPath)
        {
            DataTable dt = new DataTable();
            try
            {
                if (!System.IO.File.Exists(xmlPath))
                {
                    throw new System.IO.FileNotFoundException($"El archivo XML no se encuentra en: {xmlPath}");
                }
                XDocument doc = XDocument.Load(xmlPath);
                string elementName = doc.Root?.Elements().FirstOrDefault()?.Name.LocalName;
                if (string.IsNullOrEmpty(elementName))
                {
                    System.Diagnostics.Debug.WriteLine($"Advertencia: El archivo XML {xmlPath} no contiene elementos para procesar.");
                    return dt; // Retorna un DataTable vacío si no hay elementos
                }

                // 1. Crear columnas basadas en los atributos del primer elemento
                foreach (XAttribute attr in doc.Root.Elements(elementName).FirstOrDefault()?.Attributes() ?? Enumerable.Empty<XAttribute>())
                {
                    if (!dt.Columns.Contains(attr.Name.LocalName))
                    {
                        dt.Columns.Add(attr.Name.LocalName);
                    }
                }
                // 2. Crear columnas basadas en los elementos hijos del primer elemento (si existen)
                foreach (XElement childElement in doc.Root.Elements(elementName).FirstOrDefault()?.Elements() ?? Enumerable.Empty<XElement>())
                {
                    if (!dt.Columns.Contains(childElement.Name.LocalName))
                    {
                        dt.Columns.Add(childElement.Name.LocalName);
                    }
                }

                // 3. Llenar el DataTable con los datos
                foreach (XElement element in doc.Root.Elements(elementName))
                {
                    DataRow row = dt.NewRow();
                    // Cargar datos de atributos
                    foreach (XAttribute attr in element.Attributes())
                    {
                        if (dt.Columns.Contains(attr.Name.LocalName))
                        {
                            row[attr.Name.LocalName] = attr.Value;
                        }
                    }

                    // Cargar datos de elementos hijos
                    foreach (XElement childElement in element.Elements())
                    {
                        if (dt.Columns.Contains(childElement.Name.LocalName))
                        {
                            row[childElement.Name.LocalName] = childElement.Value;
                        }
                    }
                    dt.Rows.Add(row);
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCION (DAL): Archivo no encontrado - {xmlPath}: {ex.Message}");
                throw; // Relanza la excepción para que sea capturada por la BLL/UI
            }
            catch (System.Xml.XmlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCION (DAL): XML mal formado - {xmlPath}: {ex.Message}");
                throw; // Relanza la excepción
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCION (DAL): Error inesperado al cargar XML - {xmlPath}: {ex.Message}");
                throw; // Relanza la excepción
            }
            return dt;
        }

        private void GuardarDataTableEnXml(DataTable dt, string xmlPath, string rootName, string elementName)
        {
            try
            {
                XElement root = new XElement(rootName);
                foreach (DataRow row in dt.Rows)
                {
                    XElement element = new XElement(elementName);
                    foreach (DataColumn col in dt.Columns)
                    {
                        // Se guardan todos los campos como atributos
                        element.SetAttributeValue(col.ColumnName, row[col].ToString());
                    }
                    root.Add(element);
                }
                root.Save(xmlPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar DataTable en XML en {xmlPath}: {ex.Message}");
                throw new Exception($"Error al guardar datos en {xmlPath}.", ex);
            }
        }

        public DataTable Buscar()
        {
            return CargarXmlEnDataTable(_menuXmlPath);
        }

        public int BuscarPadre(int pHijo)
        {
            try
            {
                XDocument doc = XDocument.Load(_padresHijosXmlPath);
                var relacion = doc.Root.Elements("Relacion")
                             .FirstOrDefault(r => (int)r.Attribute("IdHijo") == pHijo);
                if (relacion != null)
                {
                    return (int)relacion.Attribute("IdPadre");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al buscar padre para hijo {pHijo}: {ex.Message}");
            }
            return 0; // Si no tiene padre o hay un error.
        }

        public List<int> BuscarHijos(int pPadre)
        {
            List<int> hijos = new List<int>();
            try
            {
                XDocument doc = XDocument.Load(_padresHijosXmlPath);
                hijos = doc.Root.Elements("Relacion")
                      .Where(r => (int)r.Attribute("IdPadre") == pPadre)
                      .Select(r => (int)r.Attribute("IdHijo"))
                      .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al buscar hijos para padre {pPadre}: {ex.Message}");
            }
            return hijos;
        }

        public int Agregar(string detalle)
        {
            try
            {
                XDocument doc;
                if (!System.IO.File.Exists(_menuXmlPath))
                {
                    doc = new XDocument(new XElement("Menus"));
                }
                else
                {
                    doc = XDocument.Load(_menuXmlPath);
                }

                // Calcular el siguiente ID disponible
                int nextId = 1;
                if (doc.Root.Elements("Menu").Any())
                {
                    nextId = doc.Root.Elements("Menu").Max(e => (int)e.Attribute("Id")) + 1;
                }
                XElement nuevoMenu = new XElement("Menu",
                  new XAttribute("Id", nextId),
                  new XAttribute("Detalle", detalle)
                );

                doc.Root.Add(nuevoMenu);
                doc.Save(_menuXmlPath);
                return nextId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en DAL.Agregar (Menú): {ex.Message}");
                throw new Exception($"Error en DAL al agregar el menú '{detalle}'.", ex);
            }
        }

        public void AgregarRelacion(int idPadre, int idHijo)
        {
            try
            {
                XDocument doc;
                if (!System.IO.File.Exists(_padresHijosXmlPath))
                {
                    doc = new XDocument(new XElement("Relaciones"));
                }
                else
                {
                    doc = XDocument.Load(_padresHijosXmlPath);
                }

                // Evitar relaciones duplicadas
                if (!doc.Root.Elements("Relacion")
          .Any(r => (int)r.Attribute("IdPadre") == idPadre && (int)r.Attribute("IdHijo") == idHijo))
                {
                    XElement nuevaRelacion = new XElement("Relacion",
                      new XAttribute("IdPadre", idPadre),
                      new XAttribute("IdHijo", idHijo)
                    );
                    doc.Root.Add(nuevaRelacion);
                    doc.Save(_padresHijosXmlPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en DAL.AgregarRelacion: {ex.Message}");
                throw new Exception($"Error en DAL al agregar la relación Padre={idPadre}, Hijo={idHijo}.", ex);
            }
        }

        public void Modificar(int id, string nuevoDetalle)
        {
            try
            {
                XDocument doc = XDocument.Load(_menuXmlPath);
                XElement menu = doc.Root.Elements("Menu")
                             .FirstOrDefault(m => (int)m.Attribute("Id") == id);
                if (menu != null)
                {
                    menu.SetAttributeValue("Detalle", nuevoDetalle);
                    doc.Save(_menuXmlPath);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Advertencia: Menú con Id {id} no encontrado para modificar.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en DAL.Modificar: {ex.Message}");
                throw new Exception($"Error en DAL al modificar el menú con Id {id}.", ex);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                // Eliminar el menú de Menus.xml
                XDocument menuDoc = XDocument.Load(_menuXmlPath);
                menuDoc.Root.Elements("Menu")
                  .Where(m => (int)m.Attribute("Id") == id)
                  .Remove();
                menuDoc.Save(_menuXmlPath);
                // Eliminar todas las relaciones en Padres_Hijos.xml donde este ID sea padre o hijo
                XDocument relacionesDoc = XDocument.Load(_padresHijosXmlPath);
                relacionesDoc.Root.Elements("Relacion")
                  .Where(r => (int)r.Attribute("IdPadre") == id || (int)r.Attribute("IdHijo") == id)
                  .Remove();
                relacionesDoc.Save(_padresHijosXmlPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en DAL.Eliminar: {ex.Message}");
                throw new Exception($"Error en DAL al eliminar el menú con Id {id}.", ex);
            }
        }

        public void EliminarRelacion(int idPadre, int idHijo)
        {
            try
            {
                XDocument doc = XDocument.Load(_padresHijosXmlPath);
                doc.Root.Elements("Relacion")
                  .Where(r => (int)r.Attribute("IdPadre") == idPadre && (int)r.Attribute("IdHijo") == idHijo)
                  .Remove();
                doc.Save(_padresHijosXmlPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en DAL.EliminarRelacion: {ex.Message}");
                throw new Exception($"Error en DAL al eliminar la relación Padre={idPadre}, Hijo={idHijo}.", ex);
            }
        }
    }
}