using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class ClasificacionProducto
    {
        public int Id { get; set; }
        public string Detalle { get; set; }
        public int Porciones { get; set; }
        public decimal Costo { get; set; }
        public bool Eliminado { get; set; }
    }
}
