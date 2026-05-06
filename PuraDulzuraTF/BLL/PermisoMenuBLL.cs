using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL
{
    public class PermisoMenuBLL
    {
        /* Asigna un permiso a un menú y a todos sus descendientes (si el menú es un compuesto).
           Retorna:
                   1: Éxito (se agregó al menos un permiso, el principal o uno de sus descendientes).
                   2: Todos los permisos (principal y sus descendientes) ya existían, no se agregó nada nuevo.
                   0: Fallo general o error.
        */
        public int AsignarPermisoYDescendientes(int pPermisoId, int pMenuId)
        {
            MAPPER.PermisoMenuMapper map = new MAPPER.PermisoMenuMapper();
            int resultadoGlobal = 2;

            try
            {                
                Dictionary<int, List<int>> relacionesPadresHijos = map.ObtenerPadresHijos();

                if (relacionesPadresHijos == null)
                {                    
                    return 0;
                }

                Queue<int> menuesAProcesar = new Queue<int>();
                HashSet<int> menuesYaProcesadosEnSesion = new HashSet<int>();

                // 1. Intentar asignar el permiso al menú padre inicial
                int resAgregarPadre = map.AgregarUno(pPermisoId, pMenuId);
                if (resAgregarPadre == 1)
                {
                    resultadoGlobal = 1;
                    menuesAProcesar.Enqueue(pMenuId);
                    menuesYaProcesadosEnSesion.Add(pMenuId);
                }
                else if (resAgregarPadre == 2)
                {
                    menuesAProcesar.Enqueue(pMenuId);
                    menuesYaProcesadosEnSesion.Add(pMenuId);
                }
                else
                {
                    return 0;
                }

                // 2. Procesar la cola para asignar a los hijos
                while (menuesAProcesar.Count > 0)
                {
                    int currentMenuId = menuesAProcesar.Dequeue();

                    // Buscar los hijos directos del menú actual en la estructura obtenida del Mapper
                    if (relacionesPadresHijos.ContainsKey(currentMenuId))
                    {
                        List<int> hijos = relacionesPadresHijos[currentMenuId];

                        foreach (int hijoId in hijos)
                        {
                            if (!menuesYaProcesadosEnSesion.Contains(hijoId))
                            {
                                int resAgregarHijo = map.AgregarUno(pPermisoId, hijoId);
                                if (resAgregarHijo == 1)
                                {
                                    resultadoGlobal = 1;
                                    menuesAProcesar.Enqueue(hijoId);
                                    menuesYaProcesadosEnSesion.Add(hijoId);
                                }
                                else if (resAgregarHijo == 2)
                                {
                                    menuesAProcesar.Enqueue(hijoId);
                                    menuesYaProcesadosEnSesion.Add(hijoId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al asignar permiso y descendientes en BLL: {ex.Message}");
                resultadoGlobal = 0;
            }
            finally
            {
                map = null;
            }
            return resultadoGlobal;
        }

        public int BorrarPermisoYDescendientes(int pPermisoId, int pMenuId)
        {
            MAPPER.PermisoMenuMapper map = new MAPPER.PermisoMenuMapper();
            int resultadoGlobal = 2;
            try
            {
                // La BLL le pide al Mapper la estructura de padres e hijos.
                Dictionary<int, List<int>> relacionesPadresHijos = map.ObtenerPadresHijos();

                if (relacionesPadresHijos == null)
                {
                    return 0;
                }

                Queue<int> menuesAProcesar = new Queue<int>();
                HashSet<int> menuesYaProcesadosEnSesion = new HashSet<int>();

                // Intentar borrar el permiso principal
                int resBorrarPadre = map.BorrarUno(pPermisoId, pMenuId);
                if (resBorrarPadre == 1)
                {
                    resultadoGlobal = 1;
                    menuesAProcesar.Enqueue(pMenuId);
                    menuesYaProcesadosEnSesion.Add(pMenuId);
                }
                else if (resBorrarPadre == 2)
                {
                    menuesAProcesar.Enqueue(pMenuId);
                    menuesYaProcesadosEnSesion.Add(pMenuId);
                }
                else
                {
                    return 0;
                }

                // Procesar la cola para borrar a los hijos
                while (menuesAProcesar.Count > 0)
                {
                    int currentMenuId = menuesAProcesar.Dequeue();
                    if (relacionesPadresHijos.ContainsKey(currentMenuId))
                    {
                        List<int> hijos = relacionesPadresHijos[currentMenuId];

                        foreach (int hijoId in hijos)
                        {
                            if (!menuesYaProcesadosEnSesion.Contains(hijoId))
                            {
                                int resBorrarHijo = map.BorrarUno(pPermisoId, hijoId);
                                if (resBorrarHijo == 1)
                                {
                                    resultadoGlobal = 1;
                                    menuesAProcesar.Enqueue(hijoId);
                                    menuesYaProcesadosEnSesion.Add(hijoId);
                                }
                                else if (resBorrarHijo == 2)
                                {
                                    menuesAProcesar.Enqueue(hijoId);
                                    menuesYaProcesadosEnSesion.Add(hijoId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al borrar permiso y descendientes en BLL: {ex.Message}");
                resultadoGlobal = 0;
            }
            finally
            {
                map = null;
            }
            return resultadoGlobal;
        }
                
        public int AgregarUno(int pPermisoId, int pMenuId)
        {
            MAPPER.PermisoMenuMapper map = new MAPPER.PermisoMenuMapper();
            return map.AgregarUno(pPermisoId, pMenuId);
        }

        public List<ENTITY.Permiso_Menu> BuscarUno(int permisoId)
        {
            MAPPER.PermisoMenuMapper map = new MAPPER.PermisoMenuMapper();
            return map.Buscar(permisoId);
        }
    }
}