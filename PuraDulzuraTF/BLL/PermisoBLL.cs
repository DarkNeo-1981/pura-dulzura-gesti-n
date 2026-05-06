using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PermisoBLL
    {
        public ENTITY.Permiso BuscarUno(int pId)
        {
            MAPPER.PermisosMapper map = new MAPPER.PermisosMapper();
            ENTITY.Permiso ent = map.Buscar(pId);
            map = null;
            return ent;
        }
        public List<ENTITY.Permiso> BuscarTodos()
        {
            MAPPER.PermisosMapper map = new MAPPER.PermisosMapper();
            List<ENTITY.Permiso> l = map.BuscarTodos();
            map = null;
            return l;
        }
        public int Agregar(string pDescripcion)
        {
            MAPPER.PermisosMapper map = new MAPPER.PermisosMapper();
            int resultado = map.Agregar(pDescripcion);
            map = null;
            return resultado;
        }
        public int Borrar(int pId)
        {
            MAPPER.PermisosMapper map = new MAPPER.PermisosMapper();
            int resultado = map.Borrar(pId);
            map = null;
            return resultado;
        }
    }
}
