using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENTITY
{
    public class OrdenCompra
    {        
        public int Id { get; set; }
        public DateTime FechaEmision { get; set; }        
        public Proveedores Proveedor { get; set; }
        public string Estado { get; set; } 
        public string EstadoPago { get; set; }     
        public List<DetalleOrdenCompra> Detalles { get; set; }
        public decimal Total
        {
            get
            {
                if (Detalles == null) return 0;
                decimal total = 0;
                foreach (var item in Detalles)
                {
                    total += item.Subtotal;
                }
                return total;               
            }
        }

        // Constructor para inicializar la lista
        public OrdenCompra()
        {
            Detalles = new List<DetalleOrdenCompra>();
            Proveedor = new Proveedores();
            FechaEmision = DateTime.Now;
            Estado = "Pendiente"; // Valor por defecto
        }
    }
}
