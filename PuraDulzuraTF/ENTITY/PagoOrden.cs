using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class PagoOrden
    {
        public int IdPago { get; set; }
        public int IdOrden { get; set; }
        public OrdenCompra OrdenCompra { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; }
        public string Comprobante { get; set; }
        public PagoOrden()
        {
            FechaPago = DateTime.Now;
        }
    }
}
