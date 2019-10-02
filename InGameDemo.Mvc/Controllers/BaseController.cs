using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using static InGameDemo.Mvc.Models.Enums;

namespace InGameDemo.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public void AddSweetAlert(string title, string message, NotificationType notificationType)
        {
            TempData["notification"] = "swal('" + title + "', '" + message + "', '" + notificationType + "');";
        }

        public string GetToken()
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "Token").Value;

            return token;
        }

        public string GetUserName()
        {
            var name = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

            return name;
        }
    }
}
