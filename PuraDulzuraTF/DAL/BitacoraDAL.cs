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
    public class BitacoraDAL
    {
        private readonly string ruta;

        public BitacoraDAL()
        {
            try
            {                
                string directorio = Path.Combine(Environment.CurrentDirectory, "Bitacora");
                string archivo = "Bitacora.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                if (!File.Exists(ruta))
                {                    
                    XDocument doc = new XDocument(
                        new XElement("Bitacora",
                            new XElement("Entrada",
                                new XAttribute("Id", "0"),
                                new XElement("Usuario", "0 - admin"),
                                new XElement("Fecha", DateTime.Now.ToString()),
                                new XElement("Detalle", "Se creó la bitácora")
                            )
                        )
                    );
                    doc.Save(ruta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el archivo Bitacora.xml", ex);
            }
        }
                
        public int Agregar(string Usuario, string pDetalle)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);                
                int maxId = xmlDoc.Descendants("Entrada").Any()
                                ? xmlDoc.Descendants("Entrada").Max(x => int.Parse(x.Attribute("Id").Value.ToString()))
                                : 0;

                xmlDoc.Element("Bitacora").Add(new XElement("Entrada",
                                                                 new XAttribute("Id", maxId + 1),
                                                                 new XElement("Usuario", Usuario),
                                                                 new XElement("Fecha", DateTime.Now.ToString()),
                                                                 new XElement("Detalle", pDetalle)));
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }

        public int Agregar(string pDetalle)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                // Verificación para evitar error si no hay entradas
                int maxId = xmlDoc.Descendants("Entrada").Any()
                                ? xmlDoc.Descendants("Entrada").Max(x => int.Parse(x.Attribute("Id").Value.ToString()))
                                : 0;

                xmlDoc.Element("Bitacora").Add(new XElement("Entrada",
                                                                 new XAttribute("Id", maxId + 1),
                                                                 new XElement("Usuario", "Sin usuario"),
                                                                 new XElement("Fecha", DateTime.Now.ToString()),
                                                                 new XElement("Detalle", pDetalle)));
                xmlDoc.Save(ruta);
                resultado = 1;
            }
            catch (Exception) { return 0; }
            return resultado;
        }

        public DataTable Buscar_Todos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Detalle");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from entrada in xmlDoc.Descendants("Entrada")
                               select entrada;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = xElement.Attribute("Id").Value.ToString();
                    dr["Usuario"] = xElement.Element("Usuario").Value.ToString();
                    dr["Fecha"] = xElement.Element("Fecha").Value.ToString();
                    dr["Detalle"] = xElement.Element("Detalle").Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public DataTable Buscar_BackUps()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Detalle");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var consulta = from entrada in xmlDoc.Descendants("Entrada")
                               where entrada.Element("Detalle").Value.Contains("BackUp")
                               select entrada;
                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = xElement.Attribute("Id").Value.ToString();
                    dr["Usuario"] = xElement.Element("Usuario").Value.ToString();
                    dr["Fecha"] = xElement.Element("Fecha").Value.ToString();
                    dr["Detalle"] = xElement.Element("Detalle").Value.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }
    }
}