using ENTITY;
using System; 
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace DAL
{
    public class ProductoRepository
    {
        private readonly string rutaArchivo;
        private readonly string directorioDB;

        public ProductoRepository()
        {
            directorioDB = Path.Combine(Environment.CurrentDirectory, "DB");
            rutaArchivo = Path.Combine(directorioDB, "Productos.xml");

            if (!Directory.Exists(directorioDB))
            {
                Directory.CreateDirectory(directorioDB);
            }

            if (!File.Exists(rutaArchivo))
            {
                try
                {
                    XDocument newDoc = new XDocument(new XElement("Productos"));
                    newDoc.Save(rutaArchivo);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al crear el archivo Productos.xml desde ProductoRepository: {ex.Message}", ex);
                }
            }
        }

        public List<Producto> ObtenerTodos()
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"El archivo Productos.xml no se encuentra en la ruta esperada: {rutaArchivo}. Asegúrate de que ProductoDAL lo haya creado.", rutaArchivo);
            }

            XDocument doc = XDocument.Load(rutaArchivo);
            var productos = doc.Descendants("Producto")
                .Select(x => new Producto
                {
                    ID = (int)x.Attribute("ID"),
                    Nombre = (string)x.Element("Nombre"),
                    PrecioDeVenta = decimal.Parse(x.Element("PrecioVenta").Value, CultureInfo.GetCultureInfo("es-AR"))
                }).ToList();

            return productos;
        }
    }
}
