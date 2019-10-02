using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using InGameDemo.Core.Models;
using InGameDemo.WebApi.Data;
using InGameDemo.WebApi.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InGameDemo.WebApi.Controllers
{
    [Route("api/Category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("categorymanagement")]
        public async Task<IActionResult> CategoryManagement()
        {
            try
            {
                var model = new CategoryManagementViewForDto();
                model.Categories = _mapper.Map<IEnumerable<CategoryViewForDto>>(await _unitOfWork.CategoryRepository.GetAll());

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("newcategory")]
        public async Task<IActionResult> NewCatregory([FromBody]CategoryViewForDto model)
        {
            try
            {
                model.CreateDate = DateTime.Now;
                if (!model.IsParent)
                {
                    model.ParentId = null;
                }

                if (model.ParentId.HasValue)
                {
                    model.IsParent = false;
                }

                var dataModel = _mapper.Map<Categories>(model);

                if (dataModel.ParentId.HasValue)
                {
                    var parentCategory = await _unitOfWork.CategoryRepository.GetById(dataModel.ParentId.Value);
                    if (!parentCategory.IsParent)
                    {
                        parentCategory.IsParent = true;
                        await _unitOfWork.CategoryRepository.Update(parentCategory);
                    }
                }

                await _unitOfWork.CategoryRepository.Add(dataModel);
                var result = await _unitOfWork.Save();

                if (!result)
                {
                    return BadRequest("Kategori eklenemedi. Lütfen daha sonra tekrar deneyiniz");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult Categories()
        {
            try
            {
                var categories = _mapper.Map<IEnumerable<CategoryViewForDto>>(_unitOfWork.Context.Categories.FromSql("EXECUTE spCategories").ToList());

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}