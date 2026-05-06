using DAL;
using ENTITY;
using MAPPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq; 

namespace BLL
{
    public class ProveedoresBLL
    {
        private ProveedoresDAL _ProveedoresDAL;
        // NUEVA BLL para buscar los datos completos de los productos
        private ProductoProveedorBLL _productoBLL;

        public ProveedoresBLL()
        {
            _ProveedoresDAL = new ProveedoresDAL();
            // Inicializar la nueva BLL
            _productoBLL = new ProductoProveedorBLL();
        }

        // --- Operaciones Principales ---

        public int Agregar(Proveedores proveedor)
        {
            try
            {
                // 1. VALIDACIÓN DE NEGOCIO: Campos Obligatorios
                if (string.IsNullOrWhiteSpace(proveedor.RazonSocial) ||
                    string.IsNullOrWhiteSpace(proveedor.CUIT) ||
                    string.IsNullOrWhiteSpace(proveedor.Direccion))
                {
                    throw new Exception("La Razón Social, CUIT y Dirección son campos obligatorios.");
                }

                // VALIDACIÓN DE NEGOCIO: ASIGNACIÓN DE PRODUCTOS 
                if (proveedor.IdsProductosSuministrados == null || !proveedor.IdsProductosSuministrados.Any())
                {
                    throw new Exception("Debe asignar al menos un producto suministrado.");
                }

                // 2. VALIDACIÓN DE NEGOCIO: CUIT Duplicado
                if (_ProveedoresDAL.Buscar_Por_CUIT(proveedor.CUIT) != null)
                {
                    throw new Exception("El CUIT ingresado ya se encuentra registrado.");
                }

                // 3. Persistencia
                return _ProveedoresDAL.Agregar(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al intentar agregar el Proveedores: " + ex.Message);
            }
        }

        public int Modificar(Proveedores proveedor)
        {
            try
            {
                // 1. VALIDACIÓN DE NEGOCIO: CUIT Único (Excluyendo el ID actual)
                Proveedores ProveedoresExistente = _ProveedoresDAL.Buscar_Por_CUIT(proveedor.CUIT);

                if (ProveedoresExistente != null && ProveedoresExistente.Id != proveedor.Id)
                {
                    throw new Exception("El CUIT ingresado ya pertenece a otro proveedor.");
                }

                // VALIDACIÓN DE NEGOCIO: ASIGNACIÓN DE PRODUCTOS
                if (proveedor.IdsProductosSuministrados == null || !proveedor.IdsProductosSuministrados.Any())
                {
                    throw new Exception("Debe asignar al menos un producto suministrado.");
                }


                // 2. Persistencia
                return _ProveedoresDAL.Modificar(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al intentar modificar el proveedor: " + ex.Message);
            }
        }

        // 3. Búsqueda y Mapeo
        public DataTable TraerProveedores(bool incluirEliminados = false)
        {
            try
            {
                return _ProveedoresDAL.Buscar_Todos(incluirEliminados);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al buscar proveedores: " + ex.Message);
            }
        }

        // 4. Baja Lógica
        public int Borrar(int idProveedor)
        {
            try
            {
                return _ProveedoresDAL.Borrar(idProveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al intentar dar de baja el proveedor: " + ex.Message);
            }
        }

        public int Deshacer_Borrar(int idProveedor)
        {
            try
            {
                return _ProveedoresDAL.Deshacer_Borrar(idProveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al intentar deshacer la baja del proveedor: " + ex.Message);
            }
        }

        public Proveedores TraerPorId(int id)
        {
            try
            {
                // 1. La BLL llama a la DAL para obtener el elemento XML crudo
                XElement element = _ProveedoresDAL.TraerElementoPorId(id);

                // 2. La BLL utiliza el Mapper para convertir el elemento crudo a la entidad
                return ProveedoresMapper.MapearXElementAEntidad(element);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al intentar buscar el proveedor por ID: " + ex.Message);
            }
        }

        // Usa los IDs para buscar los productos completos (Nombres)
        public List<ENTITY.ProductoProveedor> ObtenerCatalogoPorProveedor(int idProveedor)
        {
            try
            {
                // 1. Obtiene la entidad Proveedor
                ENTITY.Proveedores proveedor = TraerPorId(idProveedor);

                if (proveedor == null || proveedor.IdsProductosSuministrados == null || !proveedor.IdsProductosSuministrados.Any())
                {
                    return new List<ENTITY.ProductoProveedor>();
                }

                // 2. Busca todos los productos disponibles 
                DataTable dtProductos = _productoBLL.TraerTodos(true);

                // 3. Filtra y mapea el DataTable solo para los IDs del proveedor
                List<ENTITY.ProductoProveedor> catalogo = dtProductos.AsEnumerable()
                    .Where(row => proveedor.IdsProductosSuministrados.Contains(Convert.ToInt32(row["Id"])))
                    .Select(row => new ENTITY.ProductoProveedor
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nombre = row["Nombre"].ToString(),                        
                        PrecioReferencia = Convert.ToDecimal(row["PrecioReferencia"])
                    }).ToList();

                return catalogo;
            }
            catch (Exception ex)
            {
                throw new Exception("BLL Error al intentar obtener el catálogo del proveedor: " + ex.Message);
            }
        }
    }
}