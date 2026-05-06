using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using ENTITY; // Asume que la entidad Supervisor existe

namespace DAL
{
    // 1. Se hereda de la clase base genérica EmpleadoBaseDAL<T>
    public class SupervisorDAL : EmpleadoBaseDAL<Supervisor>
    {
        // 2. Usamos las propiedades abstractas de la clase base
        protected override string NombreElementoRaiz => "Supervisores";
        protected override string NombreElementoSingular => "Supervisor";

        // Mantenemos el constructor base, que se encarga de la ruta y la creación del XML inicial.
        public SupervisorDAL() : base("Supervisores.xml")
        {
            // El constructor base (EmpleadoBaseDAL) maneja la creación de la carpeta y archivo.
        }

        // 3. Implementación requerida de la clase base: Mapear XElement a Entidad
        protected override Supervisor MapearXElementAEmpleado(XElement xElement)
        {
            Supervisor supervisor = new Supervisor();

            // Mapeo de propiedades de EmpleadoBase
            supervisor.DNI = (int)xElement.Attribute("DNI");
            supervisor.Nombre = (string)xElement.Element("Nombre");
            supervisor.Apellido = (string)xElement.Element("Apellido");
            supervisor.Email = (string)xElement.Element("Email");

            // Conversiones seguras y obligatorias
            supervisor.SueldoBasico = decimal.Parse((string)xElement.Element("SueldoBasico"), System.Globalization.CultureInfo.InvariantCulture);
            supervisor.FechaIngreso = DateTime.Parse((string)xElement.Element("FechaIngreso"));
            supervisor.Eliminado = bool.Parse((string)xElement.Element("Eliminado"));

            // Mapeo de campos específicos (verificando existencia)
            if (xElement.Element("Legajo") != null)
            {
                supervisor.Legajo = int.Parse((string)xElement.Element("Legajo"));
            }
            if (xElement.Element("CUIL") != null)
            {
                supervisor.CUIL = (string)xElement.Element("CUIL");
            }
            // Mapeo de Sexo
            if (xElement.Element("Sexo") != null)
            {
                supervisor.Sexo = (string)xElement.Element("Sexo");
            }
            if (xElement.Element("DNI_Supervisor") != null)
            {                
                supervisor.DNI_Supervisor = int.Parse((string)xElement.Element("DNI_Supervisor"));
            }

            return supervisor;
        }

        // =================================================================
        // MÉTODOS ABM
        // =================================================================

        // Firma CORREGIDA: Incluye pSexo (11 argumentos)
        public int Agregar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo); // Usa rutaArchivo de la clase base                
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
                                new XElement("DNI_Supervisor", pDNI_Superior), // Mapea el superior genérico al campo DNI_Supervisor
                                new XElement("Eliminado", pEliminado.ToString())
                            ));
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // Firma CORREGIDA: Incluye pSexo (11 argumentos)
        public int Modificar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);                
                var supervisor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(s => (int)s.Attribute("DNI") == pDNI);

                if (supervisor != null)
                {
                    supervisor.Element("Legajo").Value = pLegajo.ToString();
                    supervisor.Element("Nombre").Value = pNombre;
                    supervisor.Element("Apellido").Value = pApellido;
                    if (supervisor.Element("Sexo") == null) supervisor.Add(new XElement("Sexo")); // Asegurar existencia
                    supervisor.Element("Sexo").Value = pSexo; 
                    supervisor.Element("Email").Value = pEmail;
                    if (supervisor.Element("DNI_Supervisor") == null) supervisor.Add(new XElement("DNI_Supervisor"));
                    supervisor.Element("DNI_Supervisor").Value = pDNI_Superior.ToString();
                    if (supervisor.Element("CUIL") == null) supervisor.Add(new XElement("CUIL"));
                    supervisor.Element("CUIL").Value = pCUIL;
                    if (supervisor.Element("SueldoBasico") == null) supervisor.Add(new XElement("SueldoBasico"));
                    supervisor.Element("SueldoBasico").Value = pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    if (supervisor.Element("FechaIngreso") == null) supervisor.Add(new XElement("FechaIngreso"));
                    supervisor.Element("FechaIngreso").Value = pFechaIngreso.ToString("yyyy-MM-dd");
                    if (supervisor.Element("Eliminado") == null) supervisor.Add(new XElement("Eliminado"));
                    supervisor.Element("Eliminado").Value = pEliminado.ToString();
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // Borrar y Deshacer_Borrar solo requieren cambiar el campo "Eliminado"
        public int Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var supervisor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(s => (int)s.Attribute("DNI") == pDNI);

                if (supervisor != null)
                {
                    // Aseguramos que el elemento exista antes de intentar establecer el valor
                    if (supervisor.Element("Eliminado") == null) supervisor.Add(new XElement("Eliminado", "True"));
                    else supervisor.Element("Eliminado").Value = "True";

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
                var supervisor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(s => (int)s.Attribute("DNI") == pDNI);

                if (supervisor != null)
                {
                    // Aseguramos que el elemento exista antes de intentar establecer el valor
                    if (supervisor.Element("Eliminado") == null) supervisor.Add(new XElement("Eliminado", "False"));
                    else supervisor.Element("Eliminado").Value = "False";

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // =================================================================
        // MÉTODOS DE CONSULTA DATATABLE (CORREGIDOS PARA EL DGV)
        // =================================================================

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
                var supervisor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(s => (int)s.Attribute("DNI") == pDNI);

                if (supervisor != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)supervisor.Attribute("DNI");
                    dr["Legajo"] = (string)supervisor.Element("Legajo");
                    dr["Nombre"] = (string)supervisor.Element("Nombre");
                    dr["Apellido"] = (string)supervisor.Element("Apellido");
                    dr["Sexo"] = (string)supervisor.Element("Sexo");
                    dr["Email"] = (string)supervisor.Element("Email");
                    dr["CUIL"] = (string)supervisor.Element("CUIL");
                    dr["SueldoBasico"] = (string)supervisor.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)supervisor.Element("FechaIngreso");
                    dr["Superior"] = (string)supervisor.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Supervisor"
                    dr["Eliminado"] = (string)supervisor.Element("Eliminado");
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
                    consulta = xmlDoc.Descendants(NombreElementoSingular).Where(s => (string)s.Element("Eliminado") == "False");
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
                    dr["Cargo"] = NombreElementoSingular; // "Supervisor"
                    dr["Eliminado"] = (string)xElement.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        // Se mantiene este método, aunque la herencia de EmpleadoBaseDAL lo proporciona
        public int CantidadSupervisores()
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


