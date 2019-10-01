using System;
using System.Collections.Generic;

namespace InGameDemo.WebApi.Data
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
