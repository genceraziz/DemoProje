using System;

namespace InGameDemo.Core.Models
{
    public class CategoryViewForDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsParent { get; set; }

        public int? ParentId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string ParentName { get; set; }
    }
}
