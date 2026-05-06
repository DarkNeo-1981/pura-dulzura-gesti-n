using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Proveedores
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string CondicionIVA { get; set; }
        public DateTime FechaAlta { get; set; }        
        public List<int> IdsProductosSuministrados { get; set; }
        public bool EstaActivo { get; set; }
        public bool Eliminado { get; set; }

        public Proveedores()
        {            
            IdsProductosSuministrados = new List<int>();
            EstaActivo = true;
            FechaAlta = DateTime.Now;
        }
    }
}
