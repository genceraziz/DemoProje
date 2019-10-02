using InGameDemo.Core;
using InGameDemo.Core.Models;
using InGameDemo.Mvc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var serRes = await _categoryService.GetCategories(GetToken());
            if (serRes.ResultStatus.Status != Models.Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Models.Enums.NotificationType.warning);

                return View(string.Empty);
            }

            var model = serRes.Result;
            var html = string.Empty;

            if (model != null && model.Any())
            {
                html = Tools.GenerateCategories(model);
            }

            ViewBag.CategoriesHTML = html;

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CategoryManagement()
        {
            var model = new CategoryManagementViewForDto();

            var serRes = await _categoryService.GetCategoryManagement(GetToken());
            if (serRes.ResultStatus.Status != Models.Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Models.Enums.NotificationType.warning);
                return View(model);
            }

            model = serRes.Result;

            return View(model);
        }

        [HttpPost]
        [Authorize()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCategory(CategoryManagementViewForDto model)
        {
            if (string.IsNullOrEmpty(model.Category.Name))
            {
                AddSweetAlert("", "Lütfen tüm alanları doldurunuz.", Models.Enums.NotificationType.warning);
                return RedirectToAction("CategoryManagement");
            }

            if (model.Category.Name.Length > 100)
            {
                AddSweetAlert("", "Kategori Adı 100 karakterden büyük olamaz.", Models.Enums.NotificationType.warning);
                return RedirectToAction("CategoryManagement");
            }

            if (model.Category.IsParent)
            {
                if (!model.Category.ParentId.HasValue)
                {
                    AddSweetAlert("", "Lütfen üst kategori bilgisini seçiniz.", Models.Enums.NotificationType.warning);
                    return RedirectToAction("CategoryManagement");
                }
            }

            var serRes = await _categoryService.NewCategory(GetToken(), model);
            if (serRes.ResultStatus.Status != Models.Enums.ResultStatus.Success)
            {
                AddSweetAlert("Beklenmedik bir durum oluştu", serRes.ResultStatus.Explanation, Models.Enums.NotificationType.warning);

                return RedirectToAction("CategoryManagement");
            }

            return RedirectToAction("CategoryManagement");
        }
    }
}