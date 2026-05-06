using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL
{
    public class NovedadDAL
    {        
        public NovedadDAL() { }

        // ==========================================================
        // FUNCIÓN AUXILIAR (Calcula la ruta dinámica y asegura las carpetas)
        // ==========================================================
        private string ObtenerRutaPorPeriodo(DateTime periodo)
        {
            // 1. Ruta base con la estructura Sueldos/Novedades
            string directorioBase = Path.Combine(Environment.CurrentDirectory, "Sueldos", "Novedades");

            // 2. Añade Año y Mes
            string directorioPeriodo = Path.Combine(directorioBase,
                                                    periodo.Year.ToString(),
                                                    periodo.Month.ToString("00"));

            string rutaArchivo = Path.Combine(directorioPeriodo, "Novedades.xml");

            // 3. Crea los directorios si no existen
            if (!Directory.Exists(directorioPeriodo))
            {
                // Crea todos los directorios (Sueldos, Novedades, Año, Mes)
                Directory.CreateDirectory(directorioPeriodo);
            }

            return rutaArchivo;
        }

        // ==========================================================
        // MÉTODO DE LECTURA (ObtenerNovedadesPorDniYPeriodo)
        // ==========================================================
        public List<Novedad> ObtenerNovedadesPorDniYPeriodo(int dni, DateTime periodo)
        {
            List<Novedad> novedades = new List<Novedad>();
            // La ruta se calcula acá, usando el 'periodo' que recibimos como parámetro.
            string rutaNovedad = ObtenerRutaPorPeriodo(periodo);

            if (!File.Exists(rutaNovedad))
            {
                return novedades;
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaNovedad);

                // Filtramos por DNI en el archivo de ese mes/año específico.
                var consulta = from n in xmlDoc.Descendants("Novedad")
                               where (int)n.Element("DNI_Empleado") == dni
                               select n;

                foreach (XElement xElement in consulta)
                {
                    novedades.Add(new Novedad
                    {
                        Id = (int)xElement.Attribute("Id"),
                        DNI_Empleado = (int)xElement.Element("DNI_Empleado"),                        
                        Periodo = DateTime.Parse(xElement.Element("Periodo").Value),
                        TipoNovedad = xElement.Element("TipoNovedad").Value,
                        Valor = decimal.Parse(xElement.Element("Valor").Value),
                        EsDescuento = bool.Parse(xElement.Element("EsDescuento").Value),
                        Observacion = xElement.Element("Observacion").Value
                    });
                }
            }
            catch (Exception ex)
            {
                
            }
            return novedades;
        }

        // ==========================================================
        // MÉTODO DE ESCRITURA (AgregarNovedad)
        // ==========================================================
        public void AgregarNovedad(Novedad novedad)
        {
            // La ruta se calcula acá, usando el 'periodo' del objeto Novedad.
            string rutaNovedad = ObtenerRutaPorPeriodo(novedad.Periodo);

            try
            {
                // Si el archivo no existe (primera novedad del mes), se crea con el nodo raíz.
                if (!File.Exists(rutaNovedad))
                {
                    new XDocument(new XElement("Novedades")).Save(rutaNovedad);
                }

                XDocument xmlDoc = XDocument.Load(rutaNovedad);              
                // Calcula el ID secuencial
                int nuevoId = xmlDoc.Descendants("Novedad")
                                     .Where(r => r.Attribute("Id") != null) // Filtra solo los que tienen el atributo 'Id'
                                     .Any()
                                     ? xmlDoc.Descendants("Novedad")
                                             .Max(r => (int)r.Attribute("Id")) + 1 
                                     : 1;

                xmlDoc.Element("Novedades").Add(new XElement("Novedad",
                                                   new XAttribute("Id", nuevoId),
                                                   new XElement("DNI_Empleado", novedad.DNI_Empleado),
                                                   new XElement("Periodo", novedad.Periodo.ToString("yyyy-MM-dd")),
                                                   new XElement("TipoNovedad", novedad.TipoNovedad),
                                                   new XElement("Valor", novedad.Valor),
                                                   new XElement("EsDescuento", novedad.EsDescuento),
                                                   new XElement("Observacion", novedad.Observacion)));

                xmlDoc.Save(rutaNovedad);
            }
            catch (Exception)
            {
                // Se relanza una excepción genérica para que la BLL la maneje
                throw new Exception("Error al guardar la novedad.");
            }
        }
    }
}