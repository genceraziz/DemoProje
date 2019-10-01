using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InGameDemo.WebApi.Controllers
{
    [Authorize]
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

    }
}