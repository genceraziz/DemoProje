using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Service
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryManagementViewForDto>> GetCategoryManagement(string token);

        Task<ServiceResult<IEnumerable<CategoryViewForDto>>> GetCategories(string token);

        Task<ServiceResult> NewCategory(string token, CategoryManagementViewForDto model);
    }
}
