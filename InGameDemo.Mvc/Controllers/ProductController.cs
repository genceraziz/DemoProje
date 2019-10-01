using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}