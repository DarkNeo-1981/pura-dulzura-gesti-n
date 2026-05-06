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
    public class ClienteDAL
    {        
        private string ruta;

        public ClienteDAL()
        {
            try
            {                
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "Clientes.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                if (!File.Exists(ruta))
                {                    
                    var nuevoDoc = new XDocument(new XElement("Clientes"));
                    nuevoDoc.Save(ruta);
                }
            }
            catch (Exception ex)
            {               
                throw new Exception("Error al crear el archivo Clientes.xml", ex);
            }
        }

        public int Agregar(string nombre, string apellido, string dni, string telefono, string email, string calle, string numero, string piso, string depto, string localidad)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                int maxId = xmlDoc.Descendants("Cliente").Any() ? xmlDoc.Descendants("Cliente").Max(x => int.Parse(x.Attribute("Id")?.Value ?? "0")) : 0;

                xmlDoc.Element("Clientes").Add(new XElement("Cliente",
                    new XAttribute("Id", maxId + 1),
                    new XElement("Nombre", nombre),
                    new XElement("Apellido", apellido),
                    new XElement("Dni", dni),
                    new XElement("Telefono", telefono),
                    new XElement("Email", email),
                    new XElement("Calle", calle),
                    new XElement("Numero", numero),
                    new XElement("Piso", piso),
                    new XElement("Depto", depto),
                    new XElement("Localidad", localidad),
                    new XElement("Eliminado", "False")
                ));
                xmlDoc.Save(ruta);
                return 1;
            }
            catch (Exception) { return 0; }
        }

        public int Modificar(int id, string nombre, string apellido, string dni, string telefono, string email, string calle, string numero, string piso, string depto, string localidad)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);                
                var cliente = xmlDoc.Descendants("Cliente").FirstOrDefault(c => (int)c.Attribute("Id") == id);

                if (cliente != null)
                {
                    cliente.Element("Nombre").Value = nombre;
                    cliente.Element("Apellido").Value = apellido;
                    cliente.Element("Dni").Value = dni;
                    cliente.Element("Telefono").Value = telefono;
                    cliente.Element("Email").Value = email;
                    cliente.Element("Calle").Value = calle;
                    cliente.Element("Numero").Value = numero;
                    cliente.Element("Piso").Value = piso;
                    cliente.Element("Depto").Value = depto;
                    cliente.Element("Localidad").Value = localidad;
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Borrar(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var cliente = xmlDoc.Descendants("Cliente").FirstOrDefault(c => (int)c.Attribute("Id") == id);

                if (cliente != null)
                {
                    cliente.Element("Eliminado").Value = "True";
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Deshacer_Borrar(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var cliente = xmlDoc.Descendants("Cliente").FirstOrDefault(c => (int)c.Attribute("Id") == id);

                if (cliente != null)
                {
                    cliente.Element("Eliminado").Value = "False";
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public DataTable Buscar_Todos(bool incluirEliminados = false)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Apellido");
            dt.Columns.Add("Dni");
            dt.Columns.Add("Telefono");
            dt.Columns.Add("Email");
            dt.Columns.Add("Calle");
            dt.Columns.Add("Numero");
            dt.Columns.Add("Piso");
            dt.Columns.Add("Depto");
            dt.Columns.Add("Localidad");
            dt.Columns.Add("Eliminado");

            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = incluirEliminados
                    ? xmlDoc.Descendants("Cliente")
                    : xmlDoc.Descendants("Cliente").Where(c => (string)c.Element("Eliminado") == "False");

                foreach (var cliente in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = cliente.Attribute("Id").Value;
                    dr["Nombre"] = cliente.Element("Nombre").Value;
                    dr["Apellido"] = cliente.Element("Apellido").Value;
                    dr["Dni"] = cliente.Element("Dni").Value;
                    dr["Telefono"] = cliente.Element("Telefono").Value;
                    dr["Email"] = cliente.Element("Email").Value;
                    dr["Calle"] = cliente.Element("Calle").Value;
                    dr["Numero"] = cliente.Element("Numero").Value;
                    dr["Piso"] = cliente.Element("Piso").Value;
                    dr["Depto"] = cliente.Element("Depto").Value;
                    dr["Localidad"] = cliente.Element("Localidad").Value;
                    dr["Eliminado"] = cliente.Element("Eliminado").Value;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public int CantidadClientes()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                return xmlDoc.Descendants("Cliente").Count();
            }
            catch (Exception) { return 0; }
        }
    }
}
