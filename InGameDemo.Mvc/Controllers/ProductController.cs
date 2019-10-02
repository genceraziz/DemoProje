using InGameDemo.Core;
using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using InGameDemo.Mvc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductService productService, ICategoryService categoryService, IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = new List<ProductViewForDto>();
            var serRes = await _productService.GetProducts(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);
                return View(model);
            }

            model = serRes.Result;

            return View(model);
        }

        [Route("/ProductByCategory/{categoryId}"), HttpGet]
        public async Task<IActionResult> ProductById(int categoryId)
        {
            var model = new ProductByCategoryViewForDto();
            var serRes = await _productService.GetProductsByCategory(GetToken(), categoryId);
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);
                return View(model);
            }

            model = serRes.Result;

            ViewBag.Title = $"{serRes.Result.CategoryName} Kategorisine Ait Ürünler";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> NewProduct()
        {
            var model = new ProductFormForDto();

            var serRes = await _categoryService.GetCategoryManagement(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return View(model);
            }

            if (serRes.Result != null && serRes.Result.Categories != null && serRes.Result.Categories.Any())
            {
                model.Categories = serRes.Result.Categories;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewProduct(ProductFormForDto model)
        {
            #region Kategorileri Çekiyoruz

            var serRes = await _categoryService.GetCategoryManagement(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return View(new ProductFormForDto());
            }

            if (serRes.Result != null && serRes.Result.Categories != null && serRes.Result.Categories.Any())
            {
                model.Categories = serRes.Result.Categories;
            }

            #endregion

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Name) || !model.CategoryId.HasValue || model.File == null || !model.Price.HasValue)
                {
                    ModelState.AddModelError("", "Lütfen tüm alanları doldurunuz.");
                }

                return View(model);
            }

            var uniqueFileName = Tools.GetUniqueFileName(model.File.FileName);
            var uploadsPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            // Ürün ekleniyor.
            var dataModel = new ProductAddForDto
            {
                Name = model.Name,
                ImageUrl = "/uploads/" + uniqueFileName,
                Price = model.Price.Value,
                CategoryId = model.CategoryId.Value,
                Description = model.Description,
                UserName = GetUserName()
            };

            var newProductSerRes = await _productService.NewProduct(GetToken(), dataModel);
            if (newProductSerRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Hata", serRes.ResultStatus.Explanation, Enums.NotificationType.error);
                return View(model);
            }

            var filePath = Path.Combine(uploadsPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
                stream.Close();
            }

            AddSweetAlert("Tebrikler", "Ürün başarıyla eklendi", Enums.NotificationType.success);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Product/ProductDetail/{name}/{id}")]
        [Authorize(Roles = "Admin, Product_View")]
        public async Task<IActionResult> ProductDetail(int id)
        {
            var model = new ProductViewForDto();

            var serRes = await _productService.ProductDetail(GetToken(), id);
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Bulunamadı.", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);
                return View(model);
            }

            model = serRes.Result;

            return View(model);
        }

        [HttpGet]
        [Route("Product/ProductUpdate/{id}")]
        public async Task<IActionResult> ProductUpdate(int id)
        {
            var model = new ProductFormForDto();

            #region Kategoriler
            var serRes = await _categoryService.GetCategoryManagement(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return View(model);
            }

            if (serRes.Result != null && serRes.Result.Categories != null && serRes.Result.Categories.Any())
            {
                model.Categories = serRes.Result.Categories;
            }
            #endregion

            var productSerRes = await _productService.ProductDetail(GetToken(), id);
            if (productSerRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Bulunamadı.", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);
                return View(model);
            }

            model.Id = productSerRes.Result.Id;
            model.Name = productSerRes.Result.Name;
            model.Price = productSerRes.Result.Price;
            model.Description = productSerRes.Result.Description;
            model.CategoryId = productSerRes.Result.CategoryId;
            model.ImageName = productSerRes.Result.ImageUrl;

            ViewBag.ImageUrl = productSerRes.Result.ImageUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductUpdate(int id, ProductFormForDto model)
        {
            #region Kategorileri Çekiyoruz

            var serRes = await _categoryService.GetCategoryManagement(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return RedirectToAction("ProductUpdate", new { id });
            }

            if (serRes.Result != null && serRes.Result.Categories != null && serRes.Result.Categories.Any())
            {
                model.Categories = serRes.Result.Categories;
            }

            #endregion

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Name) || !model.CategoryId.HasValue || !model.Price.HasValue)
                {
                    ModelState.AddModelError("", "Lütfen tüm alanları doldurunuz.");
                }

                return View(model);
            }

            var uniqueFileName = string.Empty;
            var uploadsPath = string.Empty;
            var imageUrl = model.ImageName;
            if (model.File != null)
            {
                uniqueFileName = Tools.GetUniqueFileName(model.File.FileName);
                uploadsPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                imageUrl = "/uploads/" + uniqueFileName;
            }

            // Ürün ekleniyor.
            var dataModel = new ProductAddForDto
            {
                Id = model.Id,
                Name = model.Name,
                ImageUrl = imageUrl,
                Price = model.Price.Value,
                CategoryId = model.CategoryId.Value,
                Description = model.Description,
                UserName = GetUserName()
            };

            var newProductSerRes = await _productService.ProductUpdate(GetToken(), dataModel.Id, dataModel);
            if (newProductSerRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Hata", serRes.ResultStatus.Explanation, Enums.NotificationType.error);
                return View(model);
            }

            if (model.File != null)
            {
                var filePath = Path.Combine(uploadsPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                    stream.Close();
                }
            }

            AddSweetAlert("Tebrikler", "Ürün başarıyla eklendi", Enums.NotificationType.success);

            return RedirectToAction("ProductDetail", new { name = Url.UrlFormat(model.Name), id = model.Id });
        }

        [HttpDelete("{id}")]
        [Route("Product/ProductDelete/{id}")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var serRes = await _productService.ProductDelete(GetToken(), id);
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("Hata", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return RedirectToAction("ProductDetail", new { id });
            }

            AddSweetAlert("", "Ürün başarıyla silindi", Enums.NotificationType.success);
            return RedirectToAction("Index");
        }
    }
}