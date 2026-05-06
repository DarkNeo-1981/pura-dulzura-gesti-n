using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using ENTITY;
using DAL.Interfaces;

namespace DAL
{
    public class OrdenCompraDAL : IOrdenCompraDAL // Implementa la Interfaz
    {
        private readonly string ruta;

        public OrdenCompraDAL()
        {
            try
            {
                // Rutas de archivos
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "OrdenesCompra.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                // Si el archivo no existe, crea la estructura XML raíz
                if (!File.Exists(ruta))
                {
                    var nuevoDoc = new XDocument(new XElement("OrdenesCompra"));
                    nuevoDoc.Save(ruta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear o acceder al archivo OrdenesCompra.xml en DAL.", ex);
            }
        }

        // --- MÉTODOS DE LA INTERFAZ IOrdenCompraDAL ---

        // 1. Agregar: Recibe el XElement, asigna nuevo ID y persiste.
        public int Agregar(XElement ordenElement)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                // Calcula el próximo ID disponible
                int maxId = xmlDoc.Descendants("OrdenCompra").Any()
                    ? xmlDoc.Descendants("OrdenCompra").Max(x => int.Parse(x.Attribute("Id")?.Value ?? "0"))
                    : 0;

                int nuevoId = maxId + 1;

                // Asigna el nuevo ID al atributo 'Id'
                ordenElement.SetAttributeValue("Id", nuevoId.ToString());

                xmlDoc.Element("OrdenesCompra").Add(ordenElement);
                xmlDoc.Save(ruta);
                return nuevoId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar agregar la orden de compra.", ex);
            }
        }

        // 2. Modificar: Recibe el ID y el XElement y reemplaza el elemento existente.
        public void Modificar(int id, XElement elementoModificado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("OrdenCompra").FirstOrDefault(e =>
                    e.Attribute("Id") != null && int.TryParse(e.Attribute("Id").Value, out int currentId) && currentId == id
                );

                if (element != null)
                {
                    // Reemplaza el elemento XML existente por el nuevo (que viene del BLL/Mapper)
                    element.ReplaceWith(elementoModificado);
                    xmlDoc.Save(ruta);
                }
                else
                {
                    throw new KeyNotFoundException($"Orden ID {id} no encontrada para modificar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar modificar la orden de compra.", ex);
            }
        }

        // 3. Traer un XElement por Id
        public XElement TraerElementoPorId(int idOrden)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                return xmlDoc.Descendants("OrdenCompra")
                    .FirstOrDefault(e => (int)e.Attribute("Id") == idOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al traer elemento por ID.", ex);
            }
        }

        // 4. Traer todos los XElement (para ser mapeados en la BLL)
        public IQueryable<XElement> TraerTodosElementos()
        {
            try
            {
                // Carga el documento y devuelve una consulta IQueryable<XElement>
                XDocument xmlDoc = XDocument.Load(ruta);
                return xmlDoc.Descendants("OrdenCompra").AsQueryable();
            }
            catch (FileNotFoundException)
            {
                // Si el archivo no existe, devuelve una lista vacía para evitar fallos
                return Enumerable.Empty<XElement>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al traer todos los elementos.", ex);
            }
        }

        // 5. Actualizar Estado de Pago (Método específico para PagoOrdenBLL)
        public void ActualizarEstadoPago(int idOrden, string nuevoEstadoPago)
        {
            try
            {
                XDocument doc = XDocument.Load(ruta);                
                XElement ordenElement = doc.Descendants("OrdenCompra")
                                           .FirstOrDefault(e => (int)e.Attribute("Id") == idOrden);

                if (ordenElement != null)
                {
                    // 1. ACTUALIZACIÓN DE ESTADOPAGO 
                    XElement estadoPagoElement = ordenElement.Element("EstadoPago");

                    if (estadoPagoElement != null)
                        estadoPagoElement.Value = nuevoEstadoPago;
                    else
                        // Si el elemento no existe, lo añade
                        ordenElement.Add(new XElement("EstadoPago", nuevoEstadoPago));

                    // 2. ACTUALIZACIÓN DEL ESTADO GENERAL DE LA ORDEN
                    string estadoGeneralNuevo = null;
                    string estadoPagoUpper = nuevoEstadoPago.ToUpper();

                    if (estadoPagoUpper == "PAGADA")
                    {
                        // Si está pagada, la orden en general se considera CERRADA
                        estadoGeneralNuevo = "CERRADA";
                    }
                    else if (estadoPagoUpper == "CANCELADA" || estadoPagoUpper == "DEVOLUCION")
                    {
                        // Si se cancela o devuelve, la orden en general se ANULA
                        estadoGeneralNuevo = "ANULADA";
                    }
                    // Si es "PENDIENTE DE PAGO" o "PAGO PARCIAL", el Estado general sigue siendo "PENDIENTE" 

                    // Si hay un estado general que actualizar:
                    if (!string.IsNullOrEmpty(estadoGeneralNuevo))
                    {
                        XElement estadoGeneralElement = ordenElement.Element("Estado");
                        if (estadoGeneralElement != null)
                        {
                            estadoGeneralElement.Value = estadoGeneralNuevo;
                        }
                        else
                        {
                            // Si el elemento no existe, lo añade
                            ordenElement.Add(new XElement("Estado", estadoGeneralNuevo));
                        }
                    }

                    // 3. Guarda el documento con ambos estados actualizados
                    doc.Save(ruta);
                }
                else
                {
                    throw new KeyNotFoundException($"Orden de Compra ID {idOrden} no encontrada para actualizar estado de pago.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error DAL al actualizar el estado de pago en el XML: {ex.Message}", ex);
            }
        }

        // 6. Cambiar Estado (Estado general de la orden: Pendiente, Recibida, Cancelada)
        public int CambiarEstado(int idOrden, string nuevoEstado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("OrdenCompra").FirstOrDefault(e => (int)e.Attribute("Id") == idOrden);

                if (element != null)
                {
                    element.Element("Estado").Value = nuevoEstado;
                    xmlDoc.Save(ruta);
                    return 1; // Retorna 1 para indicar que se modificó
                }
                return 0; // Retorna 0 si no se encontró la orden
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estado.", ex);
            }
        }

        public string ObtenerEstadoPagoActual(int idOrden)
        {
            try
            {
                XDocument doc = XDocument.Load(ruta);
                // Ajusta la búsqueda según si el Id es atributo o elemento
                XElement ordenElement = doc.Descendants("OrdenCompra")
                                           .FirstOrDefault(e => (int)e.Attribute("Id") == idOrden);

                if (ordenElement == null)
                {
                    throw new KeyNotFoundException($"Orden de Compra ID {idOrden} no encontrada.");
                }

                // Devolvemos el valor, o un valor por defecto si la etiqueta no existe.
                return ordenElement.Element("EstadoPago")?.Value ?? "PENDIENTE DE PAGO";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error DAL al obtener el estado de pago de la Orden {idOrden}: {ex.Message}", ex);
            }
        }
    }
}