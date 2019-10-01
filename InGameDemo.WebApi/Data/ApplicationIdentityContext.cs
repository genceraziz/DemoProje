using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InGameDemo.WebApi.Data
{
    /// <summary>
    /// Asp.Net Identity context sınıfı.
    /// </summary>
    public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options) : base(options)
        {
            
        }
    }
}
