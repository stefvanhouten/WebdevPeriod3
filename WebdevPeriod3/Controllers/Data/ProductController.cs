using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Interfaces;

namespace WebdevPeriod3.Controllers.Data
{
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GellAll()
        {
            var products = await _productRepository.GetAllProducts();
            return Ok(products);
        }

        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productRepository.GetById(id);
            return Ok(product);
        }
   
        public async Task<ActionResult> AddProduct(Product entity)
        {
            await _productRepository.AddProduct(entity);
            return Ok(entity);
        }
     
        public async Task<ActionResult<Product>> Update(Product entity, int id)
        {
            await _productRepository.UpdateProduct(entity, id);
            return Ok(entity);
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _productRepository.RemoveProduct(id);
            return Ok();
        }
    }
}
