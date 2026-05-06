using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using ENTITY;

namespace DAL
{
    public class JefeDAL : EmpleadoBaseDAL<Jefe>
    {
        protected override string NombreElementoRaiz => "Jefes";
        protected override string NombreElementoSingular => "Jefe";

        public JefeDAL() : base("Jefes.xml")
        {
        }

        protected override Jefe MapearXElementAEmpleado(XElement xElement)
        {
            Jefe jefe = new Jefe();

            jefe.DNI = (int)xElement.Attribute("DNI");
            jefe.Nombre = (string)xElement.Element("Nombre");
            jefe.Apellido = (string)xElement.Element("Apellido");
            jefe.Email = (string)xElement.Element("Email");
            if (xElement.Element("Sexo") != null) jefe.Sexo = (string)xElement.Element("Sexo");
            jefe.SueldoBasico = decimal.Parse((string)xElement.Element("SueldoBasico"), System.Globalization.CultureInfo.InvariantCulture);
            jefe.FechaIngreso = DateTime.Parse((string)xElement.Element("FechaIngreso"));
            jefe.Eliminado = bool.Parse((string)xElement.Element("Eliminado"));
            if (xElement.Element("Legajo") != null)
            {
                jefe.Legajo = int.Parse((string)xElement.Element("Legajo"));
            }
            if (xElement.Element("CUIL") != null)
            {
                jefe.CUIL = (string)xElement.Element("CUIL");
            }
            // Mapear DNI_Supervisor 
            if (xElement.Element("DNI_Supervisor") != null)
            {
                jefe.DNI_Supervisor = int.Parse((string)xElement.Element("DNI_Supervisor"));
            }
            return jefe;
        }

        // -----------------------------------------------------------------
        // MÉTODOS ABM
        // -----------------------------------------------------------------

        public int Agregar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var dniExistente = xmlDoc.Descendants(NombreElementoSingular).Any(x => (int)x.Attribute("DNI") == pDNI);

