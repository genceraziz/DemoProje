using AutoMapper;
using InGameDemo.Core.Models;
using InGameDemo.WebApi.Data;
using InGameDemo.WebApi.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

                model = _mapper.Map<List<ProductViewForDto>>(_unitOfWork.Context.Products.FromSql("EXECUTE spProducts"));

                var getAllProducts = await _unitOfWork.CategoryRepository.GetAll();

                if (model != null && model.Any() && getAllProducts != null && getAllProducts.Any())
                {
                    model.ForEach(x =>
                    {
                        x.CategoryName = getAllProducts.FirstOrDefault(c => c.Id == x.CategoryId)?.Name;
                    });
                }

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

                model.Products = _mapper.Map<List<ProductViewForDto>>(await _unitOfWork.ProductRepository.SearchBy(x => x.IsActive && x.CategoryId == categoryId, x => x.Category)).OrderByDescending(o => o.Id).ToList();

                // Products ların boş olma ihtimaline karşı özel olarak çekiliyor.
                model.CategoryName = _unitOfWork.CategoryRepository.GetById(categoryId).Result.Name;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("newproduct")]
        public async Task<IActionResult> NewProduct([FromBody]ProductAddForDto model)
        {
            try
            {
                var dataModel = _mapper.Map<Products>(model);
                dataModel.Category = null;
                dataModel.CreateDate = DateTime.Now;
                dataModel.IsActive = true;

                await _unitOfWork.ProductRepository.Add(dataModel);
                var result = await _unitOfWork.Save();
                if (!result)
                {
                    return BadRequest("Ürün eklenemedi. Lütfen daha sonra tekrar deneyiniz");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("productdetail/{id}")]
        public async Task<IActionResult> ProductDetail(int id)
        {
            try
            {
                var model = new ProductViewForDto();
                model = _mapper.Map<ProductViewForDto>(await _unitOfWork.ProductRepository.FindBy(x => x.Id == id && x.IsActive, x => x.Category));

                if (model == null)
                {
                    return NotFound("Ürün bulunamadı. Lütfen daha sonra tekrar deneyiniz.");
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("ProductDelete/{id}")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            try
            {
                var getProduct = await _unitOfWork.ProductRepository.GetById(id);
                if (getProduct == null)
                {
                    return NotFound("Ürün bulunamadı. Lütfen daha sonra tekrar deneyiniz.");
                }

                getProduct.IsActive = false;

                await _unitOfWork.ProductRepository.Update(getProduct);
                var result = await _unitOfWork.Save();
                if (!result)
                {
                    return BadRequest("Ürün silinemedi. Lütfen daha sonra tekrar deneyiniz.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("ProductUpdate/{id}")]
        public async Task<IActionResult> ProductUpdate(int id, [FromBody]ProductAddForDto model)
        {
            try
            {
                var getProduct = await _unitOfWork.ProductRepository.GetById(id);
                if (getProduct == null)
                {
                    return NotFound("Ürün bulunamadı.");
                }

                if (!getProduct.IsActive)
                {
                    return NotFound("Ürün güncellenemez. Ürün silinmiş");
                }

                getProduct.Name = model.Name;
                getProduct.ImageUrl = model.ImageUrl;
                getProduct.CategoryId = model.CategoryId;
                getProduct.Description = model.Description;
                getProduct.Price = model.Price;

                await _unitOfWork.ProductRepository.Update(getProduct);
                var result = await _unitOfWork.Save();
                if (!result)
                {
                    return BadRequest("Ürün güncellenemedi. Lütfen daha sonra tekrar deneyiniz");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}