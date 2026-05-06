using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class OrdenDePedido
    {
        public int Id { get; set; }
        public int DNI_Vendedor { get; set; }
        public int DNI_Cliente { get; set; }           
        public string FechaDeVenta { get; set; }
        public decimal Total { get; set; }             
        public bool Eliminado { get; set; }
        public bool Facturada { get; set; }
        public bool Cobrada { get; set; }
        public List<DetalleOrdenDePedido> Detalles { get; set; }  
    }

    public class DetalleOrdenDePedido
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