                if (!dniExistente)
                {
                    xmlDoc.Element(NombreElementoRaiz).Add(new XElement(NombreElementoSingular,
                                new XAttribute("DNI", pDNI),
                                new XElement("Legajo", pLegajo),
                                new XElement("Nombre", pNombre),
                                new XElement("Apellido", pApellido),
                                new XElement("Sexo", pSexo),
                                new XElement("Email", pEmail),
                                new XElement("CUIL", pCUIL),
                                new XElement("SueldoBasico", pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                                new XElement("FechaIngreso", pFechaIngreso.ToString("yyyy-MM-dd")),
                                new XElement("DNI_Supervisor", pDNI_Superior), // Se guarda el superior (podría ser 0 o -1)
                                new XElement("Eliminado", pEliminado.ToString())
                            ));
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Modificar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var jefe = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(j => (int)j.Attribute("DNI") == pDNI);

                if (jefe != null)
                {
                    jefe.Element("Legajo").Value = pLegajo.ToString();
                    jefe.Element("Nombre").Value = pNombre;
                    jefe.Element("Apellido").Value = pApellido;
                    if (jefe.Element("Sexo") == null) jefe.Add(new XElement("Sexo"));
                    jefe.Element("Sexo").Value = pSexo;
                    jefe.Element("Email").Value = pEmail;                    
                    if (jefe.Element("DNI_Supervisor") == null) jefe.Add(new XElement("DNI_Supervisor"));
                    jefe.Element("DNI_Supervisor").Value = pDNI_Superior.ToString();
                    if (jefe.Element("CUIL") == null) jefe.Add(new XElement("CUIL"));
                    jefe.Element("CUIL").Value = pCUIL;
                    if (jefe.Element("SueldoBasico") == null) jefe.Add(new XElement("SueldoBasico"));
                    jefe.Element("SueldoBasico").Value = pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    if (jefe.Element("FechaIngreso") == null) jefe.Add(new XElement("FechaIngreso"));
                    jefe.Element("FechaIngreso").Value = pFechaIngreso.ToString("yyyy-MM-dd");
                    if (jefe.Element("Eliminado") == null) jefe.Add(new XElement("Eliminado"));
                    jefe.Element("Eliminado").Value = pEliminado.ToString();
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var jefe = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(j => (int)j.Attribute("DNI") == pDNI);

                if (jefe != null)
                {
                    if (jefe.Element("Eliminado") == null) jefe.Add(new XElement("Eliminado", "True"));
                    else jefe.Element("Eliminado").Value = "True";

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Deshacer_Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var jefe = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(j => (int)j.Attribute("DNI") == pDNI);

                if (jefe != null)
                {
                    if (jefe.Element("Eliminado") == null) jefe.Add(new XElement("Eliminado", "False"));
                    else jefe.Element("Eliminado").Value = "False";

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // -----------------------------------------------------------------
        // MÉTODOS DE CONSULTA DATATABLE (CORREGIDOS PARA EL DGV)
        // -----------------------------------------------------------------

        public DataTable Buscar_Uno(int pDNI)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DNI");
            dt.Columns.Add("Legajo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Apellido");            
            dt.Columns.Add("Sexo");
            dt.Columns.Add("Email");
            dt.Columns.Add("CUIL");
            dt.Columns.Add("SueldoBasico");
            dt.Columns.Add("FechaIngreso");
            dt.Columns.Add("Superior");
            dt.Columns.Add("Cargo");
            dt.Columns.Add("Eliminado");

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var jefe = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(j => (int)j.Attribute("DNI") == pDNI);

                if (jefe != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)jefe.Attribute("DNI");
                    dr["Legajo"] = (string)jefe.Element("Legajo");
                    dr["Nombre"] = (string)jefe.Element("Nombre");
                    dr["Apellido"] = (string)jefe.Element("Apellido");                    
                    dr["Sexo"] = (string)jefe.Element("Sexo");
                    dr["Email"] = (string)jefe.Element("Email");
                    dr["CUIL"] = (string)jefe.Element("CUIL");
                    dr["SueldoBasico"] = (string)jefe.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)jefe.Element("FechaIngreso");                   
                    dr["Superior"] = (string)jefe.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Jefe"
                    dr["Eliminado"] = (string)jefe.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public DataTable Buscar_Todos(bool pEliminado)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DNI");
            dt.Columns.Add("Legajo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Apellido");           
            dt.Columns.Add("Sexo");
            dt.Columns.Add("Email");
            dt.Columns.Add("CUIL");
            dt.Columns.Add("SueldoBasico");
            dt.Columns.Add("FechaIngreso");
            dt.Columns.Add("Superior");
            dt.Columns.Add("Cargo");
            dt.Columns.Add("Eliminado");

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                IEnumerable<XElement> consulta;

                if (pEliminado)
                {
                    consulta = xmlDoc.Descendants(NombreElementoSingular);
                }
                else
                {
                    consulta = xmlDoc.Descendants(NombreElementoSingular).Where(j => (string)j.Element("Eliminado") == "False");
                }

                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)xElement.Attribute("DNI");
                    dr["Legajo"] = (string)xElement.Element("Legajo");
                    dr["Nombre"] = (string)xElement.Element("Nombre");
                    dr["Apellido"] = (string)xElement.Element("Apellido");                    
                    dr["Sexo"] = (string)xElement.Element("Sexo");
                    dr["Email"] = (string)xElement.Element("Email");
                    dr["CUIL"] = (string)xElement.Element("CUIL");
                    dr["SueldoBasico"] = (string)xElement.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)xElement.Element("FechaIngreso");                   
                    dr["Superior"] = (string)xElement.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Jefe"
                    dr["Eliminado"] = (string)xElement.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public int CantidadJefes()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                return xmlDoc.Descendants(NombreElementoSingular).Count();
            }
            catch (Exception) { return 0; }
        }
    }
}