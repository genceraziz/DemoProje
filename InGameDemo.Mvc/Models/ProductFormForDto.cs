using InGameDemo.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InGameDemo.Mvc.Models
{
    public class ProductFormForDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen ürün ismini giriniz")]
        [MaxLength(500, ErrorMessage = "Ürün ismi maksimum 500 karakter olabilir")]
        [Display(Name = "Ürün İsmi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen kategori seçiniz")]
        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }

        public string ImageName { get; set; }

        [Required(ErrorMessage = "Lütfen ürün fiyatını belirtiniz")]
        [Display(Name = "Ürün Fiyatı")]
        [DisplayFormat(DataFormatString = "{0:#.00}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }

        [Display(Name = "Ürün Açıklaması")]
        public string Description { get; set; }

        [Display(Name = "Ürün Resmi")]
        public IFormFile File { get; set; }

        public IEnumerable<CategoryViewForDto> Categories { get; set; }
    }
}
