using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class PermisoCompuesto:Permiso
    {
        private List<Permiso> hijos = new List<Permiso>();

        public override void AgregarHijo(Permiso permiso)
        {
            hijos.Add(permiso);
        }

        public override List<Permiso> ObtenerHijos()
        {
            return hijos;
        }
    }
}
