using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITY;

namespace MAPPER
{
    public class PermisosMapper
    {
        public Permiso Buscar(int pId)
        {
            DAL.PermisosDAL permiso = new DAL.PermisosDAL();
            List<Permiso> l = new List<Permiso>();
            l = castDstoEnt(permiso.Buscar_Uno(pId));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public List<Permiso> BuscarTodos()
        {
            DAL.PermisosDAL permiso = new DAL.PermisosDAL();
            List<Permiso> l = new List<Permiso>();
            l = castDstoEnt(permiso.Buscar_Todos());
            if (l.Count > 0) return l;
            else return null;
        }
        public int Agregar(string pDescripcion)
        {
            DAL.PermisosDAL permiso = new DAL.PermisosDAL();
            int resultado = permiso.Agregar(pDescripcion);
            return resultado;
        }
        public int Borrar(int pId)
        {
            DAL.PermisosDAL permiso = new DAL.PermisosDAL();
            int resultado = permiso.Borrar(pId);
            return resultado;
        }
        private List<Permiso> castDstoEnt(DataTable dt)
        {
            List<Permiso> Lista_P = new List<Permiso>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Permiso p = new Permiso();
                    p.Id = int.Parse(dr["Id"].ToString());
                    p.Detalle = dr["Descripcion"].ToString();
                    Lista_P.Add(p);
                }
            }
            return Lista_P;
        }
    }
}
