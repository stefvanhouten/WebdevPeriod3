using System.Collections.Generic;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel(Product product, IEnumerable<Product> subProducts, IEnumerable<CommentViewModel> comments)
        {
            Product = product;
            SubProducts = subProducts;
            Comments = comments;
        }

        public Product Product;
        public IEnumerable<Product> SubProducts;
        public IEnumerable<CommentViewModel> Comments;
    }
}


