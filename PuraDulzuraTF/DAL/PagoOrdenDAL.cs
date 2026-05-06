using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Globalization;
using ENTITY; 
using DAL.Interfaces;

namespace DAL
{
    public class PagoOrdenDAL : IPagoOrdenDAL
    {
        private readonly string ruta;        

        public PagoOrdenDAL()
        {
            try
            {
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "PagosOrden.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                if (!File.Exists(ruta))
                {
                    var nuevoDoc = new XDocument(new XElement("PagosOrden"));
                    nuevoDoc.Save(ruta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear o acceder al archivo PagosOrden.xml en DAL.", ex);
            }
        }

        // 1. REGISTRA UN NUEVO PAGO      
        public int Agregar(XElement pagoElement)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                // Calcula el próximo ID disponible
                int maxId = xmlDoc.Descendants("Pago").Any()
                    ? xmlDoc.Descendants("Pago").Max(x => int.Parse(x.Attribute("IdPago")?.Value ?? "0"))
                    : 0;

                int nuevoId = maxId + 1;              
                pagoElement.SetAttributeValue("IdPago", nuevoId.ToString());
                xmlDoc.Element("PagosOrden").Add(pagoElement);
                xmlDoc.Save(ruta);

                return nuevoId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DAL al intentar agregar el pago.", ex);
            }
        }

        // 2. TRAE TODOS LOS PAGOS DE UNA ORDEN        
        public IQueryable<XElement> TraerPagosPorOrdenId(int idOrden)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);                
                return xmlDoc.Descendants("Pago")
                             .Where(e => (int)e.Element("IdOrden") == idOrden)
                             .AsQueryable();
            }
            catch (FileNotFoundException)
            {
                return Enumerable.Empty<XElement>().AsQueryable(); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error DAL al intentar cargar los pagos.", ex);
            }
        }

        // 3. OBTIENE LA SUMA TOTAL PAGADA PARA UNA ORDEN
        public decimal ObtenerMontoTotalPagado(int idOrden)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                decimal total = xmlDoc.Descendants("Pago")
                                     .Where(e => (int)e.Element("IdOrden") == idOrden)
                                     .Sum(e =>
                                     {
                                         if (decimal.TryParse(e.Element("Monto")?.Value,
                                                              System.Globalization.NumberStyles.Any,
                                                              System.Globalization.CultureInfo.InvariantCulture,
                                                              out decimal monto))
                                         {
                                             return monto;
                                         }
                                         return 0m;
                                     });

                return total;
            }
            catch (FileNotFoundException)
            {
                return 0m;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DAL al intentar calcular el monto total pagado.", ex);
            }
        }
    }
}