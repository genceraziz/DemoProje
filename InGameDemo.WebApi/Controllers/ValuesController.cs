using Microsoft.AspNetCore.Mvc;

namespace InGameDemo.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "InGame Api";
        }
    }
}
