using InGameDemo.WebApi.Data;
using Microsoft.AspNetCore.Identity;

namespace InGameDemo.WebApi.Infrastructure.Seed
{
    public static class IdentitySeedDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("ingame").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "ingame",
                    Email = "azizgencerr@gmail.com"
                };

                var result = userManager.CreateAsync(user, "gamer_1").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole { Name = "Admin" };

                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole { Name = "User" };

                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Product_View").Result)
            {
                var role = new IdentityRole { Name = "Product_View" };

                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
