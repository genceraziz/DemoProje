using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Service
{
    public class CategoryService : ICategoryService
    {
        private IHttpClientFactory _httpClientFactory;

        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResult<IEnumerable<CategoryViewForDto>>> GetCategories(string token)
        {
            var serRes = new ServiceResult<IEnumerable<CategoryViewForDto>>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("category/categories");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.Result = JsonConvert.DeserializeObject<IEnumerable<CategoryViewForDto>>(await response.Content.ReadAsStringAsync());

            serRes.ResultStatus.Status = Enums.ResultStatus.Success;
            return serRes;
        }

        public async Task<ServiceResult<CategoryManagementViewForDto>> GetCategoryManagement(string token)
        {
            var serRes = new ServiceResult<CategoryManagementViewForDto>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("category/categorymanagement");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.Result = JsonConvert.DeserializeObject<CategoryManagementViewForDto>(await response.Content.ReadAsStringAsync());
            serRes.ResultStatus.Status = Enums.ResultStatus.Success;

            return serRes;
        }

        public async Task<ServiceResult> NewCategory(string token, CategoryManagementViewForDto model)
        {
            var serRes = new ServiceResult();

            var json = JsonConvert.SerializeObject(model.Category);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.PostAsync("category/newcategory", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.ResultStatus.Status = Enums.ResultStatus.Success;
            return serRes;
        }
    }
}
