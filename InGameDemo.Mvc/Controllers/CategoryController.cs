using InGameDemo.Core;
using InGameDemo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        private IHttpClientFactory _httpClientFactory;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await httpClinet.GetAsync("category/categories");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                AddSweetAlert("Beklenmedik bir durum oluştu", message, Models.Enums.NotificationType.warning);

                return View(string.Empty);
            }

            var model = JsonConvert.DeserializeObject<IEnumerable<CategoryViewForDto>>(await response.Content.ReadAsStringAsync());

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

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await httpClinet.GetAsync("category/categorymanagement");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                AddSweetAlert("Beklenmedik bir durum oluştu", message, Models.Enums.NotificationType.warning);

                return View(model);
            }

            model = JsonConvert.DeserializeObject<CategoryManagementViewForDto>(await response.Content.ReadAsStringAsync());

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

            var json = JsonConvert.SerializeObject(model.Category);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await httpClinet.PostAsync("category/newcategory", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                AddSweetAlert("Beklenmedik bir durum oluştu", message, Models.Enums.NotificationType.warning);

                return RedirectToAction("CategoryManagement");
            }

            return RedirectToAction("CategoryManagement");
        }
    }
}