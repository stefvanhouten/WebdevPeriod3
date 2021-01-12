using System.Collections.Generic;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.Interfaces
{
    public interface IProductRepository
    {
        ValueTask<Product> GetById(int id);
        Task AddProduct(Product entity);
        Task UpdateProduct(Product entity, int id);
        Task RemoveProduct(int id);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
