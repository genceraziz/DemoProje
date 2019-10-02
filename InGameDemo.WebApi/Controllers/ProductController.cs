using AutoMapper;
using InGameDemo.Core.Models;
using InGameDemo.WebApi.Data;
using InGameDemo.WebApi.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace InGameDemo.WebApi.Controllers
{
    [Authorize]
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> Products()
        {
            try
            {
                var model = new List<ProductViewForDto>();

                model = _mapper.Map<List<ProductViewForDto>>(await _unitOfWork.ProductRepository.SearchBy(x => x.IsActive));

                return Ok(model.OrderBy(o => o.Id).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("productsbycategory/{categoryId}")]
        public async Task<IActionResult> ProductsByCategory(int categoryId)
        {
            try
            {
                var model = new ProductByCategoryViewForDto
                {
                    Products = new List<ProductViewForDto>(),
                    CategoryName = string.Empty
                };

                model.Products = _mapper.Map<List<ProductViewForDto>>(await _unitOfWork.ProductRepository.SearchBy(x => x.IsActive && x.CategoryId == categoryId));
                model.CategoryName = _unitOfWork.CategoryRepository.GetById(categoryId).Result.Name;
                model.Products = model.Products.OrderBy(o => o.Id).ToList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}