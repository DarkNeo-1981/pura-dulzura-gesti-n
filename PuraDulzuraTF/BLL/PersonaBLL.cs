using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IPersona
    {
        string[] DevolverDatos(int pDni);
        int BorrarUno(int pDNI);
        int DeshacerBorrarUno(int pDNI);
                
        int AgregarUno(
            int pDNI,
            int pLegajo,
            string pNombre,
            string pApellido,
            string pSexo, 
            string pEmail,
            string pCUIL,
            decimal pSueldoBasico,
            DateTime pFechaIngreso,
            int pDNI_Superior,
            bool pEliminado
        );
        
        int ModificarUno(
            int pDNI,
            int pLegajo,
            string pNombre,
            string pApellido,
            string pSexo, 
            string pEmail,
            string pCUIL,
            decimal pSueldoBasico,
            DateTime pFechaIngreso,
            int pDNI_Superior,
            bool pEliminado
        );
    }
}
