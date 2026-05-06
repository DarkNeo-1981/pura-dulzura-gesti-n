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
    public class PermisosDAL
    {
        private readonly string ruta;
        public PermisosDAL()
        {
            try
            {
                //Ruta de acceso
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "Permisos.xml";
                ruta = Path.Combine(directorio, archivo);
                //Verifico si existe la carpeta, si no existe, la creo
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                //Verifico si existe el archivo, si no existe, lo creo
                if (!File.Exists(ruta))
                {
                    XmlTextWriter archivoxml = new XmlTextWriter(ruta, System.Text.Encoding.UTF8);
                    archivoxml.Formatting = Formatting.Indented;
                    //identation es para que se escriba nueva lineas
                    archivoxml.Indentation = 2;
                    archivoxml.WriteStartDocument(true);
                    archivoxml.WriteStartElement("Permisos");
                    archivoxml.WriteStartElement("Permiso");
                    archivoxml.WriteAttributeString("Id", "1");
                    archivoxml.WriteElementString("Descripcion", "Administrador");
                    archivoxml.WriteEndElement();
                    archivoxml.WriteStartElement("Permiso");
                    archivoxml.WriteAttributeString("Id", "2");
                    archivoxml.WriteElementString("Descripcion", "Gerente");
                    archivoxml.WriteEndElement();
                    archivoxml.WriteStartElement("Permiso");
                    archivoxml.WriteAttributeString("Id", "3");
                    archivoxml.WriteElementString("Descripcion", "Supervisor");
                    archivoxml.WriteEndElement();
                    archivoxml.WriteStartElement("Permiso");
                    archivoxml.WriteAttributeString("Id", "4");
                    archivoxml.WriteElementString("Descripcion", "Vendedor");
                    archivoxml.WriteEndElement();
                    archivoxml.WriteStartElement("Permiso");
                    archivoxml.WriteAttributeString("Id", "5");
                    archivoxml.WriteElementString("Descripcion", "Encargado");
                    archivoxml.WriteEndElement();
                    archivoxml.WriteEndElement();
                    archivoxml.WriteEndDocument();
                    archivoxml.Close();
                }
            }
            catch (Exception) { throw new Exception("Error al crear el archivo Permisos.xml"); }
        }
        public DataTable Buscar_Uno(int pId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Descripcion");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Permiso in xmlDoc.Descendants("Permiso")
                               where Permiso.Attribute("Id").Value == pId.ToString()
                               select Permiso;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = pId;
                    dr["Descripcion"] = xElement.Element("Descripcion").Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }
        public DataTable Buscar_Todos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Descripcion");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Permiso in xmlDoc.Descendants("Permiso")
                               select Permiso;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = xElement.Attribute("Id").Value.ToString();
                    dr["Descripcion"] = xElement.Element("Descripcion").Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }
        public int Agregar(string pDescripcion)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                int maxId = xmlDoc.Descendants("Permiso").Max(x => int.Parse(x.Attribute("Id").Value.ToString()));
                xmlDoc.Element("Permisos").Add(new XElement("Permiso",
                                                    new XAttribute("Id", maxId + 1),
                                                    new XElement("Descripcion", pDescripcion)));
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }
        public int Borrar(int pId)
        {
            try
            {
                UsuariosDAL o = new UsuariosDAL();
                if (o.PermisoEnUso(pId))
                {
                    return 0;
                }
                else
                {
                    Permiso_MenuDAL pm = new Permiso_MenuDAL();
                    if (pm.PermisoEnUso(pId))
                    {
                        DataTable dt = pm.Buscar_X_Permiso(pId);
                        if (dt != null)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                pm.Borrar(int.Parse(dr[0].ToString()), int.Parse(dr[1].ToString()));
                            }
                        }
                    }
                    XDocument xmlDoc = XDocument.Load(ruta);
                    var consulta = from Permiso in xmlDoc.Descendants("Permiso")
                                   where Permiso.Attribute("Id").Value == pId.ToString()
                                   select Permiso;
                    consulta.Remove();
                    xmlDoc.Save(ruta);
                    return 1;
                }
            }
            catch (Exception) { return 0; }
        }
    }
}
