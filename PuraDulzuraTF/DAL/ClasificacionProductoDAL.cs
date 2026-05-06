using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL
{
    public class ClasificacionProductoDAL
    {
        private string ruta;

        public ClasificacionProductoDAL()
        {
            try
            {
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "ClasificacionProductos.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                if (!File.Exists(ruta))
                {
                    var nuevoDoc = new XDocument(new XElement("Clasificaciones"));
                    nuevoDoc.Save(ruta);
                }
            }
            catch (Exception ex)
            {               
                throw new Exception("Error al inicializar el archivo ClasificacionProductos.xml", ex);
            }
        }

        public XDocument ObtenerDocumento()
        {            
            return XDocument.Load(ruta);
        }

        public void Guardar(XElement nuevoElemento)
        {
            try
            {
                var doc = ObtenerDocumento();
                doc.Element("Clasificaciones").Add(nuevoElemento);
                doc.Save(ruta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el nuevo elemento", ex);
            }
        }

        public void Modificar(XElement elementoModificado)
        {
            try
            {
                var doc = ObtenerDocumento();
                string id = elementoModificado.Attribute("Id")?.Value;

                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("El elemento a modificar no tiene un atributo 'Id'.");
                }

                var nodo = doc.Descendants("Item").FirstOrDefault(x => x.Attribute("Id")?.Value == id);
                if (nodo == null)
                {
                    // Lanza una excepción si el nodo no se encuentra
                    throw new InvalidOperationException($"El elemento con Id '{id}' no se encontró para modificar.");
                }

                nodo.ReplaceWith(elementoModificado);
                doc.Save(ruta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el elemento", ex);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                var doc = ObtenerDocumento();
                var nodo = doc.Descendants("Item").FirstOrDefault(x => (int?)x.Attribute("Id") == id);
                if (nodo == null)
                {
                    // Lanza una excepción si el nodo no se encuentra
                    throw new InvalidOperationException($"El elemento con Id '{id}' no se encontró para eliminar.");
                }

                // Verificación para evitar NullReferenceException
                var elementoEliminado = nodo.Element("Eliminado");
                if (elementoEliminado != null)
                {
                    elementoEliminado.Value = "True";
                }
                else
                {
                    throw new InvalidOperationException("El elemento a eliminar no contiene la etiqueta 'Eliminado'.");
                }
                doc.Save(ruta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el elemento", ex);
            }
        }

        public XElement[] ObtenerTodos()
        {
            try
            {
                var doc = ObtenerDocumento();
                return doc.Descendants("Item")
                            .Where(x => x.Element("Eliminado")?.Value == "False")
                            .ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer los datos del XML", ex);
            }
        }

        public int ObtenerMaxId()
        {
            try
            {
                var doc = ObtenerDocumento();
                var items = doc.Descendants("Item");
                // Me aseguro que la colección no esté vacía antes de llamar a Max
                return items.Any() ? items.Max(x => int.Parse(x.Attribute("Id")?.Value ?? "0")) : 0;
            }
            catch (Exception ex)
            {                
                throw new Exception("Error al obtener el ID máximo", ex);
            }
        }
    }
}
