using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Encargado:EmpleadoBase
    {
        public int Id { get; set; }
        public int DNI_Supervisor { get; set; }
    }
}
