using InGameDemo.Core.Models;
using System.Collections.Generic;

namespace InGameDemo.Core.Models
{
    /// <summary>
    /// Bir kategoriye göre üürün listeleme modeli.
    /// </summary>
    public class ProductByCategoryViewForDto
    {
        public List<ProductViewForDto> Products { get; set; }

        public string CategoryName { get; set; }
    }
}
