using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class AdministrativoMapper
    {
        public int CantidadAdministrativos()
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            int resultado = 0;
            resultado = adm.CantidadAdministrativos();
            return resultado;
        }
        public ENTITY.Administrativo Buscar(int pDNI)
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            List<ENTITY.Administrativo> l = new List<ENTITY.Administrativo>();
            l = castDstoEnt(adm.Buscar_Uno(pDNI));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public List<ENTITY.Administrativo> BuscarTodos(bool pEliminado)
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            List<ENTITY.Administrativo> l = new List<ENTITY.Administrativo>();
            l = castDstoEnt(adm.Buscar_Todos(pEliminado));
            if (l.Count > 0) return l;
            else return null;
        }
        public int BorrarUno(int pDNI)
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            int resultado = adm.Borrar(pDNI);
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            int resultado = adm.Deshacer_Borrar(pDNI);
            return resultado;
        }

        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Supervisor, bool pEliminado)
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            int resultado = adm.Agregar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Supervisor, pEliminado);
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Supervisor, bool pEliminado)
        {
            DAL.AdministrativoDAL adm = new DAL.AdministrativoDAL();
            int resultado = adm.Modificar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Supervisor, pEliminado);
            return resultado;
        }

        private List<ENTITY.Administrativo> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Administrativo> l = new List<ENTITY.Administrativo>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Administrativo ent = new ENTITY.Administrativo();
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
