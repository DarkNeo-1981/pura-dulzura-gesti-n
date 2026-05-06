using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Producto
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Clasificacion { get; set; }
        public int Porciones { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioDeVenta { get; set; }
        public bool ProductoActivo { get; set; }
    }
}
