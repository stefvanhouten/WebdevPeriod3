using System.Collections.Generic;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel(Product product, IEnumerable<Product> subProducts)
        {
            Product = product;
            SubProducts = subProducts;
        }

        public Product Product;
        public IEnumerable<Product> SubProducts;
    }
}


