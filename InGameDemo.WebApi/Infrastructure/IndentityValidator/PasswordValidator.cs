using InGameDemo.WebApi.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InGameDemo.WebApi.Infrastructure.IndentityValidator
{
    /// <summary>
    /// Customer şifre kontroller.
    /// </summary>
    public class PasswordValidator : IPasswordValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            var errors = new List<IdentityError>();

            // Şifre içerisinde kullanıcı adı olamaz.
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PASSERR1", Description = "Şifre içerisinde kullanıcı adı bilgisi olamaz." });
            }

            // Şifre 1234567 olamaz.
            if (password.Contains("1234567"))
            {
                errors.Add(new IdentityError { Code = "PASSERR2", Description = "Şifre içerisinde 1234567 olamaz." });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
