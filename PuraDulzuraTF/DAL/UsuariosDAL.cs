using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace DAL
{
    public class UsuariosDAL
    {
        private readonly string ruta;
        
        public UsuariosDAL()
        {
            try
            {
                //Ruta de acceso a la base de datos
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "Usuarios.xml";
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
                    archivoxml.WriteStartElement("Usuarios");
                    archivoxml.WriteStartElement("Usuario");
                    archivoxml.WriteAttributeString("Id", "0");
                    archivoxml.WriteElementString("Nombre", "admin");
                    archivoxml.WriteElementString("Clave", "gC2Q+pQdgHBl/HXXUzmQIg==");
                    archivoxml.WriteElementString("DNI", "0");
                    archivoxml.WriteElementString("Permiso", "1");
                    archivoxml.WriteEndElement();
                    archivoxml.WriteEndElement();
                    archivoxml.WriteEndDocument();
                    archivoxml.Close();
                }
            }
            catch (Exception) { throw new Exception("Error al crear el archivo Usuarios.xml"); }
        }
        public int Agregar(string pUsuario, string pClave, int pDNI, int pPermiso)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                string claveEncriptada = SERVICIOS.Encriptacion.Encriptar(pClave);
                int maxId = xmlDoc.Descendants("Usuario").Max(x => int.Parse(x.Attribute("Id").Value.ToString()));
                xmlDoc.Element("Usuarios").Add(new XElement("Usuario",
                                                    new XAttribute("Id", maxId + 1),
                                                    new XElement("Nombre", pUsuario),
                                                    new XElement("Clave", claveEncriptada),
                                                    new XElement("DNI", pDNI),
                                                    new XElement("Permiso", pPermiso)));
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }
        public int CambiarClave(int pId, string pClave)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                string claveEncriptada = SERVICIOS.Encriptacion.Encriptar(pClave);
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               where Usuario.Attribute("Id").Value == pId.ToString()
                               select Usuario;
                foreach (XElement EModificar in consulta)
                {
                    EModificar.Element("Clave").Value = claveEncriptada;
                }
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;

        }
        public int Modificar(int pId, string pUsuario, int pPermiso)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               where Usuario.Attribute("Id").Value == pId.ToString()
                               select Usuario;
                foreach (XElement EModificar in consulta)
                {
                    EModificar.Element("Nombre").Value = pUsuario;
                    EModificar.Element("Permiso").Value = pPermiso.ToString();
                }
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }
        public int Borrar(int pId)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               where Usuario.Attribute("Id").Value == pId.ToString()
                               select Usuario;
                consulta.Remove();
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }
        public DataTable Buscar_Uno(int pId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Clave");
            dt.Columns.Add("DNI");
            dt.Columns.Add("Permiso");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               where Usuario.Attribute("Id").Value == pId.ToString()
                               select Usuario;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = pId;
                    dr["Usuario"] = xElement.Element("Nombre").Value.ToString();
                    dr["Clave"] = xElement.Element("Clave").Value.ToString();
                    dr["DNI"] = xElement.Element("DNI").Value.ToString();
                    dr["Permiso"] = xElement.Element("Permiso").Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }
        public DataTable Buscar_Uno(string pUsuario)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Clave");
            dt.Columns.Add("DNI");
            dt.Columns.Add("Permiso");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               where Usuario.Element("Nombre").Value == pUsuario.ToString()
                               select Usuario;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = xElement.Attribute("Id").Value.ToString();
                    dr["Usuario"] = pUsuario;
                    dr["Clave"] = xElement.Element("Clave").Value.ToString();
                    dr["DNI"] = xElement.Element("DNI").Value.ToString();
                    dr["Permiso"] = xElement.Element("Permiso").Value.ToString();
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
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Clave");
            dt.Columns.Add("DNI");
            dt.Columns.Add("Permiso");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               select Usuario;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = xElement.Attribute("Id").Value.ToString();
                    dr["Usuario"] = xElement.Element("Nombre").Value.ToString();
                    dr["Clave"] = xElement.Element("Clave").Value.ToString();
                    dr["DNI"] = xElement.Element("DNI").Value.ToString();
                    dr["Permiso"] = xElement.Element("Permiso").Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }
        public int ReestablecerClave(int pId)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                string claveEncriptada = SERVICIOS.Encriptacion.Encriptar("cambiar");
                var consulta = from Usuario in xmlDoc.Descendants("Usuario")
                               where Usuario.Attribute("Id").Value == pId.ToString()
                               select Usuario;
                foreach (XElement EModificar in consulta)
                {
                    EModificar.Element("Clave").Value = claveEncriptada;
                }
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }
        public bool PermisoEnUso(int pIdPermiso)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var permisoEnUso = xmlDoc.Descendants("Usuario").Any(x => (string)x.Element("Permiso") == pIdPermiso.ToString());
                if (permisoEnUso) { return true; }
                else { return false; }
            }
            catch (Exception) { throw new Exception("Se produjo un error al intentar verificar si el permiso se encuentra en uso"); }
        }
        public bool DniEnUso(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var DNIEnUso = xmlDoc.Descendants("Usuario").Any(x => (string)x.Element("DNI") == pDNI.ToString());
                if (DNIEnUso)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception) { throw new Exception("Se produjo un error al intentar verificar si el DNI/Id se encuentra en uso"); }
        }
    }
}
