using System;
using System.Collections.Generic;
using System.Xml;
using ENTITY;
using DAL;
using MAPPER;
using System.Linq;
using BLL;

namespace BLL
{
    public class ProductoBLL
    {
        private ProductoDAL dal = new ProductoDAL();
        private ProductoMapper mapper = new ProductoMapper();

        public List<Producto> ObtenerTodosLosProductos()
        {
            XmlDocument doc = dal.ObtenerDocumento();
            return mapper.ObtenerTodos(doc);
        }

        public Producto TraerPorId(int idProducto)
        {
            return ObtenerTodosLosProductos().FirstOrDefault(p => p.ID == idProducto);
        }

        public List<ENTITY.Producto> ObtenerProductosDeProveedores(int idProveedor)
        {           
            ProveedoresBLL proveedorBLL = new ProveedoresBLL();
            ENTITY.Proveedores proveedor = proveedorBLL.TraerPorId(idProveedor);

            if (proveedor == null || proveedor.IdsProductosSuministrados == null)
            {
                return new List<ENTITY.Producto>();
            }

            // Instanciar ProductoProveedorBLL para obtener el catálogo del proveedor
            ProductoProveedorBLL ppBLL = new ProductoProveedorBLL();
            List<ENTITY.Producto> catalogo = new List<ENTITY.Producto>();

            foreach (int idProducto in proveedor.IdsProductosSuministrados)
            {                
                ENTITY.ProductoProveedor productoProveedor = ppBLL.TraerPorId(idProducto);

                if (productoProveedor != null)
                {
                    catalogo.Add(new ENTITY.Producto
                    {                       
                        ID = productoProveedor.Id,
                        Nombre = productoProveedor.Nombre
                    });
                }
            }

            return catalogo;
        }

        public void AgregarNuevoProducto(Producto newProducto)
        {
            ValidarProducto(newProducto);

            XmlDocument doc = dal.ObtenerDocumento();
            int nuevoID = ObtenerProximoID(doc);

            XmlElement xmlProducto = mapper.MapearAXml(doc, newProducto, nuevoID);
            dal.Guardar(xmlProducto);
        }

        private int ObtenerProximoID(XmlDocument doc)
        {
            XmlNodeList productos = doc.SelectNodes("//Producto");
            int maxID = 0;

            foreach (XmlNode nodo in productos)
            {
                int id = int.Parse(nodo.Attributes["ID"].Value);
                if (id > maxID)
                    maxID = id;
            }

            return maxID + 1;
        }

        public void ActualizarProducto(Producto producto)
        {
            ValidarProducto(producto);

            XmlDocument doc = dal.ObtenerDocumento();
            XmlElement xmlProducto = mapper.MapearAXml(doc, producto, producto.ID);
            dal.Actualizar(xmlProducto);
        }

        public void EliminarProducto(int id)
        {
            dal.Eliminar(id);
        }

        private void ValidarProducto(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("El nombre del producto no puede estar vacío.");

            if (producto.Costo <= 0)
                throw new Exception("El costo debe ser mayor a cero.");

            if (producto.PrecioDeVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor a cero.");
        }
    }
}
