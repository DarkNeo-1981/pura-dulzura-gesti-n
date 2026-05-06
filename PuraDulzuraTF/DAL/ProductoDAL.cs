using System;
using System.IO;
using System.Xml;

namespace DAL
{
    public class ProductoDAL
    {
        private readonly string ruta;

        public ProductoDAL()
        {
            string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
            string archivo = "Productos.xml";
            ruta = Path.Combine(directorio, archivo);

            if (!Directory.Exists(directorio))
                Directory.CreateDirectory(directorio);

            if (!File.Exists(ruta))
            {
                using (XmlTextWriter writer = new XmlTextWriter(ruta, System.Text.Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("Productos");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
        }

        public XmlDocument ObtenerDocumento()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            return doc;
        }

        public void Guardar(XmlElement productoXml)
        {
            XmlDocument doc = ObtenerDocumento();
            doc.DocumentElement.AppendChild(doc.ImportNode(productoXml, true));
            doc.Save(ruta);
        }

        public void Actualizar(XmlElement productoActualizado)
        {
            XmlDocument doc = ObtenerDocumento();

            string id = productoActualizado.GetAttribute("ID");
            XmlNode productoExistente = doc.SelectSingleNode($"//Producto[@ID='{id}']");

            if (productoExistente == null)
                throw new Exception("Producto no encontrado para actualizar.");

            XmlNode padre = productoExistente.ParentNode;
            padre.ReplaceChild(doc.ImportNode(productoActualizado, true), productoExistente);
            doc.Save(ruta);
        }

        public void Eliminar(int id)
        {
            XmlDocument doc = ObtenerDocumento();

            XmlNode productoExistente = doc.SelectSingleNode($"//Producto[@ID='{id}']");

            if (productoExistente == null)
                throw new Exception("Producto no encontrado para eliminar.");

            productoExistente.ParentNode.RemoveChild(productoExistente);
            doc.Save(ruta);
        }
    }
}
