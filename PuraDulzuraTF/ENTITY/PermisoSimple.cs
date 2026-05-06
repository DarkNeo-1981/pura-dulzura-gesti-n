using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class PermisoSimple:Permiso
    {
        public override void AgregarHijo(Permiso permiso)
        {
            throw new NotSupportedException("Un permiso simple no puede tener hijos.");
        }

        public override List<Permiso> ObtenerHijos()
        {
            return new List<Permiso>(); // sin hijos
        }   
    }
}
