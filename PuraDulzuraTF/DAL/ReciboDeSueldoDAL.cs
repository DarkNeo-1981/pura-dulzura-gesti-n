using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL
{
    public class ReciboDeSueldoDAL
    {
        public ReciboDeSueldoDAL() { } // Constructor vacío

        // ==========================================================
        // FUNCIÓN AUXILIAR (Calcula la ruta dinámica y asegura las carpetas)
        // ==========================================================
        private string ObtenerRutaPorPeriodo(DateTime periodo)
        {
            // 1. Se define la ruta base con la estructura Sueldos/Recibos
            string directorioBase = Path.Combine(Environment.CurrentDirectory, "Sueldos", "Recibos");

            // 2. SE añade Año y Mes
            string directorioPeriodo = Path.Combine(directorioBase,
                                                    periodo.Year.ToString(),
                                                    periodo.Month.ToString("00"));

            string rutaArchivo = Path.Combine(directorioPeriodo, "Recibos.xml");

            // 3. Crear los directorios si no existen
            if (!Directory.Exists(directorioPeriodo))
            {
                // Crea todos los directorios (Sueldos, Recibos, Año, Mes)
                Directory.CreateDirectory(directorioPeriodo);
            }

            return rutaArchivo;
        }

        // ==========================================================
        // MÉTODO DE ESCRITURA (GuardarRecibo)
        // Se guarda el recibo en un archivo XML específico del período (Año/Mes).
        // ==========================================================
        public void GuardarRecibo(ReciboDeSueldo recibo)
        {
            string rutaRecibo = ObtenerRutaPorPeriodo(recibo.Periodo);

            try
            {
                // Si el archivo no existe (primer recibo del mes), se crea con el nodo raíz.
                if (!File.Exists(rutaRecibo))
                {
                    new XDocument(new XElement("Recibos")).Save(rutaRecibo);
                }

                XDocument xmlDoc = XDocument.Load(rutaRecibo);

                // 1. Verificar y Eliminar duplicados del mismo DNI en el mismo período (Archivo)
                xmlDoc.Descendants("Recibo")
                    .Where(r => (int)r.Element("DNI_Empleado") == recibo.DNI_Empleado)
                    .Remove(); // No se necesita filtrar por periodo, ya que el archivo es del periodo.

                // 2. Generar un nuevo ID basado en el máximo existente
                int nuevoId = xmlDoc.Descendants("Recibo").Any()
                                 ? xmlDoc.Descendants("Recibo").Max(r => (int?)r.Attribute("Id") ?? 0) + 1
                                 : 1;

                // 3. Crear el nuevo elemento Recibo
                XElement nuevoRecibo = new XElement("Recibo",
                    new XAttribute("Id", nuevoId),
                    new XElement("DNI_Empleado", recibo.DNI_Empleado),
                    new XElement("Periodo", recibo.Periodo.ToString("yyyy-MM-dd")),
                    new XElement("FechaEmision", recibo.FechaEmision.ToString("yyyy-MM-dd HH:mm:ss")),

                    // Propiedades de Totales
                    new XElement("Bruto", recibo.Bruto),
                    new XElement("NetoAPagar", recibo.NetoAPagar),

                    // 4. Agregar el Detalle del Recibo
                    new XElement("Detalle",
                        recibo.Detalle.Select(item => new XElement("Item",
                            new XElement("Concepto", item.Concepto),
                            new XElement("Tipo", item.Tipo), // Haber o Descuento
                            new XElement("Monto", item.Monto)
                        ))
                    )
                );

                xmlDoc.Element("Recibos").Add(nuevoRecibo);
                xmlDoc.Save(rutaRecibo);
            }
            catch (Exception ex)
            {                
                throw new Exception($"Error al guardar el recibo de sueldo para DNI {recibo.DNI_Empleado}: {ex.Message}");
            }
        }

        // ==========================================================
        // MÉTODOS DE CONSULTA (ExisteReciboParaPeriodo)
        // Busca en el archivo XML específico del período.
        // ==========================================================
        public bool ExisteReciboParaPeriodo(int dni, DateTime periodo)
        {
            string rutaRecibo = ObtenerRutaPorPeriodo(periodo);

            if (!File.Exists(rutaRecibo)) return false;

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaRecibo);
                // El filtro es solo por DNI, ya que el archivo XML ya es del período.
                return xmlDoc.Descendants("Recibo")
                             .Any(r => (int)r.Element("DNI_Empleado") == dni);
            }
            catch (Exception)
            {                
                return false;
            }
        }

        // ==========================================================
        // MÉTODOS DE CONSULTA (ObtenerRecibosPorDni)
        // Recorre todos los archivos de recibos existentes.
        // ==========================================================
        public List<ReciboDeSueldo> ObtenerRecibosPorDni(int dni)
        {
            List<ReciboDeSueldo> recibos = new List<ReciboDeSueldo>();
            string directorioBase = Path.Combine(Environment.CurrentDirectory, "Sueldos", "Recibos");

            if (!Directory.Exists(directorioBase)) return recibos;

            try
            {
                // Obtiene todos los archivos Recibos.xml en cualquier subdirectorio bajo Sueldos/Recibos
                string[] rutasArchivos = Directory.GetFiles(directorioBase, "Recibos.xml", SearchOption.AllDirectories);

                foreach (string ruta in rutasArchivos)
                {
                    XDocument xmlDoc = XDocument.Load(ruta);
                    var consulta = from r in xmlDoc.Descendants("Recibo")
                                   where (int)r.Element("DNI_Empleado") == dni
                                   select r;

                    foreach (XElement xElement in consulta)
                    {
                        recibos.Add(MapearXElementARecibo(xElement));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los recibos de sueldo: {ex.Message}");
            }
            return recibos;
        }

        // ==========================================================
        // MÉTODO PRIVADO AUXILIAR (Mapeo)
        // Reconstruye el Recibo, incluyendo la lista Detalle.
        // ==========================================================
        private ReciboDeSueldo MapearXElementARecibo(XElement xElement)
        {
            ReciboDeSueldo recibo = new ReciboDeSueldo
            {                 
                DNI_Empleado = (int)xElement.Element("DNI_Empleado"),
                Periodo = DateTime.Parse(xElement.Element("Periodo").Value),
                FechaEmision = DateTime.Parse(xElement.Element("FechaEmision").Value),
                Bruto = decimal.Parse(xElement.Element("Bruto")?.Value ?? "0"),
                NetoAPagar = decimal.Parse(xElement.Element("NetoAPagar")?.Value ?? "0"),
                Detalle = new List<ItemRecibo>()
            };

            // Mapeo los Items del Detalle
            var detalleElement = xElement.Element("Detalle");
            if (detalleElement != null)
            {
                recibo.Detalle.AddRange(detalleElement.Descendants("Item").Select(item => new ItemRecibo
                {
                    Concepto = item.Element("Concepto")?.Value,
                    Tipo = item.Element("Tipo")?.Value,
                    Monto = decimal.Parse(item.Element("Monto")?.Value ?? "0")
                }));
            }

            // Asignación del Id 
            if (xElement.Attribute("Id") != null)
            {                
                recibo.Id = (int)xElement.Attribute("Id");
            }

            return recibo;
        }
    }
}