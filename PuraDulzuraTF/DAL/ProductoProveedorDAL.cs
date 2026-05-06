using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using ENTITY;

namespace DAL
{
    public class ProductoProveedorDAL
    {
        private string ruta;

        public ProductoProveedorDAL()
        {
            try
            {
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "ProductosProveedor.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                if (!File.Exists(ruta))
                {
                    // Nodo raíz actualizado
                    var nuevoDoc = new XDocument(new XElement("ProductosProveedor"));
                    nuevoDoc.Save(ruta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear o acceder al archivo ProductosProveedor.xml", ex);
            }
        }

        // MÉTODO AÑADIDO: Implementa la búsqueda por ID del XML
        public XElement Buscar_Por_Id(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                return xmlDoc.Descendants("ProductoProveedor")
                    .FirstOrDefault(e => (int)e.Attribute("Id") == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int Agregar(ProductoProveedor producto)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                // Buscar en descendientes "ProductoProveedor"
                int maxId = xmlDoc.Descendants("ProductoProveedor").Any()
                    ? xmlDoc.Descendants("ProductoProveedor").Max(x => int.Parse(x.Attribute("Id")?.Value ?? "0"))
                    : 0;

                producto.Id = maxId + 1;

                xmlDoc.Element("ProductosProveedor").Add(new XElement("ProductoProveedor",
                    new XAttribute("Id", producto.Id),
                    new XElement("Nombre", producto.Nombre),
                    new XElement("PrecioReferencia", producto.PrecioReferencia.ToString()),
                    new XElement("EstaActivo", producto.EstaActivo.ToString()),
                    new XElement("Eliminado", "False")
                ));

                xmlDoc.Save(ruta);
                return 1;
            }
            catch (Exception) { return 0; }
        }

        public int Modificar(ProductoProveedor producto)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("ProductoProveedor").FirstOrDefault(e => (int)e.Attribute("Id") == producto.Id);

                if (element != null)
                {
                    element.Element("Nombre").Value = producto.Nombre;
                    element.Element("PrecioReferencia").Value = producto.PrecioReferencia.ToString();
                    element.Element("EstaActivo").Value = producto.EstaActivo.ToString();

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
                var element = xmlDoc.Descendants("ProductoProveedor").FirstOrDefault(e => (int)e.Attribute("Id") == id);

                if (element != null)
                {
                    element.Element("Eliminado").Value = "True";
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
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("PrecioReferencia", typeof(decimal));
            dt.Columns.Add("EstaActivo", typeof(bool));
            dt.Columns.Add("Eliminado", typeof(bool));

            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                var consulta = incluirEliminados
                    ? xmlDoc.Descendants("ProductoProveedor")
                    : xmlDoc.Descendants("ProductoProveedor").Where(p => (string)p.Element("Eliminado") == "False");

                foreach (var item in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = int.Parse(item.Attribute("Id").Value);
                    dr["Nombre"] = item.Element("Nombre").Value;
                    dr["PrecioReferencia"] = decimal.Parse(item.Element("PrecioReferencia")?.Value ?? "0");
                    dr["EstaActivo"] = bool.Parse(item.Element("EstaActivo").Value);
                    dr["Eliminado"] = bool.Parse(item.Element("Eliminado").Value);
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }

            return dt;
        }

        public ProductoProveedor Buscar_Por_Nombre(string nombre)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("ProductoProveedor")
                    .FirstOrDefault(e => (string)e.Element("Nombre") == nombre && (string)e.Element("Eliminado") == "False");

                if (element != null)
                {
                    return new ProductoProveedor
                    {
                        Id = (int)element.Attribute("Id"),
                        Nombre = element.Element("Nombre").Value,
                        PrecioReferencia = decimal.Parse(element.Element("PrecioReferencia")?.Value ?? "0"),
                        EstaActivo = bool.Parse(element.Element("EstaActivo").Value)
                    };
                }
                return null;
            }
            catch (Exception) { return null; }
        }
    }
}