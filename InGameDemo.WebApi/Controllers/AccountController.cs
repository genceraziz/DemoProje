using AutoMapper;
using InGameDemo.Core.Models;
using InGameDemo.WebApi.Data;
using InGameDemo.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InGameDemo.WebApi.Controllers
{
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;
        private IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                IOptions<AppSettings> appSettings, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterForDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest("Lütfen tüm alanları doldurunuz.");
                }

                var isExist = await _userManager.FindByNameAsync(model.UserName);
                if (isExist != null)
                {
                    return BadRequest($"{model.UserName} isminde zaten bir kullanıcı var.");
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var identityResult = await _userManager.CreateAsync(applicationUser, model.Password);
                if (!identityResult.Succeeded)
                {
                    return BadRequest(string.Join(",", identityResult.Errors.Select(s => s.Description).ToArray()));
                }

                // Default user rol'ü ekleniyor.
                await _userManager.AddToRoleAsync(applicationUser, "User");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginForDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Lütfen tüm alanları doldurunuz.");
                }

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return BadRequest($"{model.UserName} adında bir kullanıcı bulunamadı.");
                }

                var signIn = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (!signIn.Succeeded)
                {
                    return BadRequest("Kullanıcı girişi başarısız. Lütfen bilgilerinizi kontrol ediniz.");
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var expireDate = DateTime.UtcNow.AddHours(1);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, string.Join(",", userRoles))
                    }),
                    Expires = expireDate,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.SecretKey)), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var userInfo = new UserInfo
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Token = tokenHandler.WriteToken(token),
                    ExpireDate = expireDate,
                    Roles = userRoles.ToArray()
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordForDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    return BadRequest("Lütfen tüm alanları doldurunuz.");
                }

                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    return BadRequest($"{model.UserName} isminde bir kullanıcı bulunamadı. Lütfen bilgilerinizi kontrol ediniz.");
                }

                var passResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = "http://localhost:1453/Home/ResetPassword?token=" + passResetToken;
                var mailBody = $"Merhaba {user.UserName}, <br> Şifre sıfırlama linkiniz aşağıdaki gibidir. Eğer şifre sıfırlama linkini siz talep etmediyseniz lütfen bu maili dikkate almayınız. <br><br> {resetLink} <br><br> Teşekkürler, <br> InGame Group";

                Tools.SendEmail(mailBody, user.Email, user.UserName);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordForDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword) || string.IsNullOrEmpty(model.Token))
                {
                    return BadRequest("Lütfen tüm alanları doldurunuz.");
                }

                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    return BadRequest($"{model.UserName} isminde bir kullanıcı bulunamadı. Lütfen bilgilerinizi kontrol ediniz.");
                }

                var resetPassResult = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(model.Token), model.Password);
                if (!resetPassResult.Succeeded)
                {
                    return BadRequest(string.Join(",", resetPassResult.Errors.Select(s => s.Description)));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("userrolemanagement")]
        [Authorize]
        public IActionResult UserRoleManagement()
        {
            try
            {
                var model = new UserRoleFormForDto
                {
                    Users = _mapper.Map<List<UserForDto>>(_userManager.Users.ToList()),
                    Roles = _mapper.Map<List<RoleForDto>>(_roleManager.Roles.ToList())
                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("addroleforuser")]
        [Authorize]
        public async Task<IActionResult> AddRoleForUser([FromBody]UserRoleFormForDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                if (user.UserName == "ingame")
                {
                    return BadRequest("Sistem kullanıcısı için değişiklik yapamazsınız");
                }

                var roleExist = await _userManager.GetRolesAsync(user);
                if (roleExist.Contains(model.Role))
                {
                    return BadRequest("Atamak istediğiniz rol zaten kullanıcıya verilmiş");
                }

                await _userManager.AddToRoleAsync(user, model.Role);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("requestaccess/{userName}")]
        [Authorize]
        public async Task<IActionResult> RequestAccess(string userName)
        {
            try
            {
                var systemUser = await _userManager.FindByNameAsync("ingame");
                if (systemUser == null)
                {
                    return BadRequest("Bir problem oldu. Lütfen daha sonra tekrar deneyiniz");
                }

                var mailBody = $"Merhaba Patron :) <br><br> <b>{userName}</b> isimli kullanıcı hak talep ediyor. İstersen kullanıcı rol yönetiminden rol tanımı yapabilirsin. <br><br> İyi Çalışmalar";
                Tools.SendEmail(mailBody, systemUser.Email, systemUser.UserName);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}