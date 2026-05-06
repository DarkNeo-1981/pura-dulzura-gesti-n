using DAL;
using ENTITY;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class EmpleadoServiceBLL
    {        
        private readonly AdministrativoDAL _administrativoDAL;
        private readonly SupervisorDAL _supervisorDAL;
        private readonly JefeDAL _jefeDAL;
        private readonly GerenteDAL _gerenteDAL;
        private readonly EncargadoDAL _encargadoDAL;
        private readonly VendedorDAL _vendedorDAL;

        public EmpleadoServiceBLL()
        {            
            _administrativoDAL = new AdministrativoDAL();
            _supervisorDAL = new SupervisorDAL();
            _jefeDAL = new JefeDAL();
            _gerenteDAL = new GerenteDAL();
            _encargadoDAL = new EncargadoDAL();
            _vendedorDAL = new VendedorDAL();
        }
                
        // Asigna la propiedad Categoria al empleado basándose en el tipo de su instancia.       
        private EmpleadoBase AsignarCategoria(EmpleadoBase empleado)
        {
            if (empleado == null) return null;                       
            if (empleado is Gerente)
                empleado.Categoria = "Gerente";
            else if (empleado is Jefe)
                empleado.Categoria = "Jefe";
            else if (empleado is Supervisor)
                empleado.Categoria = "Supervisor";
            else if (empleado is Encargado)
                empleado.Categoria = "Encargado";
            else if (empleado is Administrativo)
                empleado.Categoria = "Administrativo";
            else if (empleado is Vendedor)
                empleado.Categoria = "Vendedor";
            else
                empleado.Categoria = "Categoría Desconocida"; // En caso de un EmpleadoBase genérico

            return empleado;
        }

        // Busca un empleado por DNI en todas las tablas de roles y le asigna su categoría.
        public EmpleadoBase ObtenerEmpleadoPorDni(int dni)
        {
            EmpleadoBase empleado = null;

            empleado = _gerenteDAL.ObtenerPorDNI(dni);
            if (empleado != null) return AsignarCategoria(empleado);

            empleado = _jefeDAL.ObtenerPorDNI(dni);
            if (empleado != null) return AsignarCategoria(empleado); // <--- Se asigna aca.

            empleado = _supervisorDAL.ObtenerPorDNI(dni);
            if (empleado != null) return AsignarCategoria(empleado);

            empleado = _encargadoDAL.ObtenerPorDNI(dni);
            if (empleado != null) return AsignarCategoria(empleado);

            empleado = _administrativoDAL.ObtenerPorDNI(dni);
            if (empleado != null) return AsignarCategoria(empleado);

            empleado = _vendedorDAL.ObtenerPorDNI(dni);
            if (empleado != null) return AsignarCategoria(empleado);

            return null; // No encontrado en ninguna categoría
        }
                
        // Obtiene una lista consolidada de todos los empleados activos y les asigna su categoría.        
        public List<EmpleadoBase> ObtenerTodosLosEmpleados()
        {
            List<EmpleadoBase> todos = new List<EmpleadoBase>();

            // Obtener y asignar categorías de todos
            todos.AddRange(_gerenteDAL.ObtenerTodos().Select(AsignarCategoria));
            todos.AddRange(_jefeDAL.ObtenerTodos().Select(AsignarCategoria));
            todos.AddRange(_supervisorDAL.ObtenerTodos().Select(AsignarCategoria));
            todos.AddRange(_encargadoDAL.ObtenerTodos().Select(AsignarCategoria));
            todos.AddRange(_administrativoDAL.ObtenerTodos().Select(AsignarCategoria));
            todos.AddRange(_vendedorDAL.ObtenerTodos().Select(AsignarCategoria));

            return todos;
        }
    }
}