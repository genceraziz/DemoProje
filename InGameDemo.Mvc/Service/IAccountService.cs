using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Service
{
    public interface IAccountService
    {
        Task<ServiceResult<UserRoleFormForDto>> UserRoleManagement(string token);

        Task<ServiceResult> AddRoleForUser(string token, UserRoleFormForDto model);

        Task<ServiceResult> RequestAccess(string token, string userName);
    }
}