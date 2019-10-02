using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using InGameDemo.Mvc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InGameDemo.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : BaseController
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> UserRoleManagement()
        {
            var model = new UserRoleFormForDto();

            var serRes = await _accountService.UserRoleManagement(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return View(model);
            }

            model = serRes.Result;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRoleManagement(UserRoleFormForDto model)
        {
            var serRes = await _accountService.UserRoleManagement(GetToken());
            if (serRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("", serRes.ResultStatus.Explanation, Enums.NotificationType.warning);

                return View(model);
            }

            model.Users = serRes.Result.Users;
            model.Roles = serRes.Result.Roles;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var addRoleSerRes = await _accountService.AddRoleForUser(GetToken(), model);
            if (addRoleSerRes.ResultStatus.Status != Enums.ResultStatus.Success)
            {
                AddSweetAlert("", addRoleSerRes.ResultStatus.Explanation, Enums.NotificationType.warning);
                return View(model);
            }

            AddSweetAlert("", "Kullanıcıya başarıyla rol tanımlandı.", Enums.NotificationType.success);

            return RedirectToAction("UserRoleManagement");
        }
    }
}