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
    public class ProductService : IProductService
    {
        private IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResult<List<ProductViewForDto>>> GetProducts(string token)
        {
            var serRes = new ServiceResult<List<ProductViewForDto>>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("product/products");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.Result = JsonConvert.DeserializeObject<List<ProductViewForDto>>(await response.Content.ReadAsStringAsync());
            serRes.ResultStatus.Status = Enums.ResultStatus.Success;
            return serRes;
        }

        public async Task<ServiceResult<ProductByCategoryViewForDto>> GetProductsByCategory(string token, int categoryId)
        {
            var serRes = new ServiceResult<ProductByCategoryViewForDto>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("product/productsbycategory/" + categoryId);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.Result = JsonConvert.DeserializeObject<ProductByCategoryViewForDto>(await response.Content.ReadAsStringAsync());
            serRes.ResultStatus.Status = Enums.ResultStatus.Success;
            return serRes;
        }

        public async Task<ServiceResult> NewProduct(string token, ProductAddForDto model)
        {
            var serRes = new ServiceResult();

            var json = JsonConvert.SerializeObject(model);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.PostAsync("product/newproduct", new StringContent(json, Encoding.UTF8, "application/json"));
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

        public async Task<ServiceResult> ProductDelete(string token, int id)
        {
            var serRes = new ServiceResult();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.DeleteAsync("product/productdelete/" + id);
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

        public async Task<ServiceResult<ProductViewForDto>> ProductDetail(string token, int id)
        {
            var serRes = new ServiceResult<ProductViewForDto>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("product/productdetail/" + id);
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.Result = JsonConvert.DeserializeObject<ProductViewForDto>(await response.Content.ReadAsStringAsync());
            serRes.ResultStatus.Status = Enums.ResultStatus.Success;
            return serRes;
        }

        public async Task<ServiceResult> ProductUpdate(string token, int id, ProductAddForDto model)
        {
            var serRes = new ServiceResult();

            var json = JsonConvert.SerializeObject(model);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.PutAsync("product/productupdate/" + id, new StringContent(json, Encoding.UTF8, "application/json"));
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
