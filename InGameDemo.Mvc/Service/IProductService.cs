using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InGameDemo.Mvc.Service
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductViewForDto>>> GetProducts(string token);

        Task<ServiceResult<ProductByCategoryViewForDto>> GetProductsByCategory(string token, int categoryId);

        Task<ServiceResult> NewProduct(string token, ProductAddForDto model);

        Task<ServiceResult<ProductViewForDto>> ProductDetail(string token, int id);

        Task<ServiceResult> ProductDelete(string token, int id);

        Task<ServiceResult> ProductUpdate(string token, int id, ProductAddForDto model);
    }
}
