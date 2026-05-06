using DAL;
using ENTITY;
using System.Linq;

namespace BLL
{
    public class ClienteServiceBLL
    {
        private ClienteRepository repo = new ClienteRepository();

        public Clientes BuscarPorDNI(int dni)
        {
            return repo.ObtenerTodos().FirstOrDefault(c => c.Dni == dni.ToString());
        }
    }
}