using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InGameDemo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var model = new List<ProductViewForDto>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await httpClinet.GetAsync("product/products");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                AddSweetAlert("Beklenmedik bir durum oluştu", message, Models.Enums.NotificationType.warning);

                return View(model);
            }

            model = JsonConvert.DeserializeObject<List<ProductViewForDto>>(await response.Content.ReadAsStringAsync());

            return View(model);
        }

        [Route("/ProductByCategory/{categoryId}")]
        public async Task<IActionResult> ProductById(int categoryId)
        {
            var model = new ProductByCategoryViewForDto();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await httpClinet.GetAsync("product/productsbycategory/" + categoryId);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                AddSweetAlert("Beklenmedik bir durum oluştu", message, Models.Enums.NotificationType.warning);

                return View(model);
            }

            model = JsonConvert.DeserializeObject<ProductByCategoryViewForDto>(await response.Content.ReadAsStringAsync());

            ViewBag.Title = $"{model.CategoryName} Kategorisine Ait Ürünler";

            return View(model);
        }
    }
}