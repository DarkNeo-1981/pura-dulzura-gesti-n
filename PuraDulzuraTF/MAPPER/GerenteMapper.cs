using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ENTITY;
using DAL;

namespace MAPPER
{
    public class GerenteMapper
    {
        public int CantidadGerentes()
        {
            DAL.GerenteDAL j = new DAL.GerenteDAL();
            int resultado = 0;
            resultado = j.CantidadGerentes();
            return resultado;
        }

        public ENTITY.Gerente Buscar(int pDNI)
        {
            DAL.GerenteDAL Gerente = new DAL.GerenteDAL();
            List<ENTITY.Gerente> l = new List<ENTITY.Gerente>();
            l = castDstoEnt(Gerente.Buscar_Uno(pDNI));
            if (l != null && l.Count > 0) return l[0];
            else return null;
        }

        public List<ENTITY.Gerente> BuscarTodos(bool pEliminado)
        {
            DAL.GerenteDAL Gerente = new DAL.GerenteDAL();
            List<ENTITY.Gerente> l = new List<ENTITY.Gerente>();
            l = castDstoEnt(Gerente.Buscar_Todos(pEliminado));
            if (l != null && l.Count > 0) return l;
            else return new List<ENTITY.Gerente>(); // Devolver lista vacía en lugar de null
        }

        public int BorrarUno(int pDNI)
        {
            DAL.GerenteDAL Gerente = new DAL.GerenteDAL();
            int resultado = Gerente.Borrar(pDNI);
            return resultado;
        }

        public int DeshacerBorrarUno(int pDNI)
        {
            DAL.GerenteDAL Gerente = new DAL.GerenteDAL();
            int resultado = Gerente.Deshacer_Borrar(pDNI);
            return resultado;
        }
        
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.GerenteDAL Gerente = new DAL.GerenteDAL();
            
            int resultado = Gerente.Agregar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);

            return resultado;
        }
        
        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.GerenteDAL Gerente = new DAL.GerenteDAL();
            
            int resultado = Gerente.Modificar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);

            return resultado;
        }
        
        private List<ENTITY.Gerente> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Gerente> l = new List<ENTITY.Gerente>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Gerente ent = new ENTITY.Gerente();
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
