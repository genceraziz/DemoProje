using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InGameDemo.Core.Models
{
    public class UserRoleFormForDto
    {
        [Required(ErrorMessage = "Lütfen kullanıcı seçiniz")]
        [Display(Name = "Kullanıcı")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Lütfen rol seçiniz")]
        [Display(Name = "Rol")]
        public string Role { get; set; }

        public IEnumerable<UserForDto> Users { get; set; }
        public IEnumerable<RoleForDto> Roles { get; set; }
    }
}
