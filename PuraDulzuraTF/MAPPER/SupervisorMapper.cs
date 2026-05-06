using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class SupervisorMapper
    {
        public int CantidadSupervisores()
        {
            DAL.SupervisorDAL s = new DAL.SupervisorDAL();
            int resultado = 0;
            resultado = s.CantidadSupervisores();
            return resultado;
        }
        public ENTITY.Supervisor Buscar(int pDNI)
        {
            DAL.SupervisorDAL sup = new DAL.SupervisorDAL();
            List<ENTITY.Supervisor> l = new List<ENTITY.Supervisor>();
            l = castDstoEnt(sup.Buscar_Uno(pDNI));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public List<ENTITY.Supervisor> BuscarTodos(bool pEliminado)
        {
            DAL.SupervisorDAL sup = new DAL.SupervisorDAL();
            List<ENTITY.Supervisor> l = new List<ENTITY.Supervisor>();
            l = castDstoEnt(sup.Buscar_Todos(pEliminado));
            if (l.Count > 0) return l;
            else return null;
        }
        public int BorrarUno(int pDNI)
        {
            DAL.SupervisorDAL sup = new DAL.SupervisorDAL();
            int resultado = sup.Borrar(pDNI);
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            DAL.SupervisorDAL sup = new DAL.SupervisorDAL();
            int resultado = sup.Deshacer_Borrar(pDNI);
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.SupervisorDAL sup = new DAL.SupervisorDAL();
            int resultado = sup.Agregar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.SupervisorDAL sup = new DAL.SupervisorDAL();
            int resultado = sup.Modificar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);

            return resultado;
        }
        private List<ENTITY.Supervisor> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Supervisor> l = new List<ENTITY.Supervisor>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Supervisor ent = new ENTITY.Supervisor();
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
