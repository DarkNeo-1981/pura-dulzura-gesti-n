using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using ENTITY;

namespace MAPPER
{
    public static class OrdenCompraMapper
    {
        // 1. Mapear un XElement (XML) a una Entidad OrdenCompra
        public static OrdenCompra MapearXElementAEntidad(XElement ordenElement)
        {
            // Leer el elemento EstadoPago, si no existe, asigna "PENDIENTE DE PAGO"
            string estadoPago = ordenElement.Element("EstadoPago")?.Value ?? "PENDIENTE DE PAGO";

            OrdenCompra orden = new OrdenCompra
            {
                Id = (int)ordenElement.Attribute("Id"),
                FechaEmision = (DateTime)ordenElement.Element("FechaEmision"),
                Estado = ordenElement.Element("Estado")?.Value,
                EstadoPago = estadoPago, 
                Proveedor = new ENTITY.Proveedores { Id = (int)ordenElement.Element("IdProveedor") },
                Detalles = new List<ENTITY.DetalleOrdenCompra>()
            };

            XElement detallesElement = ordenElement.Element("Detalles");
            if (detallesElement != null)
            {
                foreach (XElement detalleElement in detallesElement.Elements("Item")) 
                {
                    orden.Detalles.Add(new ENTITY.DetalleOrdenCompra
                    {
                        IdProducto = (int)detalleElement.Element("IdProducto"),
                        Producto = detalleElement.Element("ProductoNombre")?.Value, 
                        Cantidad = (int)detalleElement.Element("Cantidad"),
                        PrecioUnitario = (decimal)detalleElement.Element("PrecioUnitario")
                    });
                }
            }

            return orden;
        }

        // 2. Mapear una Entidad OrdenCompra a un XElement (para guardar en XML)
        public static XElement MapearEntidadAXElement(OrdenCompra orden)
        {
            if (orden == null) return null;

            var ordenElement = new XElement("OrdenCompra",
                new XAttribute("Id", orden.Id),
                new XElement("FechaEmision", orden.FechaEmision.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Estado", orden.Estado),
                new XElement("EstadoPago", orden.EstadoPago ?? "PENDIENTE DE PAGO"),
                new XElement("IdProveedor", orden.Proveedor?.Id ?? 0),

                // Persistencia de los Detalles
                new XElement("Detalles",
                    orden.Detalles.Select(detalle => new XElement("Item",
                        new XElement("IdProducto", detalle.IdProducto),
                        new XElement("ProductoNombre", detalle.Producto), 
                        new XElement("Cantidad", detalle.Cantidad),
                        new XElement("PrecioUnitario", detalle.PrecioUnitario)
                    ))
                )
            );
            return ordenElement;
        }
    }
}