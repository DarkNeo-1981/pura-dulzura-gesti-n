using ENTITY;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System;

namespace DAL
{
    // Clase base abstracta para manejar la persistencia de todos los roles de Empleado.
    public abstract class EmpleadoBaseDAL<T> where T : EmpleadoBase
    {
        protected readonly string rutaArchivo;
        protected abstract string NombreElementoRaiz { get; }
        protected abstract string NombreElementoSingular { get; }        

        public EmpleadoBaseDAL(string nombreArchivo)
        {
            string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
            rutaArchivo = Path.Combine(directorio, nombreArchivo);

            if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);
            if (!File.Exists(rutaArchivo))
            {
                new XDocument(new XElement(NombreElementoRaiz)).Save(rutaArchivo);
            }
        }
        
        protected abstract T MapearXElementAEmpleado(XElement xElement);

        // *** Métodos Requeridos por EmpleadoServiceBLL ***

        public T ObtenerPorDNI(int dni)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var element = xmlDoc.Descendants(NombreElementoSingular)
                    .FirstOrDefault(e =>
                    {
                        int parsedDni;
                        return int.TryParse((string)e.Attribute("DNI"), out parsedDni) && parsedDni == dni;
                    });


                if (element != null)
                {
                    return MapearXElementAEmpleado(element);
                }
            }
            catch (Exception)
            {
                // Usar BitacoraDAL
            }
            return null;
        }

        public List<T> ObtenerTodos()
        {
            List<T> lista = new List<T>();
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                lista = xmlDoc.Descendants(NombreElementoSingular)
                              .Select(MapearXElementAEmpleado)
                              .ToList();
            }
            catch (Exception)
            {
                // Usar BitacoraDAL
            }
            return lista;
        }
    }
}