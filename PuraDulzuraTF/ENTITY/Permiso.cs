using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{    
    public class Permiso : OpcionesDeMenu
    {
        // El Id y Detalle ya los hereda de OpcionesDeMenu
        // Detalle de OpcionesDeMenu va a ser la Descripción de permiso.

        public Permiso()
        {            
        }

        public override IList<OpcionesDeMenu> Hijos
        {
            get { return new List<OpcionesDeMenu>(); } // Un Permiso no tiene hijos directamente 
        }

        public override void AgregarHijo(OpcionesDeMenu opcion)
        {
            // Un Permiso no agrega hijos directamente
        }

        public override void VaciarHijos()
        {
            // Un Permiso no tiene hijos que vaciar
        }
    }
}
