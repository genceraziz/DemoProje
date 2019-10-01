using InGameDemo.Core.Models;
using InGameDemo.Mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static InGameDemo.Mvc.Models.Enums;

namespace InGameDemo.Mvc.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterForDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterForDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            var response = await httpClinet.PostAsync("account/register", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";
                ModelState.AddModelError("", message);
            }
            else
            {
                AddSweetAlert("Teşekkürler", "Kaydınız başarıyla yapıldı. Bilgileriniz ile giriş yapabilirsiniz.", NotificationType.success);

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginForDto { ReturnUrl = ReturnUrl };

            return View(new LoginForDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginForDto model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model, Formatting.Indented);
                var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
                var response = await httpClinet.PostAsync("account/login", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";
                    ModelState.AddModelError("", message);
                }
                else
                {
                    try
                    {
                        var userInfoRead = await response.Content.ReadAsStringAsync();
                        var userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoRead);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                            new Claim(ClaimTypes.Name, userInfo.Name),
                            new Claim(ClaimTypes.Email, userInfo.Email),
                            new Claim("Roles", string.Join(",", userInfo.Roles)),
                            new Claim("Token", userInfo.Token),
                        };

                        foreach (var role in userInfo.Roles)
                        {
                            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
                        }

                        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(userIdentity);

                        await HttpContext.SignInAsync(principal, new AuthenticationProperties { IsPersistent = true, ExpiresUtc = userInfo.ExpireDate });

                        if (!string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordForDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordForDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            var response = await httpClinet.PostAsync("account/forgotpassword", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";
                ModelState.AddModelError("", message);
            }
            else
            {
                AddSweetAlert("", "Şifre reset linkiniz mail adresine gönderilmiştir. Lütfen mail adresinizi kontrol ediniz.", NotificationType.success);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var model = new ResetPasswordForDto();

            if (string.IsNullOrEmpty(token))
            {
                AddSweetAlert("Geçersiz token.", "Lütfen tekrar deneyiniz.", NotificationType.warning);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordForDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var httpClinet = _httpClientFactory.CreateClient("ingamedemo");
            var response = await httpClinet.PostAsync("account/resetpassword", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message)) message = "Beklenmedik bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";
                ModelState.AddModelError("", message);
            }
            else
            {
                AddSweetAlert("Tebrikler.", "Şifreniz yenilenmiştir. Giriş yapabilirsiniz.", NotificationType.success);

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}
