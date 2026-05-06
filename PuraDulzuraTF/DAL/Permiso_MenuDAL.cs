using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace DAL
{
    public class Permiso_MenuDAL
    {        
        private readonly string ruta;

        public Permiso_MenuDAL()
        {
            try
            {
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "Permisos_Menu.xml";
                ruta = Path.Combine(directorio,archivo);

                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                if (!File.Exists(ruta))
                {
                    XDocument doc = new XDocument(new XElement("Permisos_Menu"));
                    doc.Save(ruta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al inicializar Permiso_MenuDAL y crear el archivo Permisos_Menu.xml: " + ex.Message, ex);
            }
        }

        public DataTable Buscar_X_Permiso(int pPermisoId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PermisoId", typeof(int));
            dt.Columns.Add("MenuId", typeof(int));

            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                var consulta = from Permiso_Menu in xmlDoc.Descendants("Permiso_Menu")
                               where (int)Permiso_Menu.Attribute("PermisoId") == pPermisoId
                               select Permiso_Menu;

                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["PermisoId"] = pPermisoId;
                    dr["MenuId"] = (int)xElement.Attribute("MenuId");
                    dt.Rows.Add(dr);
                }
            }
            catch (FileNotFoundException)
            {
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar permisos por ID en DAL: {ex.Message}");
                return null;
            }
            return dt;
        }

        public int Agregar(int pPId, int pMId)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                bool existe = xmlDoc.Descendants("Permiso_Menu")
                                     .Any(pm => (int)pm.Attribute("PermisoId") == pPId &&
                                                (int)pm.Attribute("MenuId") == pMId);

                if (existe)
                {
                    return 2;
                }

                xmlDoc.Element("Permisos_Menu").Add(new XElement("Permiso_Menu",
                                                     new XAttribute("PermisoId", pPId),
                                                     new XAttribute("MenuId", pMId)));
                xmlDoc.Save(ruta);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar Permiso_Menu en DAL: {ex.Message}");
                return 0;
            }
        }

        public int Borrar(int pPId, int pMId)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                var elementToRemove = xmlDoc.Descendants("Permiso_Menu")
                                            .FirstOrDefault(pm => (int)pm.Attribute("PermisoId") == pPId &&
                                                                  (int)pm.Attribute("MenuId") == pMId);

                if (elementToRemove != null)
                {
                    elementToRemove.Remove();
                    xmlDoc.Save(ruta);
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al borrar Permiso_Menu en DAL: {ex.Message}");
                return 0;
            }
        }

        public bool PermisoEnUso(int pIdPermiso)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var permisoEnUso = xmlDoc.Descendants("Permiso_Menu").Any(x => (int)x.Attribute("PermisoId") == pIdPermiso);
                return permisoEnUso;
            }
            catch (Exception ex)
            {
                throw new Exception($"Se produjo un error al intentar verificar si el permiso se encuentra en uso: {ex.Message}", ex);
            }
        }
        
        public Dictionary<int, List<int>> ObtenerPadresHijos()
        {
            try
            {
                string rutaPadresHijos = Environment.CurrentDirectory + "\\DB\\Padres_Hijos.xml";
                                
                if (!File.Exists(rutaPadresHijos))
                {                    
                    throw new FileNotFoundException($"El archivo Padres_Hijos.xml no se encuentra en la ruta: {rutaPadresHijos}");
                }

                XDocument xmlDocPadresHijos = XDocument.Load(rutaPadresHijos);

                var relaciones = xmlDocPadresHijos.Descendants("Padre_Hijo")
                                    .GroupBy(ph => (int)ph.Attribute("Padre"))
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.Select(ph => (int)ph.Attribute("Hijo")).ToList()
                                    );
                return relaciones;
            }
            catch (Exception ex)
            {                
                throw new Exception("Error al obtener la estructura de padres e hijos desde el archivo XML.", ex);
            }
        }
    }
}