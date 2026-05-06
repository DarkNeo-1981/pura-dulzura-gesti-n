using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class Permisos_MenuService
    {
        private readonly PermisoMenuRepositoryDAL _repo = new PermisoMenuRepositoryDAL();

        public List<PermisoMenuDAL> ObtenerPorPermiso(int permisoId)
            => _repo.BuscarPorPermiso(permisoId);

        public void Asignar(int permisoId, int menuId)
            => _repo.Agregar(new PermisoMenuDAL { PermisoId = permisoId, MenuId = menuId });

        public void Quitar(int permisoId, int menuId)
            => _repo.Borrar(new PermisoMenuDAL { PermisoId = permisoId, MenuId = menuId });

        public bool EstaAsignado(int permisoId)
            => _repo.EstaEnUso(permisoId);
    }
}
