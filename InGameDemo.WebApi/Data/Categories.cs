using System;
using System.Collections.Generic;

namespace InGameDemo.WebApi.Data
{
    public partial class Categories
    {
        public Categories()
        {
            InverseParent = new HashSet<Categories>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Categories Parent { get; set; }
        public virtual ICollection<Categories> InverseParent { get; set; }
    }
}
