using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public abstract class OpcionesDeMenu
    {
        public int Id { get; set; }
        public string Detalle { get; set; }
        public int IdPadre { get; set; } 
        public abstract IList<OpcionesDeMenu> Hijos { get; }
        public abstract void AgregarHijo(OpcionesDeMenu opcion);
        public abstract void VaciarHijos();

        public override string ToString()
        {
            return Detalle;
        }
    }

    public class Compuesto : OpcionesDeMenu
    {
        private IList<OpcionesDeMenu> _hijos;

        public Compuesto()
        {
            _hijos = new List<OpcionesDeMenu>();
        }
        public override IList<OpcionesDeMenu> Hijos
        {
            get { return _hijos; }
        }
        public override void AgregarHijo(OpcionesDeMenu opcion)
        {            
            if (!_hijos.Any(x => x.Id == opcion.Id))
            {
                _hijos.Add(opcion);
            }
        }
        public override void VaciarHijos()
        {
            _hijos = new List<OpcionesDeMenu>();
        }
    }

    public class Patente : OpcionesDeMenu
    {        
        public override IList<OpcionesDeMenu> Hijos
        {
            get { return new List<OpcionesDeMenu>(); }
        }
        
        public override void AgregarHijo(OpcionesDeMenu opcion)
        {            
        }
        
        public override void VaciarHijos()
        {            
        }
    }
}
