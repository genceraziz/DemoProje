using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InGameDemo.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CategoryManagement()
        {
            return View();
        }
    }
}