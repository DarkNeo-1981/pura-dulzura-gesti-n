using MAPPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class OpcionesDeMenuBLL
    {
        private OpcionesDeMenuMapper _mapper;

        public OpcionesDeMenuBLL()
        {
            _mapper = new OpcionesDeMenuMapper();
        }

        public List<ENTITY.OpcionesDeMenu> TraerTodo(int idMenuRaiz = 0)
        {
            try
            {
                List<ENTITY.OpcionesDeMenu> ent = _mapper.TraerTodo(idMenuRaiz);
                return ent;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.TraerTodo(int idMenuRaiz): {ex.Message}");
                throw new Exception("Error al traer la estructura de menú jerárquica.", ex);
            }
        }

        public List<ENTITY.OpcionesDeMenu> TraerTodoSinComposite()
        {
            try
            {
                List<ENTITY.OpcionesDeMenu> ent = _mapper.TraerTodo();
                return ent;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.TraerTodoSinComposite(): {ex.Message}");
                throw new Exception("Error al traer la lista plana de opciones de menú.", ex);
            }
        }

        public int BuscarPadre(int pHijo)
        {
            try
            {
                return _mapper.BuscarPadre(pHijo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.BuscarPadre(): {ex.Message}");
                throw new Exception($"Error al buscar el padre del menú {pHijo}.", ex);
            }
        }

        public List<int> BuscarHijos(int pPadre)
        {
            try
            {
                return _mapper.BuscarHijos(pPadre);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.BuscarHijos(): {ex.Message}");
                throw new Exception($"Error al buscar los hijos del menú {pPadre}.", ex);
            }
        }

        public int AgregarMenu(string detalle, int idPadre = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(detalle))
                {
                    throw new ArgumentException("El detalle del menú no puede estar vacío.");
                }

                int nuevoId = _mapper.AgregarMenu(detalle);

                if (idPadre != 0)
                {
                    _mapper.AgregarRelacionPadreHijo(idPadre, nuevoId);
                }
                return nuevoId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.AgregarMenu(): {ex.Message}");
                throw new Exception($"Error al agregar el menú '{detalle}'.", ex);
            }
        }

        public void ActualizarDetalleMenu(int idMenu, string nuevoDetalle)
        {
            try
            {
                if (idMenu <= 0) { throw new ArgumentException("El Id del menú debe ser un valor positivo."); }
                if (string.IsNullOrWhiteSpace(nuevoDetalle)) { throw new ArgumentException("El nuevo detalle del menú no puede estar vacío."); }

                _mapper.ActualizarDetalleMenu(idMenu, nuevoDetalle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.ActualizarDetalleMenu(): {ex.Message}");
                throw new Exception($"Error al actualizar el menú con Id {idMenu}.", ex);
            }
        }

        public void EliminarMenu(int idMenu)
        {
            try
            {
                if (idMenu <= 0) { throw new ArgumentException("El Id del menú a eliminar debe ser un valor positivo."); }

                _mapper.EliminarMenu(idMenu);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.EliminarMenu(): {ex.Message}");
                throw new Exception($"Error al eliminar el menú con Id {idMenu}.", ex);
            }
        }

        public void AgregarRelacionPadreHijo(int idPadre, int idHijo)
        {
            try
            {
                if (idPadre <= 0 || idHijo <= 0) { throw new ArgumentException("Los IDs de padre e hijo deben ser valores positivos."); }
                if (idPadre == idHijo) { throw new ArgumentException("Un menú no puede ser su propio padre."); }

                _mapper.AgregarRelacionPadreHijo(idPadre, idHijo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.AgregarRelacionPadreHijo(): {ex.Message}");
                throw new Exception($"Error al agregar la relación Padre={idPadre}, Hijo={idHijo}.", ex);
            }
        }

        public void EliminarRelacionPadreHijo(int idPadre, int idHijo)
        {
            try
            {
                if (idPadre <= 0 || idHijo <= 0) { throw new ArgumentException("Los IDs de padre e hijo deben ser valores positivos."); }

                _mapper.EliminarRelacionPadreHijo(idPadre, idHijo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en BLL.OpcionesDeMenuBLL.EliminarRelacionPadreHijo(): {ex.Message}");
                throw new Exception($"Error al eliminar la relación Padre={idPadre}, Hijo={idHijo}.", ex);
            }
        }        
    }
}