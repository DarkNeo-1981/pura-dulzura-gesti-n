using System.Collections.Generic;
using System.Data;

namespace MAPPER
{
    public class PermisoMenuMapper
    {
        public List<ENTITY.Permiso_Menu> Buscar(int PermisoId)
        {
            DAL.Permiso_MenuDAL permiso_menu = new DAL.Permiso_MenuDAL();
            List<ENTITY.Permiso_Menu> l = new List<ENTITY.Permiso_Menu>();
            l = castDstoEnt(permiso_menu.Buscar_X_Permiso(PermisoId));
            if (l.Count > 0) return l;
            else return null;
        }

        public int AgregarUno(int pPId, int pMId)
        {
            DAL.Permiso_MenuDAL pm = new DAL.Permiso_MenuDAL();
            int resultado = pm.Agregar(pPId, pMId);
            return resultado;
        }

        public int BorrarUno(int pPId, int pMId)
        {
            DAL.Permiso_MenuDAL pm = new DAL.Permiso_MenuDAL();
            int resultado = pm.Borrar(pPId, pMId);
            return resultado;
        }
        
        public Dictionary<int, List<int>> ObtenerPadresHijos()
        {
            DAL.Permiso_MenuDAL dal = new DAL.Permiso_MenuDAL();
            return dal.ObtenerPadresHijos();
        }

        private List<ENTITY.Permiso_Menu> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Permiso_Menu> Lista_P = new List<ENTITY.Permiso_Menu>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Permiso_Menu p = new ENTITY.Permiso_Menu();
                    p.PermisoId = int.Parse(dr["PermisoId"].ToString());
                    p.MenuId = int.Parse(dr["MenuId"].ToString());
                    Lista_P.Add(p);
                }
            }
            return Lista_P;
        }
    }
}
