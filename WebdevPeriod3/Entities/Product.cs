using System;
namespace WebdevPeriod3.Entities
{
    /// <summary>
    /// Represents a product in our application
    /// </summary>
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PosterId { get; set; }
        public bool ShowInCatalog { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
