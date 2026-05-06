using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class ProductoProveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioReferencia { get; set; }
        public bool EstaActivo { get; set; }
        public bool Eliminado { get; set; }

        public ProductoProveedor()
        {
            EstaActivo = true;
            Eliminado = false;
            PrecioReferencia = 0;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
