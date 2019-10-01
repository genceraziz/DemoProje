using System.Collections.Generic;

namespace InGameDemo.Core.Models
{
    /// <summary>
    /// Kategori yönetimi sayfası için kullanılan model.
    /// </summary>
    public class CategoryManagementViewForDto
    {
        public IEnumerable<CategoryViewForDto> Categories { get; set; }

        public CategoryViewForDto Category { get; set; }
    }
}
