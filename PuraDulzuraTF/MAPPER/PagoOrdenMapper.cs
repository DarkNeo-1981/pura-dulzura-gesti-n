using System;
using System.Xml.Linq;
using System.Globalization;
using ENTITY;

namespace MAPPER
{    
    public class PagoOrdenMapper
    {
        // Usa InvariantCulture para asegurar la consistencia de decimales y fechas en XML
        private readonly CultureInfo culture = CultureInfo.InvariantCulture;

        // 1. Mapear Entidad PagoOrden a XElement (Para guardar en DAL)
        public XElement EntidadToXml(PagoOrden pago)
        {
            if (pago == null) return null;

            var pagoElement = new XElement("Pago",
                new XAttribute("IdPago", pago.IdPago), 
                new XElement("IdOrden", pago.IdOrden),
                new XElement("Fecha", pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss", culture)),               
                new XElement("Monto", pago.Monto.ToString(culture))
            );
            return pagoElement;
        }

        // 2. Mapear XElement a Entidad PagoOrden (Para leer de DAL)
        public PagoOrden XmlToEntidad(XElement pagoElement)
        {
            if (pagoElement == null) return null;

            // Intenta parsear el monto usando InvariantCulture para el decimal
            decimal.TryParse(pagoElement.Element("Monto")?.Value,
                             NumberStyles.Any,
                             culture,
                             out decimal monto);

            // Intenta obtener la fecha
            DateTime.TryParse(pagoElement.Element("Fecha")?.Value, culture, DateTimeStyles.None, out DateTime fecha);

            return new PagoOrden
            {                
                IdPago = int.Parse(pagoElement.Attribute("IdPago")?.Value ?? "0"),
                IdOrden = (int)pagoElement.Element("IdOrden"),
                FechaPago = fecha,
                Monto = monto
            };
        }
    }
}