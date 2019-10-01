using Microsoft.AspNetCore.Mvc;
using static InGameDemo.Mvc.Models.Enums;

namespace InGameDemo.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public void AddSweetAlert(string title, string message, NotificationType notificationType)
        {
            TempData["notification"] = "swal('" + title + "', '" + message + "', '" + notificationType + "');";
        }
    }
}
