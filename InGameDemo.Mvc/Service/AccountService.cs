using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Service
{
    public class AccountService : IAccountService
    {
        private IHttpClientFactory _httpClientFactory;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResult<UserRoleFormForDto>> UserRoleManagement(string token)
        {
            var serRes = new ServiceResult<UserRoleFormForDto>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("account/userrolemanagement");
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";

                serRes.ResultStatus.Explanation = message;
                return serRes;
            }

            serRes.Result = JsonConvert.DeserializeObject<UserRoleFormForDto>(await response.Content.ReadAsStringAsync());
            serRes.ResultStatus.Status = Enums.ResultStatus.Success;
            return serRes;
        }

        public async Task<ServiceResult> AddRoleForUser(string token, UserRoleFormForDto model)
        {
            var serRes = new ServiceResult();

            var json = JsonConvert.SerializeObject(model);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.PostAsync("account/addroleforuser", new StringContent(json, Encoding.UTF8, "application/json"));
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

        public async Task<ServiceResult> RequestAccess(string token, string userName)
        {
            var serRes = new ServiceResult<UserRoleFormForDto>();

            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClinet.GetAsync("account/requestaccess/" + userName);
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

        public async Task<ServiceResult> Register(RegisterForDto model)
        {
            var serRes = new ServiceResult();

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            var response = await httpClinet.PostAsync("account/register", new StringContent(json, Encoding.UTF8, "application/json"));
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
