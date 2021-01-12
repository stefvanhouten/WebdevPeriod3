namespace WebdevPeriod3.Interfaces
{
    public interface IProductCommandText
    {
        string GetProducts { get; }
        string GetProductById { get; }
        string AddProduct { get; }
        string UpdateProduct { get; }
        string RemoveProduct { get; }
    }

    public class ProductCommandText : IProductCommandText
    {
        public string GetProducts => "Select * From Product";
        public string GetProductById => "Select * From Product Where Id= @Id";
        public string AddProduct => "Insert Into  Product (Name, Cost, CreatedDate) Values (@Name, @Cost, @CreatedDate)";
        public string UpdateProduct => "Update Product set Name = @Name, Cost = @Cost, CreatedDate = GETDATE() Where Id =@Id";
        public string RemoveProduct => "Delete From Product Where Id= @Id";
    }
}
