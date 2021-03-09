using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Services;

namespace WebdevPeriod3.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductSearchController : ControllerBase
    {

        private readonly ProductRepository _productRepository;

        public ProductSearchController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get(string searchTerm)
        {
            return await _productRepository.FindProductsBySearchTerm(searchTerm);
        }

    }
}
