using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace DAL
{
    public class ClienteRepository
    {
        private readonly string rutaArchivo;
        private readonly string directorioDB;

        public ClienteRepository()
        {            
            directorioDB = Path.Combine(Environment.CurrentDirectory, "DB");
            rutaArchivo = Path.Combine(directorioDB, "Clientes.xml");

            if (!Directory.Exists(directorioDB))
            {
                Directory.CreateDirectory(directorioDB);
            }

            if (!File.Exists(rutaArchivo))
            {
                try
                {                    
                    XDocument doc = new XDocument(new XElement("Clientes"));
                    doc.Save(rutaArchivo);
                }
                catch (Exception ex)
                {                    
                    throw new Exception("Error al crear el archivo Clientes.xml desde ClienteRepository.", ex);
                }
            }
        }

        public List<Clientes> ObtenerTodos()
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"El archivo Clientes.xml no se encuentra en la ruta esperada: {rutaArchivo}.");
            }
            try
            {
                XDocument doc = XDocument.Load(rutaArchivo);
                var clientes = doc.Descendants("Cliente")
                    .Select(x => new Clientes
                    {
                        Dni = (string)x.Element("Dni"),
                        Apellido = (string)x.Element("Apellido"),
                        Nombre = (string)x.Element("Nombre"),
                        Calle = (string)x.Element("Calle"),
                        Numero = (string)x.Element("Numero"),
                        Piso = (string)x.Element("Piso"),
                        Depto = (string)x.Element("Depto"),
                        Localidad = (string)x.Element("Localidad")
                    }).ToList();

                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer el archivo Clientes.xml", ex);
            }
        }        

        public void Guardar(Clientes nuevoCliente)
        {
            try
            {
                XDocument doc = XDocument.Load(rutaArchivo);
                int maxId = doc.Descendants("Cliente").Any() ? doc.Descendants("Cliente").Max(x => (int)x.Attribute("Id")) : 0;

                XElement nuevoElemento = new XElement("Cliente",
                    new XAttribute("Id", maxId + 1),
                    new XElement("Nombre", nuevoCliente.Nombre),
                    new XElement("Apellido", nuevoCliente.Apellido),
                    new XElement("Dni", nuevoCliente.Dni),
                    new XElement("Telefono", nuevoCliente.Telefono),
                    new XElement("Email", nuevoCliente.Email),
                    new XElement("Calle", nuevoCliente.Calle),
                    new XElement("Numero", nuevoCliente.Numero),
                    new XElement("Piso", nuevoCliente.Piso),
                    new XElement("Depto", nuevoCliente.Depto),
                    new XElement("Localidad", nuevoCliente.Localidad),
                    new XElement("Eliminado", "False")
                );

                doc.Element("Clientes").Add(nuevoElemento);
                doc.Save(rutaArchivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el nuevo cliente.", ex);
            }
        }
    }
}
