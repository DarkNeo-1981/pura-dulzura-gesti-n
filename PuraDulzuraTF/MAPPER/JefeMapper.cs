using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class JefeMapper
    {
        public int CantidadJefes()
        {
            DAL.JefeDAL j = new DAL.JefeDAL();
            int resultado = 0;
            resultado = j.CantidadJefes();
            return resultado;
        }
        public ENTITY.Jefe Buscar(int pDNI)
        {
            DAL.JefeDAL jefe = new DAL.JefeDAL();
            List<ENTITY.Jefe> l = new List<ENTITY.Jefe>();
            l = castDstoEnt(jefe.Buscar_Uno(pDNI));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public List<ENTITY.Jefe> BuscarTodos(bool pEliminado)
        {
            DAL.JefeDAL jefe = new DAL.JefeDAL();
            List<ENTITY.Jefe> l = new List<ENTITY.Jefe>();
            l = castDstoEnt(jefe.Buscar_Todos(pEliminado));
            if (l.Count > 0) return l;
            else return null;
        }
        public int BorrarUno(int pDNI)
        {
            DAL.JefeDAL jefe = new DAL.JefeDAL();
            int resultado = jefe.Borrar(pDNI);
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            DAL.JefeDAL jefe = new DAL.JefeDAL();
            int resultado = jefe.Deshacer_Borrar(pDNI);
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.JefeDAL jefe = new DAL.JefeDAL();            
            int resultado = jefe.Agregar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.JefeDAL jefe = new DAL.JefeDAL();
            int resultado = jefe.Modificar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            return resultado;
        }

        private List<ENTITY.Jefe> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Jefe> l = new List<ENTITY.Jefe>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Jefe ent = new ENTITY.Jefe();
                    ent.DNI = int.Parse(dr["DNI"].ToString());
                    ent.Legajo = int.Parse(dr["Legajo"].ToString());
                    ent.Nombre = dr["Nombre"].ToString();
                    ent.Apellido = dr["Apellido"].ToString();
                    ent.Sexo = dr["Sexo"].ToString(); 
                    ent.Email = dr["Email"].ToString(); 
                    ent.CUIL = dr["CUIL"].ToString(); 

                    // Conversiones seguras
                    decimal sueldo;
                    if (decimal.TryParse(dr["SueldoBasico"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out sueldo))
                    {
                        ent.SueldoBasico = sueldo;
                    }

                    DateTime fecha;
                    if (DateTime.TryParse(dr["FechaIngreso"].ToString(), out fecha))
                    {
                        ent.FechaIngreso = fecha;
                    }

                    if (dt.Columns.Contains("Superior") && dr["Superior"] != DBNull.Value)
                    {
                        int supervisorDNI;
                        if (int.TryParse(dr["Superior"].ToString(), out supervisorDNI))
                        {
                            ent.DNI_Supervisor = supervisorDNI;
                        }
                    }

                    ent.Eliminado = bool.Parse(dr["Eliminado"].ToString());
                    l.Add(ent);                    
                }
            }
            return l;
        }
    }
}
