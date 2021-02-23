using System;
using System.Linq;
using System.Collections.Generic;
namespace WebdevPeriod3.Entities
{
    /// <summary>
    /// Represents a product in our application
    /// </summary>
    public class Product
    {
        public Product()
        {
            Comments = new List<Comment>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PosterId { get; set; }
        public bool ShowInCatalog { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
        public int Replies => Comments.Count();
    }
}
