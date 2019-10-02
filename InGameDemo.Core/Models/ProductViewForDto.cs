namespace InGameDemo.Core.Models
{
    /// <summary>
    /// Ürün listeleme için kullanılan model.
    /// </summary>
    public class ProductViewForDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }

        public string ProductName { get; set; }
    }
}
