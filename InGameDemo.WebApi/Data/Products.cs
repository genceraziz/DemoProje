using System;
using System.Collections.Generic;

namespace InGameDemo.WebApi.Data
{
    public partial class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }

        public virtual Categories Category { get; set; }
    }
}
