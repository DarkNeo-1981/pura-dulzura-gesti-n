using System.Linq;
using DAL;
using ENTITY;

namespace BLL
{
    public class ProductoServiceBLL
    {
        private ProductoRepository repo = new ProductoRepository();

        public Producto BuscarPorId(int id)
        {
            return repo.ObtenerTodos().FirstOrDefault(p => p.ID == id);
        }
    }
}
