using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace DAL
{
    public class OrdenDePedidoDAL
    {
        private readonly string ruta;

        public OrdenDePedidoDAL()
        {            
            try
            {
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "OrdenesDePedido.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                if (!File.Exists(ruta))
                {
                    XmlTextWriter archivoxml = new XmlTextWriter(ruta, System.Text.Encoding.UTF8);
                    archivoxml.Formatting = Formatting.Indented;
                    archivoxml.Indentation = 2;
                    archivoxml.WriteStartDocument(true);
                    archivoxml.WriteStartElement("OrdenesDePedido");
                    archivoxml.WriteEndElement(); 
                    archivoxml.WriteEndDocument();
                    archivoxml.Close();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error al crear el archivo OrdenesDePedido.xml");
            }
        }

        public int Agregar(OrdenDePedido pedido)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                int maxId = xmlDoc.Descendants("Pedido").Any()
                    ? xmlDoc.Descendants("Pedido").Max(x => int.Parse(x.Attribute("Id").Value))
                    : 0;

                XElement nuevo = new XElement("Pedido",
                    new XAttribute("Id", maxId + 1),
                    new XElement("DNI_Vendedor", pedido.DNI_Vendedor),
                    new XElement("DNI_Cliente", pedido.DNI_Cliente),
                    new XElement("FechaDeVenta", pedido.FechaDeVenta),                    
                    new XElement("Total", pedido.Total.ToString(CultureInfo.InvariantCulture)),
                    new XElement("Eliminado", "False"),                    
                    new XElement("Facturada", "False"), // Una nueva orden se inicia como NO facturada
                    new XElement("Detalles",
                        pedido.Detalles.Select(d => new XElement("Detalle",
                                new XElement("IdProducto", d.IdProducto),
                                new XElement("Cantidad", d.Cantidad),                                
                                new XElement("PrecioUnitario", d.PrecioUnitario.ToString(CultureInfo.InvariantCulture))
                            ))
                    )
                );
                xmlDoc.Element("OrdenesDePedido").Add(nuevo);
                xmlDoc.Save(ruta);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public DataTable Buscar_Todos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("DNI_Vendedor");
            dt.Columns.Add("DNI_Cliente");
            dt.Columns.Add("FechaDeVenta");
            dt.Columns.Add("Total");
            dt.Columns.Add("Eliminado");
            dt.Columns.Add("Cobrada");
            dt.Columns.Add("Facturada");

            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var pedidos = from p in xmlDoc.Descendants("Pedido")
                              select p;

                foreach (XElement p in pedidos)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = p.Attribute("Id").Value;
                    dr["DNI_Vendedor"] = p.Element("DNI_Vendedor").Value;
                    dr["DNI_Cliente"] = p.Element("DNI_Cliente").Value;
                    dr["FechaDeVenta"] = p.Element("FechaDeVenta").Value;                    
                    dr["Total"] = decimal.Parse(p.Element("Total").Value, CultureInfo.InvariantCulture);
                    dr["Eliminado"] = p.Element("Eliminado").Value;
                    dr["Cobrada"] = (p.Element("Cobrada") != null && bool.Parse(p.Element("Cobrada").Value)).ToString();
                    dr["Facturada"] = (p.Element("Facturada") != null && bool.Parse(p.Element("Facturada").Value)).ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return dt;
        }

        public DataTable Buscar_Uno(int id)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("DNI_Vendedor");
            dt.Columns.Add("DNI_Cliente");
            dt.Columns.Add("FechaDeVenta");
            dt.Columns.Add("Total");
            dt.Columns.Add("Eliminado");            
            dt.Columns.Add("Facturada");
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var pedido = (from p in xmlDoc.Descendants("Pedido")
                              where (int)p.Attribute("Id") == id
                              select p).FirstOrDefault();

                if (pedido != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = pedido.Attribute("Id").Value;
                    dr["DNI_Vendedor"] = pedido.Element("DNI_Vendedor").Value;
                    dr["DNI_Cliente"] = pedido.Element("DNI_Cliente").Value;
                    dr["FechaDeVenta"] = pedido.Element("FechaDeVenta").Value;                    
                    dr["Total"] = decimal.Parse(pedido.Element("Total").Value, CultureInfo.InvariantCulture);
                    dr["Eliminado"] = pedido.Element("Eliminado").Value;                    
                    dr["Facturada"] = (pedido.Element("Facturada") != null && bool.Parse(pedido.Element("Facturada").Value)).ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        public int Eliminar(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var pedido = xmlDoc.Descendants("Pedido")
                                   .FirstOrDefault(p => (int)p.Attribute("Id") == id);

                if (pedido != null)
                {
                    pedido.Element("Eliminado").Value = "True";
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
       
        public int MarcarComoFacturada(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var pedido = xmlDoc.Descendants("Pedido")
                                   .FirstOrDefault(p => (int)p.Attribute("Id") == id);

                if (pedido != null)
                {
                    XElement facturadaElement = pedido.Element("Facturada");
                    if (facturadaElement == null) // Si la orden es vieja y no tiene el campo, lo añade
                    {
                        pedido.Add(new XElement("Facturada", "True"));
                    }
                    else // Si ya tiene el campo, actualiza su valor
                    {
                        facturadaElement.Value = "True";
                    }
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0; // Orden no encontrada
            }
            catch (Exception)
            {
                return 0; // Error al guardar
            }
        }

        public int MarcarComoCobrada(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var pedido = xmlDoc.Descendants("Pedido")
                                   .FirstOrDefault(p => (int)p.Attribute("Id") == id);

                if (pedido != null)
                {
                    XElement cobradaElement = pedido.Element("Cobrada");
                    if (cobradaElement == null) // Si la orden es vieja y no tiene el campo, lo añade
                    {
                        pedido.Add(new XElement("Cobrada", "True"));
                    }
                    else // Si ya tiene el campo, actualiza su valor
                    {
                        cobradaElement.Value = "True";
                    }

                    xmlDoc.Save(ruta);
                    return 1; // Éxito
                }

                return 0; // Orden no encontrada
            }
            catch (Exception)
            {
                return 0; // Error al guardar
            }
        }

        private List<DetalleOrdenDePedido> ObtenerDetalles(XElement pedidoElement)
        {
            List<DetalleOrdenDePedido> detalles = new List<DetalleOrdenDePedido>();

            var detallesElement = pedidoElement.Element("Detalles");
            if (detallesElement != null)
            {
                foreach (var detalleElem in detallesElement.Elements("Detalle"))
                {
                    detalles.Add(new DetalleOrdenDePedido
                    {
                        IdProducto = int.Parse(detalleElem.Element("IdProducto").Value),
                        Cantidad = int.Parse(detalleElem.Element("Cantidad").Value),                        
                        PrecioUnitario = decimal.Parse(detalleElem.Element("PrecioUnitario").Value, CultureInfo.InvariantCulture)
                    });
                }
            }
            return detalles;
        }

        public OrdenDePedido BuscarCompleto(int id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var pedidoElement = xmlDoc.Descendants("Pedido")
                                         .FirstOrDefault(p => (int)p.Attribute("Id") == id);

                if (pedidoElement != null)
                {
                    OrdenDePedido pedido = new OrdenDePedido
                    {
                        Id = int.Parse(pedidoElement.Attribute("Id").Value),
                        DNI_Vendedor = int.Parse(pedidoElement.Element("DNI_Vendedor").Value),
                        DNI_Cliente = int.Parse(pedidoElement.Element("DNI_Cliente").Value),
                        FechaDeVenta = DateTime.Parse(pedidoElement.Element("FechaDeVenta").Value).ToString("yyyy-MM-dd"),                        
                        Total = decimal.Parse(pedidoElement.Element("Total").Value, CultureInfo.InvariantCulture),
                        Eliminado = bool.Parse(pedidoElement.Element("Eliminado").Value),
                        Cobrada = (pedidoElement.Element("Cobrada") != null) ? bool.Parse(pedidoElement.Element("Cobrada").Value) : false,
                        Facturada = (pedidoElement.Element("Facturada") != null) ? bool.Parse(pedidoElement.Element("Facturada").Value) : false,
                        Detalles = ObtenerDetalles(pedidoElement) 
                    };
                    return pedido;
                }
            }
            catch (Exception ex) 
            {               
                Console.WriteLine("Error en BuscarCompleto: " + ex.Message);
                return null;
            }
            return null;
        }
    }
}
