using DAL;
using DAL.Interfaces;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq; 
using MAPPER; 

namespace BLL
{
    public class PagoOrdenBLL
    {
        private readonly IPagoOrdenDAL _pagoOrdenDAL;
        private readonly IOrdenCompraDAL _ordenCompraDAL;        
        private readonly PagoOrdenMapper _pagoMapper;        

        public PagoOrdenBLL(IPagoOrdenDAL pagoOrdenDAL, IOrdenCompraDAL ordenCompraDAL)
        {
            _pagoOrdenDAL = pagoOrdenDAL;
            _ordenCompraDAL = ordenCompraDAL;
            _pagoMapper = new PagoOrdenMapper();            
        }

        // --- MÉTODOS PÚBLICOS ---

        // 1. Agrega un pago y actualiza el estado de la orden
        public int Agregar(ENTITY.PagoOrden pago)
        {
            // Validación de Negocio
            if (pago.Monto <= 0)
            {
                throw new Exception("El monto del pago debe ser un valor positivo.");
            }
            if (pago.IdOrden <= 0)
            {
                throw new Exception("El pago debe estar asociado a una Orden de Compra válida.");
            }

            // Mapear la ENTITY a XElement antes de llamar a DAL
            XElement pagoElement = _pagoMapper.EntidadToXml(pago);

            // 1. Persistir el Pago - La DAL asignará el ID y lo devolverá.
            int nuevoIdPago = _pagoOrdenDAL.Agregar(pagoElement);

            // 2. Actualizar el Estado de Pago de la Orden
            ActualizarEstadoPago(pago.IdOrden);

            return nuevoIdPago;
        }

        // 2. Traer el historial de pagos de una orden
        public List<ENTITY.PagoOrden> TraerPagosPorOrdenId(int idOrden)
        {
            // Recibir IQueryable<XElement> y Mapear a List<ENTITY.PagoOrden>
            IQueryable<XElement> pagosXml = _pagoOrdenDAL.TraerPagosPorOrdenId(idOrden);

            // Linq para mapear cada XElement a una entidad PagoOrden y convertir a List
            return pagosXml.Select(xml => _pagoMapper.XmlToEntidad(xml)).ToList();
        }

        // 3. Obtener Monto Total Pagado (Solo delega)
        public decimal ObtenerMontoTotalPagado(int idOrden)
        {
            return _pagoOrdenDAL.ObtenerMontoTotalPagado(idOrden);
        }

        // --- LÓGICA DE ACTUALIZACIÓN PRIVADA ---

        private void ActualizarEstadoPago(int idOrden)
        {
            // Obtener la orden (para saber el Total)
            // Llamar a TraerElementoPorId y mapear el XElement a OrdenCompra
            XElement ordenElement = _ordenCompraDAL.TraerElementoPorId(idOrden);

            if (ordenElement == null)
            {
                throw new KeyNotFoundException($"Orden de Compra ID {idOrden} no encontrada para verificar el estado de pago.");
            }
                        
            ENTITY.OrdenCompra orden = OrdenCompraMapper.MapearXElementAEntidad(ordenElement);

            // Obtener la suma de pagos realizados
            decimal totalPagado = _pagoOrdenDAL.ObtenerMontoTotalPagado(idOrden);

            string nuevoEstadoPago;

            if (totalPagado >= orden.Total)
            {
                nuevoEstadoPago = "PAGADO TOTAL";
            }
            else if (totalPagado > 0)
            {
                nuevoEstadoPago = "PAGO PARCIAL";
            }
            else
            {
                nuevoEstadoPago = "PENDIENTE DE PAGO";
            }

            // Actualizar el estado en la base de datos (XML)
            _ordenCompraDAL.ActualizarEstadoPago(idOrden, nuevoEstadoPago);
        }
    }
}