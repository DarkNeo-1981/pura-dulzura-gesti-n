using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL;
using ENTITY;

namespace MAPPER
{
    public class OpcionesDeMenuMapper
    {
        public OpcionesDeMenuMapper()
        {
        }

        public List<ENTITY.OpcionesDeMenu> TraerTodo(int idMenuRaiz = 0)
        {
            DAL.OpcionesDeMenuDAL opmDal = new DAL.OpcionesDeMenuDAL();
            List<ENTITY.OpcionesDeMenu> todosLosComponentes = new List<ENTITY.OpcionesDeMenu>();
            Dictionary<int, ENTITY.OpcionesDeMenu> diccionarioComponentes = new Dictionary<int, ENTITY.OpcionesDeMenu>();
            try
            {                
                DataTable dtMenus = opmDal.Buscar();
                if (dtMenus != null && dtMenus.Rows.Count > 0)
                {
                    foreach (DataRow fila in dtMenus.Rows)
                    {
                        int idActual = int.Parse(fila["Id"].ToString());
                        string detalleActual = fila["Detalle"].ToString();                        
                        bool esCompuesto = opmDal.BuscarHijos(idActual).Any();
                        ENTITY.OpcionesDeMenu op;
                        if (esCompuesto)
                        {
                            op = new ENTITY.Compuesto();
                        }
                        else
                        {
                            op = new ENTITY.Patente();
                        }

                        op.Id = idActual;
                        op.Detalle = detalleActual;
                        // Buscar su padre directamente para llenar IdPadre desde Padres_Hijos.xml
                        op.IdPadre = opmDal.BuscarPadre(idActual);
                        todosLosComponentes.Add(op);
                        diccionarioComponentes.Add(op.Id, op);
                    }

                    // 2. Establecer las relaciones padre-hijo en memoria
                    foreach (ENTITY.OpcionesDeMenu componente in todosLosComponentes)
                    {
                        if (componente.IdPadre != 0) // Si tiene un padre (no es una raíz)
                        {
                            if (diccionarioComponentes.TryGetValue(componente.IdPadre, out ENTITY.OpcionesDeMenu padreObjeto))
                            {
                                if (padreObjeto is ENTITY.Compuesto compuestoPadre)
                                {
                                    compuestoPadre.AgregarHijo(componente);
                                }
                            }
                        }
                    }
                }

                // 3. Devolver solo los elementos que son hijos directos del IdMenuRaiz (0 para las raíces)
                return todosLosComponentes.Where(x => x.IdPadre == idMenuRaiz).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OpcionesDeMenuMapper.TraerTodo(int idMenuRaiz): {ex.Message}");
                throw new Exception("Error al obtener la estructura de menú jerárquica del Mapper.", ex);
            }
        }
        public List<ENTITY.OpcionesDeMenu> TraerTodo()
        {
            DAL.OpcionesDeMenuDAL opmDal = new DAL.OpcionesDeMenuDAL();
            List<ENTITY.OpcionesDeMenu> l = castDttoEnt(opmDal.Buscar());
            return l ?? new List<ENTITY.OpcionesDeMenu>();
        }

        private List<ENTITY.OpcionesDeMenu> castDttoEnt(DataTable dt)
        {
            List<ENTITY.OpcionesDeMenu> Lista_op = new List<ENTITY.OpcionesDeMenu>();
            DAL.OpcionesDeMenuDAL opmDal = new DAL.OpcionesDeMenuDAL();
            if (dt != null)
            {
                foreach (DataRow fila in dt.Rows)
                {
                    int idActual = int.Parse(fila["Id"].ToString());
                    string detalleActual = fila["Detalle"].ToString();
                    // Determinar si es compuesto o patente
                    bool esCompuesto = opmDal.BuscarHijos(idActual).Any();
                    ENTITY.OpcionesDeMenu op;
                    if (esCompuesto)
                    {
                        op = new ENTITY.Compuesto();
                    }
                    else
                    {
                        op = new ENTITY.Patente();
                    }

                    op.Id = idActual;
                    op.Detalle = detalleActual;
                    op.IdPadre = opmDal.BuscarPadre(idActual); // Asignar IdPadre también en la lista plana
                    Lista_op.Add(op);
                }
            }
            return Lista_op;
        }

        public int BuscarPadre(int pHijo)
        {
            DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
            return opm.BuscarPadre(pHijo);
        }
        public List<int> BuscarHijos(int pPadre)
        {
            DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
            return opm.BuscarHijos(pPadre);
        }

        public int AgregarMenu(string detalle)
        {
            try
            {
                DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
                return opm.Agregar(detalle); // Delegar a la DAL
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OpcionesDeMenuMapper.AgregarMenu(): {ex.Message}");
                throw new Exception($"Error en Mapper al agregar el menú '{detalle}'.", ex);
            }
        }
        public void ActualizarDetalleMenu(int idMenu, string nuevoDetalle)
        {
            try
            {
                DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
                opm.Modificar(idMenu, nuevoDetalle); // Delegar a la DAL
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OpcionesDeMenuMapper.ActualizarDetalleMenu(): {ex.Message}");
                throw new Exception($"Error en Mapper al actualizar el menú con Id {idMenu}.", ex);
            }
        }

        public void EliminarMenu(int idMenu)
        {
            try
            {
                DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
                opm.Eliminar(idMenu);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OpcionesDeMenuMapper.EliminarMenu(): {ex.Message}");
                throw new Exception($"Error en Mapper al eliminar el menú con Id {idMenu}.", ex);
            }
        }
        public void AgregarRelacionPadreHijo(int idPadre, int idHijo)
        {
            try
            {
                DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
                opm.AgregarRelacion(idPadre, idHijo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OpcionesDeMenuMapper.AgregarRelacionPadreHijo(): {ex.Message}");
                throw new Exception($"Error en Mapper al agregar la relación Padre={idPadre}, Hijo={idHijo}.", ex);
            }
        }

        public void EliminarRelacionPadreHijo(int idPadre, int idHijo)
        {
            try
            {
                DAL.OpcionesDeMenuDAL opm = new DAL.OpcionesDeMenuDAL();
                opm.EliminarRelacion(idPadre, idHijo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OpcionesDeMenuMapper.EliminarRelacionPadreHijo(): {ex.Message}");
                throw new Exception($"Error en Mapper al eliminar la relación Padre={idPadre}, Hijo={idHijo}.", ex);
            }
        }
    }
}