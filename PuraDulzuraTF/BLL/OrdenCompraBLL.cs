using DAL; 
using DAL.Interfaces; 
using ENTITY;
using MAPPER;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace BLL
{
    public class OrdenCompraBLL
    {        
        private readonly IOrdenCompraDAL _dal;
        private readonly ProveedoresBLL _proveedoresBLL; 
        
        public OrdenCompraBLL(IOrdenCompraDAL ordenCompraDAL)
        {
            _dal = ordenCompraDAL; // Recibe la DAL a través del constructor.            
            _proveedoresBLL = new ProveedoresBLL();
        }

        public OrdenCompraBLL() : this(new OrdenCompraDAL()) 
        {
            // Esta línea llama al Constructor 1 y le pasa la implementación concreta.           
        }

        // --- MÉTODOS CRUD ---

        // 1. Agregar / Crear una nueva Orden de Compra
        public int Agregar(OrdenCompra orden)
        {
            // --- Reglas de Negocio ---
            if (orden.Detalles == null || orden.Detalles.Count == 0)
            {
                throw new Exception("La Orden de Compra debe contener al menos un producto.");
            }

            // Validar si el proveedor existe y está activo
            Proveedores proveedor = _proveedoresBLL.TraerPorId(orden.Proveedor.Id);
            if (proveedor == null || !proveedor.EstaActivo)
            {
                throw new Exception("No se puede emitir una Orden de Compra a un proveedor inactivo o inexistente.");
            }
            orden.Proveedor = proveedor;

            // 1. Mapeo: Convertir la Entidad a XElement
            XElement ordenElement = OrdenCompraMapper.MapearEntidadAXElement(orden);

            // 2. Persistencia: Llamar a la DAL. La DAL devuelve el nuevo ID.
            return _dal.Agregar(ordenElement);
        }

        // 2. Modificar una Orden de Compra                
        public int Modificar(OrdenCompra orden)
        {
            // 1. Mapeo: Convertir la Entidad a XElement
            XElement ordenElement = OrdenCompraMapper.MapearEntidadAXElement(orden);

            // 2. Persistencia: Llamamos a la DAL. 
            _dal.Modificar(orden.Id, ordenElement);

            return orden.Id; 
        }

        // 3. Traer una Orden de Compra por ID
        public OrdenCompra TraerOrdenPorId(int id)
        {
            // 1. Persistencia: Traemos el XElement desde la DAL
            XElement ordenElement = _dal.TraerElementoPorId(id);
            if (ordenElement == null) return null;

            // 2. Mapeo: Convertimos el XElement a Entidad
            OrdenCompra orden = OrdenCompraMapper.MapearXElementAEntidad(ordenElement);

            // 3. Complementar la entidad (Carga perezosa del proveedor)
            if (orden.Proveedor != null && orden.Proveedor.Id != 0)
            {
                orden.Proveedor = _proveedoresBLL.TraerPorId(orden.Proveedor.Id);
            }

            return orden;
        }

        // 4. Traer todas las Órdenes de Compra (para la grilla principal)
        public DataTable TraerTodasOrdenes()
        {
            // El resultado final que se va a mostrar
            DataTable dt = new DataTable();

            // Definición de las columnas de la grilla (datos de presentación)
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("FechaEmision", typeof(DateTime));
            dt.Columns.Add("RazonSocialProveedor", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            dt.Columns.Add("EstadoPago", typeof(string));
            dt.Columns.Add("Total", typeof(decimal));

            // 1. Persistencia: Se traen todos los XElement desde la DAL
            IQueryable<XElement> elementos = _dal.TraerTodosElementos();

            foreach (var ordenElement in elementos)
            {
                // 2. Mapeo: Convertir a Entidad para acceder a las propiedades calculadas (Total)
                OrdenCompra orden = OrdenCompraMapper.MapearXElementAEntidad(ordenElement);

                // 3. Carga de datos complementarios del Proveedor
                Proveedores proveedor = _proveedoresBLL.TraerPorId(orden.Proveedor.Id);

                // 4. Carga del DataRow
                DataRow dr = dt.NewRow();
                dr["Id"] = orden.Id;
                dr["FechaEmision"] = orden.FechaEmision;
                dr["RazonSocialProveedor"] = proveedor != null ? proveedor.RazonSocial : "Proveedor Desconocido";
                dr["Estado"] = orden.Estado;
                dr["EstadoPago"] = orden.EstadoPago;
                dr["Total"] = orden.Total;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        // 5. Cambiar el estado (ej. de Pendiente a Recibida o Cancelada)
        public int CambiarEstadoOrden(int idOrden, string nuevoEstado)
        {
            // 1. Persistencia: Se llama a la DAL y devuelve 1 o 0.
            return _dal.CambiarEstado(idOrden, nuevoEstado);
        }

        public bool PagarOrden(int idOrden)
        {
            const string ESTADO_PAGADO = "PAGADA";
            try
            {
                // 1. Llama directamente a la DAL para actualizar la propiedad EstadoPago
                string estadoActual = _dal.ObtenerEstadoPagoActual(idOrden);
                if (estadoActual.ToUpper() == "PAGADA")
                {                    
                    return true;
                }
                if (estadoActual.ToUpper() == "CANCELADA" || estadoActual.ToUpper() == "DEVOLUCION")
                {
                    // RECHAZAR EL CAMBIO
                    throw new InvalidOperationException($"No se puede pagar la Orden {idOrden} porque su estado es {estadoActual}.");
                }
                return ActualizarEstadoPago(idOrden, ESTADO_PAGADO);
            }
            catch (KeyNotFoundException)
            {
                // La orden no existe
                return false;
            }
            catch (Exception ex)
            {
                // Propaga el error de la DAL si es necesario
                throw new Exception($"Error BLL al intentar pagar la orden {idOrden}.", ex);
            }
        }

        public bool ActualizarEstadoPago(int idOrden, string nuevoEstado)
        {
            try
            {                
                _dal.ActualizarEstadoPago(idOrden, nuevoEstado.ToUpper());
                return true;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error BLL al intentar actualizar el estado de pago de la orden {idOrden} a {nuevoEstado}.", ex);
            }
        }
    }
}