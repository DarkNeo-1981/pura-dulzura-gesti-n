using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MAPPER
{
    public static class ProductoProveedorMapper
    {
        public static List<ProductoProveedor> MapearDataTableAEntidades(DataTable dt)
        {
            List<ProductoProveedor> lista = new List<ProductoProveedor>();
            if (dt == null) return lista;

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new ProductoProveedor
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    PrecioReferencia = Convert.ToDecimal(row["PrecioReferencia"]),
                    EstaActivo = Convert.ToBoolean(row["EstaActivo"]),
                    Eliminado = Convert.ToBoolean(row["Eliminado"])
                });
            }
            return lista;
        }

        public static ProductoProveedor MapearXElementAEntidad(XElement element)
        {
            if (element == null) return null;

            return new ProductoProveedor
            {
                Id = (int)element.Attribute("Id"),
                Nombre = element.Element("Nombre").Value,
                PrecioReferencia = decimal.Parse(element.Element("PrecioReferencia")?.Value ?? "0"),
                EstaActivo = bool.Parse(element.Element("EstaActivo").Value),
                Eliminado = bool.Parse(element.Element("Eliminado").Value)
            };
        }
    }
}
