using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{  

    public class GerenteBLL : IPersona
    {
        public int CantidadGerentes()
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();
            int resultado = 0;
            resultado = map.CantidadGerentes();
            map = null;
            return resultado;
        }

        public ENTITY.Gerente BuscarUno(int pDNI)
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();
            ENTITY.Gerente ent = map.Buscar(pDNI);
            map = null;
            return ent;
        }

        public List<ENTITY.Gerente> BuscarTodos(bool pEliminado)
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();
            List<ENTITY.Gerente> l = map.BuscarTodos(pEliminado);
            map = null;
            return l;
        }

        public string[] DevolverDatos(int pDNI)
        {
            ENTITY.Gerente g = this.BuscarUno(pDNI);            
            string[] datos = 
                            { g.DNI.ToString(), 
                              g.Legajo.ToString(), 
                              g.Nombre.ToString(), 
                              g.Apellido.ToString(),
                              g.Sexo.ToString(),
                              g.Email.ToString(),
                              g.CUIL.ToString(),
                              g.SueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture),
                              g.FechaIngreso.ToString("yyyy-MM-dd"),
                              "0",
                              "Gerente",
                              g.Eliminado.ToString()
                            };
            return datos;
        }

        public int BorrarUno(int pDNI)
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();
            int resultado = map.BorrarUno(pDNI);
            map = null;
            return resultado;
        }

        public int DeshacerBorrarUno(int pDNI)
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();
            int resultado = map.DeshacerBorrarUno(pDNI);
            map = null;
            return resultado;
        }
       
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();
            int resultado = map.AgregarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }
        
        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.GerenteMapper map = new MAPPER.GerenteMapper();  
            int resultado = map.ModificarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);

            map = null;
            return resultado;
        }
    }
}
