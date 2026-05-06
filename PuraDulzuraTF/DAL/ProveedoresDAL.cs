using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public class ProveedoresDAL
    {
        private string ruta;

        public ProveedoresDAL()
        {
            try
            {
                // Configuración de la ruta al archivo
                string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
                string archivo = "Proveedores.xml";
                ruta = Path.Combine(directorio, archivo);

                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                if (!File.Exists(ruta))
                {
                    var nuevoDoc = new XDocument(new XElement("Proveedores"));
                    nuevoDoc.Save(ruta);
                }
            }
            catch (Exception ex)
            {
                // Relanzar la excepción para que pueda ser manejada por la BLL/UI
                throw new Exception("Error al crear o acceder al archivo Proveedores.xml", ex);
            }
        }

        // --- Métodos CRUD ---

        // 1. Agregar / Dar de Alta
        public int Agregar(Proveedores proveedor)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                // Calcula el próximo ID disponible
                int maxId = xmlDoc.Descendants("Proveedor").Any()
                    ? xmlDoc.Descendants("Proveedor").Max(x => int.Parse(x.Attribute("Id")?.Value ?? "0"))
                    : 0;

                proveedor.Id = maxId + 1;

                xmlDoc.Element("Proveedores").Add(new XElement("Proveedor",
                    new XAttribute("Id", proveedor.Id),
                    new XElement("RazonSocial", proveedor.RazonSocial),
                    new XElement("CUIT", proveedor.CUIT),
                    new XElement("Telefono", proveedor.Telefono),
                    new XElement("Email", proveedor.Email),
                    new XElement("Direccion", proveedor.Direccion),
                    new XElement("CondicionIVA", proveedor.CondicionIVA),
                    new XElement("FechaAlta", proveedor.FechaAlta.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XElement("EstaActivo", proveedor.EstaActivo.ToString()), // EstaActivo como indicador lógico
                    new XElement("Eliminado", "False"), // Campo para Baja Lógica

                    // Persistir la lista de Productos Suministrados **
                    new XElement("ProductosSuministrados",
                        proveedor.IdsProductosSuministrados.Select(p => new XElement("Producto", p))
                    )                
                ));
                xmlDoc.Save(ruta);
                return 1;
            }
            catch (Exception) { return 0; }
        }

        // 2. Modificar
        public int Modificar(Proveedores proveedor)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("Proveedor").FirstOrDefault(e => (int)e.Attribute("Id") == proveedor.Id);

                if (element != null)
                {
                    element.Element("RazonSocial").Value = proveedor.RazonSocial;
                    element.Element("CUIT").Value = proveedor.CUIT;
                    element.Element("Telefono").Value = proveedor.Telefono;
                    element.Element("Email").Value = proveedor.Email;
                    element.Element("Direccion").Value = proveedor.Direccion;
                    element.Element("CondicionIVA").Value = proveedor.CondicionIVA;
                    element.Element("EstaActivo").Value = proveedor.EstaActivo.ToString();

                    // Actualizar la lista de Productos Suministrados 
                    var productosElement = element.Element("ProductosSuministrados");
                    if (productosElement != null)
                    {
                        // Eliminar los elementos hijos existentes
                        productosElement.RemoveAll();
                        // Agregar los nuevos elementos
                        productosElement.Add(proveedor.IdsProductosSuministrados.Select(p => new XElement("Producto", p)));
                    }
                    else
                    {
                        // Si no existe el elemento, lo crea
                        element.Add(new XElement("ProductosSuministrados",
                            proveedor.IdsProductosSuministrados.Select(p => new XElement("Producto", p))
                        ));
                    }                  

                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // 3. Borrar (Baja Lógica)
        public int Borrar(int idProveedor)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("Proveedor").FirstOrDefault(e => (int)e.Attribute("Id") == idProveedor);

                if (element != null)
                {
                    element.Element("Eliminado").Value = "True";
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // 4. Deshacer Borrar
        public int Deshacer_Borrar(int idProveedor)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("Proveedor").FirstOrDefault(e => (int)e.Attribute("Id") == idProveedor);

                if (element != null)
                {
                    element.Element("Eliminado").Value = "False";
                    xmlDoc.Save(ruta);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }


        // 5. Buscar Todos (Retorna DataTable)
        public DataTable Buscar_Todos(bool incluirEliminados = false)
        {
            DataTable dt = new DataTable();

            // Definición de las columnas del DataTable 
            dt.Columns.Add("Id");
            dt.Columns.Add("RazonSocial");
            dt.Columns.Add("CUIT");
            dt.Columns.Add("Telefono");
            dt.Columns.Add("Email");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("CondicionIVA");
            dt.Columns.Add("FechaAlta");
            dt.Columns.Add("EstaActivo");
            dt.Columns.Add("Eliminado");

            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);

                var consulta = incluirEliminados
                    ? xmlDoc.Descendants("Proveedor")
                    : xmlDoc.Descendants("Proveedor").Where(p => (string)p.Element("Eliminado") == "False");

                foreach (var proveedor in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = proveedor.Attribute("Id").Value;
                    dr["RazonSocial"] = proveedor.Element("RazonSocial").Value;
                    dr["CUIT"] = proveedor.Element("CUIT").Value;
                    dr["Telefono"] = proveedor.Element("Telefono").Value;
                    dr["Email"] = proveedor.Element("Email").Value;
                    dr["Direccion"] = proveedor.Element("Direccion").Value;
                    dr["CondicionIVA"] = proveedor.Element("CondicionIVA").Value;
                    dr["FechaAlta"] = proveedor.Element("FechaAlta").Value;
                    dr["EstaActivo"] = proveedor.Element("EstaActivo").Value;
                    dr["Eliminado"] = proveedor.Element("Eliminado").Value;
                    // Nota: No se incluye ProductosSuministrados aca porque DataTable es plano.
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }

            return dt;
        }

        // --- Método Auxiliar ---

        // Método para buscar una entidad específica por CUIT (necesario para la validación de la BLL)
        public Proveedores Buscar_Por_CUIT(string cuit)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("Proveedor")
                    .FirstOrDefault(e => (string)e.Element("CUIT") == cuit && (string)e.Element("Eliminado") == "False");

                if (element != null)
                {
                    // Convertir el XElement a la entidad Proveedor                    
                    return new Proveedores
                    {
                        Id = (int)element.Attribute("Id"),
                        RazonSocial = element.Element("RazonSocial").Value,
                        CUIT = element.Element("CUIT").Value,
                        EstaActivo = (bool)element.Element("EstaActivo"),
                        FechaAlta = DateTime.Parse(element.Element("FechaAlta").Value)
                    };
                }
                return null;
            }
            catch (Exception) { return null; }
        }

        public XElement TraerElementoPorId(int id) // Retorna el XElement crudo para que el Mapper haga el trabajo completo
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ruta);
                var element = xmlDoc.Descendants("Proveedor")
                    .FirstOrDefault(e => (int)e.Attribute("Id") == id && (string)e.Element("Eliminado") == "False");

                // Retorna el elemento XML crudo.
                return element;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
