using System;
using System.Collections.Generic;
using System.Data;
using ENTITY;
using DAL;
using MAPPER;

namespace BLL
{
    public class ProductoProveedorBLL
    {
        private ProductoProveedorDAL _dal;

        public ProductoProveedorBLL()
        {
            _dal = new ProductoProveedorDAL();
        }
                
        public ProductoProveedor TraerPorId(int id)
        {
            // Llama al nuevo método Buscar_Por_Id de la DAL
            var element = _dal.Buscar_Por_Id(id);

            // Utiliza el Mapper para convertir el XML a la entidad
            return ProductoProveedorMapper.MapearXElementAEntidad(element);
        }

        public int Agregar(ProductoProveedor producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("El nombre del producto es obligatorio.");

            if (_dal.Buscar_Por_Nombre(producto.Nombre) != null)
                throw new Exception("Ya existe un producto con ese nombre.");

            return _dal.Agregar(producto);
        }

        public int Modificar(ProductoProveedor producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("El nombre del producto es obligatorio.");

            var existente = _dal.Buscar_Por_Nombre(producto.Nombre);
            if (existente != null && existente.Id != producto.Id)
                throw new Exception("El nombre ya pertenece a otro producto.");

            return _dal.Modificar(producto);
        }

        public int Borrar(int id)
        {
            return _dal.Borrar(id);
        }

        public DataTable TraerTodos(bool incluirEliminados = false)
        {
            return _dal.Buscar_Todos(incluirEliminados);
        }

        // Método optimizado para llenar ComboBoxes
        public List<ProductoProveedor> ObtenerListaProductos()
        {
            DataTable dt = _dal.Buscar_Todos(false);
            return ProductoProveedorMapper.MapearDataTableAEntidades(dt);
        }
    }
}