using InGameDemo.WebApi.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InGameDemo.WebApi.Infrastructure.IndentityValidator
{
    /// <summary>
    /// Custom kullanıcı adı kontrolleri.
    /// </summary>
    public class UserValidator : IUserValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var errors = new List<IdentityError>();

            // Kullanıcı adı içerisinde gamer kelimesi olmalı.
            //if (!user.UserName.ToLower().Contains("gamer"))
            //{
            //    errors.Add(new IdentityError { Code = "USERERR1", Description = "Kullanıcı adı içerisinde gamer kelimesi olmalı :)" });
            //}

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
