using System;
using System.Collections.Generic;
using System.Xml;
using ENTITY;

namespace MAPPER
{
    public class ProductoMapper
    {
        public XmlElement MapearAXml(XmlDocument doc, Producto producto, int nuevoID)
        {
            XmlElement nuevoProducto = doc.CreateElement("Producto");
            nuevoProducto.SetAttribute("ID", nuevoID.ToString());

            AgregarNodo(doc, nuevoProducto, "Nombre", producto.Nombre);
            AgregarNodo(doc, nuevoProducto, "Porciones", producto.Porciones.ToString());
            AgregarNodo(doc, nuevoProducto, "Costo", producto.Costo.ToString("F2"));
            AgregarNodo(doc, nuevoProducto, "PrecioVenta", producto.PrecioDeVenta.ToString("F2"));
            AgregarNodo(doc, nuevoProducto, "Clasificacion", producto.Clasificacion);
            AgregarNodo(doc, nuevoProducto, "Activo", producto.ProductoActivo.ToString());

            return nuevoProducto;
        }

        private void AgregarNodo(XmlDocument doc, XmlElement padre, string nombreNodo, string valor)
        {
            XmlElement elem = doc.CreateElement(nombreNodo);
            elem.InnerText = valor;
            padre.AppendChild(elem);
        }

        public Producto MapearAXmlAProducto(XmlNode nodo)
        {
            return new Producto
            {
                ID = int.Parse(nodo.Attributes["ID"].Value),
                Nombre = nodo["Nombre"]?.InnerText ?? "",
                Porciones = int.Parse(nodo["Porciones"]?.InnerText ?? "0"),
                Costo = decimal.Parse(nodo["Costo"]?.InnerText ?? "0"),
                PrecioDeVenta = decimal.Parse(nodo["PrecioVenta"]?.InnerText ?? "0"),
                Clasificacion = nodo["Clasificacion"]?.InnerText ?? "",
                ProductoActivo = bool.Parse(nodo["Activo"]?.InnerText ?? "false")
            };
        }

        public List<Producto> ObtenerTodos(XmlDocument doc)
        {
            List<Producto> lista = new List<Producto>();
            XmlNodeList nodosProductos = doc.SelectNodes("/Productos/Producto");

            foreach (XmlNode nodo in nodosProductos)
            {
                lista.Add(MapearAXmlAProducto(nodo));
            }

            return lista;
        }
    }
}
