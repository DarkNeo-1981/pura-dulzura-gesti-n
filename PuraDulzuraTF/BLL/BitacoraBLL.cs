using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraBLL
    {
        public List<ENTITY.Bitacora> BuscarTodos()
        {
            MAPPER.BitacoraMapper map = new MAPPER.BitacoraMapper();
            List<ENTITY.Bitacora> l = map.BuscarTodos();
            map = null;
            return l;
        }
        public List<ENTITY.Bitacora> Buscar_BackUps()
        {
            MAPPER.BitacoraMapper map = new MAPPER.BitacoraMapper();
            List<ENTITY.Bitacora> l = map.Buscar_BackUps();
            map = null;
            return l;
        }
        public int AgregarUno(string pOperador, string pDetalle)
        {
            MAPPER.BitacoraMapper map = new MAPPER.BitacoraMapper();
            int resultado = map.AgregarUno(pOperador, pDetalle);
            map = null;
            return resultado;
        }
        public int AgregarUnoSinOperador(string pDetalle)
        {
            MAPPER.BitacoraMapper map = new MAPPER.BitacoraMapper();
            int resultado = map.AgregarUnoSinOperador(pDetalle);
            map = null;
            return resultado;
        }
    }
}
