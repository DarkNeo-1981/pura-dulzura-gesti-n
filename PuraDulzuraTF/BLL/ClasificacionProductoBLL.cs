using ENTITY;
using MAPPER;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ClasificacionProductoBLL
    {
        private ClasificacionProductoDAL dal = new ClasificacionProductoDAL();
        private ClasificacionProductoMapper mapper = new ClasificacionProductoMapper();

        public List<ClasificacionProducto> ObtenerTodos()
        {
            var nodos = dal.ObtenerTodos();
            return mapper.MapearLista(nodos);
        }

        public void Agregar(ClasificacionProducto nuevo)
        {
            Validar(nuevo);
            nuevo.Id = dal.ObtenerMaxId() + 1;
            var nodo = mapper.MapearAXml(nuevo);
            dal.Guardar(nodo);
        }

        public void Modificar(ClasificacionProducto modificado)
        {
            Validar(modificado);
            var nodo = mapper.MapearAXml(modificado);
            dal.Modificar(nodo);
        }

        public void Eliminar(int id)
        {
            dal.Eliminar(id);
        }

        private void Validar(ClasificacionProducto c)
        {
            if (string.IsNullOrWhiteSpace(c.Detalle))
                throw new Exception("El campo Clasificación es obligatorio.");

            if (c.Porciones <= 0)
                throw new Exception("Las porciones deben ser mayores a cero.");

            if (c.Costo <= 0)
                throw new Exception("El costo debe ser mayor a cero.");
        }
    }
}
