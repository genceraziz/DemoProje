using System.ComponentModel.DataAnnotations;

namespace InGameDemo.Core.Models
{
    public class LoginForDto
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz")]
        [StringLength(10, ErrorMessage = "Şifreniz en az 7 en fazla 10 karakter uzunluğunda olabilir", MinimumLength = 7)]
        [Display(Name = "Şifre")]
        [UIHint("password")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
