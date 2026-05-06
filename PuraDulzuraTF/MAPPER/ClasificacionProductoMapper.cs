using ENTITY;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MAPPER
{
    public class ClasificacionProductoMapper
    {
        public XElement MapearAXml(ClasificacionProducto c)
        {
            return new XElement("Item",
                new XAttribute("Id", c.Id),
                new XElement("Detalle", c.Detalle),
                new XElement("Porciones", c.Porciones),
                new XElement("Costo", c.Costo),
                new XElement("Eliminado", c.Eliminado.ToString())
            );
        }

        public ClasificacionProducto MapearDesdeXml(XElement nodo)
        {
            return new ClasificacionProducto
            {
                Id = int.Parse(nodo.Attribute("Id").Value),
                Detalle = nodo.Element("Detalle")?.Value ?? "",
                Porciones = int.Parse(nodo.Element("Porciones")?.Value ?? "0"),
                Costo = decimal.Parse(nodo.Element("Costo")?.Value ?? "0"),
                Eliminado = bool.Parse(nodo.Element("Eliminado")?.Value ?? "False")
            };
        }

        public List<ClasificacionProducto> MapearLista(IEnumerable<XElement> nodos)
        {
            var lista = new List<ClasificacionProducto>();
            foreach (var nodo in nodos)
                lista.Add(MapearDesdeXml(nodo));
            return lista;
        }
    }
}
