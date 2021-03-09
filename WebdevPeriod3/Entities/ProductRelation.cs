namespace WebdevPeriod3.Entities
{
    /// <summary>
    /// Represents a relationship between a product and a nested product (a subproduct)
    /// </summary>
    public class ProductRelation
    {
        public string ProductId { get; set; }
        public string SubProductId { get; set; }

        public ProductRelation() { }

        public ProductRelation(string productId, string subProductId) : this()
        {
            ProductId = productId;
            SubProductId = subProductId;
        }

        public ProductRelation(Product product, Product subProduct) : this(product.Id, subProduct.Id) { }
    }
}
