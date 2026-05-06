using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class BitacoraMapper
    {
        public List<ENTITY.Bitacora> BuscarTodos()
        {
            DAL.BitacoraDAL b = new DAL.BitacoraDAL();
            List<ENTITY.Bitacora> l = new List<ENTITY.Bitacora>();
            l = castDstoEnt(b.Buscar_Todos());
            if (l.Count > 0) return l;
            else return null;
        }
        public int AgregarUno(string pOperador, string pDetalle)
        {
            DAL.BitacoraDAL b = new DAL.BitacoraDAL();
            int resultado = b.Agregar(pOperador, pDetalle);
            return resultado;
        }
        public int AgregarUnoSinOperador(string pDetalle)
        {
            DAL.BitacoraDAL b = new DAL.BitacoraDAL();
            int resultado = b.Agregar(pDetalle);
            return resultado;
        }
        public List<ENTITY.Bitacora> Buscar_BackUps()
        {
            DAL.BitacoraDAL b = new DAL.BitacoraDAL();
            List<ENTITY.Bitacora> l = new List<ENTITY.Bitacora>();
            l = castDstoEnt(b.Buscar_BackUps());
            if (l.Count > 0) return l;
            else return null;
        }
        private List<ENTITY.Bitacora> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Bitacora> l = new List<ENTITY.Bitacora>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Bitacora ent = new ENTITY.Bitacora();
                    ent.Id = int.Parse(dr["Id"].ToString());
                    ent.Usuario = dr["Usuario"].ToString();
                    ent.Fecha = DateTime.Parse(dr["Fecha"].ToString());
                    ent.Detalle = dr["Detalle"].ToString();
                    l.Add(ent);
                }
            }
            return l;
        }
    }
}
