using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.Services
{
    public class ProductManager
    {
        public Task AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByPosterId(string posterId) =>
            // TODO: replace with actual implementation
            Task.FromResult(Enumerable.Range(1, 10).Select(n => new Product()
            {
                Id = n.ToString(),
                Name = $"Product {n}",
                Description = $"Dit is product {n}",
                CreatedAt = DateTime.Now,
                PosterId = posterId,
                ShowInCatalog = true
            }));
    }
}
