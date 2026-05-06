using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public class PermisoMenuRepositoryDAL
    {
        private readonly string ruta;

        public PermisoMenuRepositoryDAL()
        {
            string directorio = Path.Combine(Environment.CurrentDirectory, "DB");
            ruta = Path.Combine(directorio, "Permisos_Menu.xml");

            if (!Directory.Exists(directorio))
                Directory.CreateDirectory(directorio);

            if (!File.Exists(ruta))
                InicializarArchivo();
        }

        private void InicializarArchivo()
        {
            var doc = new XDocument(new XElement("Permisos_Menu"));
            doc.Save(ruta);
        }

        public List<PermisoMenuDAL> BuscarPorPermiso(int permisoId)
        {
            var doc = XDocument.Load(ruta);
            return doc.Descendants("Permiso_Menu")
                .Where(x => (int)x.Attribute("PermisoId") == permisoId)
                .Select(x => new PermisoMenuDAL
                {
                    PermisoId = (int)x.Attribute("PermisoId"),
                    MenuId = (int)x.Attribute("MenuId")
                }).ToList();
        }

        public void Agregar(PermisoMenuDAL permiso)
        {
            var doc = XDocument.Load(ruta);
            doc.Root.Add(new XElement("Permiso_Menu",
                new XAttribute("PermisoId", permiso.PermisoId),
                new XAttribute("MenuId", permiso.MenuId)));
            doc.Save(ruta);
        }

        public void Borrar(PermisoMenuDAL permiso)
        {
            var doc = XDocument.Load(ruta);
            doc.Descendants("Permiso_Menu")
                .Where(x =>
                    (int)x.Attribute("PermisoId") == permiso.PermisoId &&
                    (int)x.Attribute("MenuId") == permiso.MenuId)
                .Remove();
            doc.Save(ruta);
        }

        public bool EstaEnUso(int permisoId)
        {
            var doc = XDocument.Load(ruta);
            return doc.Descendants("Permiso_Menu")
                .Any(x => (int)x.Attribute("PermisoId") == permisoId);
        }
    }
}
