using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace MAPPER
{
    public static class ProveedoresMapper
    {
        // Método 1: Convierte un DataTable (usado en la DAL) a una lista de Entidades
        public static List<Proveedores> MapearDataTableAEntidades(DataTable dt)
        {
            List<Proveedores> lista = new List<Proveedores>();

            if (dt == null) return lista;

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDataRowAEntidad(row));
            }
            return lista;
        }

        // Método 2: Convierte una fila de datos a una Entidad Proveedor
        public static Proveedores MapearDataRowAEntidad(DataRow row)
        {
            Proveedores proveedor = new Proveedores();

            proveedor.Id = Convert.ToInt32(row["Id"]);
            proveedor.RazonSocial = row["RazonSocial"].ToString();
            proveedor.CUIT = row["CUIT"].ToString();
            proveedor.Telefono = row["Telefono"].ToString();
            proveedor.Email = row["Email"].ToString();
            proveedor.Direccion = row["Direccion"].ToString();
            proveedor.CondicionIVA = row["CondicionIVA"].ToString();
            proveedor.FechaAlta = DateTime.Parse(row["FechaAlta"].ToString());
            proveedor.EstaActivo = Convert.ToBoolean(row["EstaActivo"]);
            proveedor.Eliminado = Convert.ToBoolean(row["Eliminado"]); 

            if (row.Table.Columns.Contains("IdsProductosSuministrados"))
            {
                string idsString = row["IdsProductosSuministrados"]?.ToString();

                if (!string.IsNullOrWhiteSpace(idsString))
                {
                    // Deserializar el string "1,5,8" a List<int> {1, 5, 8}
                    proveedor.IdsProductosSuministrados = idsString.Split(',')
                                                                 .Where(s => !string.IsNullOrWhiteSpace(s))
                                                                 .Select(s => Convert.ToInt32(s))
                                                                 .ToList();
                }
            }

            return proveedor;
        }

        // Método 3: Convierte un XElement a una Entidad Proveedor (para TraerPorId)
        public static Proveedores MapearXElementAEntidad(XElement element)
        {
            if (element == null) return null;

            // Cambiar "IdProducto" a "Producto"
            var idsProductos = element.Element("ProductosSuministrados")?
                                     .Elements("Producto")
                                     .Select(p => Convert.ToInt32(p.Value))
                                     .ToList() ?? new List<int>();


            // Mapear el XElement crudo a la entidad Proveedor
            return new ENTITY.Proveedores
            {
                Id = (int)element.Attribute("Id"),
                RazonSocial = element.Element("RazonSocial").Value,
                CUIT = element.Element("CUIT").Value,
                Telefono = element.Element("Telefono").Value,
                Email = element.Element("Email").Value,
                Direccion = element.Element("Direccion").Value,
                CondicionIVA = element.Element("CondicionIVA").Value,
                FechaAlta = DateTime.Parse(element.Element("FechaAlta").Value),
                EstaActivo = (bool)element.Element("EstaActivo"),
                Eliminado = (bool)element.Element("Eliminado"),
                IdsProductosSuministrados = idsProductos 
            };
        }

        // Método 4: Convierte una Entidad Proveedor a un XElement (para Guardar/Modificar en XML)
        public static XElement MapearEntidadAXElement(Proveedores entidad)
        {
            XElement productosElement = new XElement("IdsProductosSuministrados");
            
            foreach (var id in entidad.IdsProductosSuministrados)
            {
                productosElement.Add(new XElement("IdProducto", id.ToString()));
            }

            return new XElement("Proveedor",
                new XAttribute("Id", entidad.Id.ToString()),
                new XElement("RazonSocial", entidad.RazonSocial),
                new XElement("CUIT", entidad.CUIT),
                new XElement("Telefono", entidad.Telefono),
                new XElement("Email", entidad.Email),
                new XElement("Direccion", entidad.Direccion),
                new XElement("CondicionIVA", entidad.CondicionIVA),
                new XElement("FechaAlta", entidad.FechaAlta.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("EstaActivo", entidad.EstaActivo.ToString()),
                new XElement("Eliminado", entidad.Eliminado.ToString()),
                productosElement 
            );
        }
    }
}