namespace InGameDemo.Core.Models
{
    /// <summary>
    /// Ürün ekleme ve güncelleme için kullanılan model.
    /// </summary>
    public class ProductAddForDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }
    }
}
