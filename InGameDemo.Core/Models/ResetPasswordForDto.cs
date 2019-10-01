using System.ComponentModel.DataAnnotations;

namespace InGameDemo.Core.Models
{
    public class ResetPasswordForDto
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz")]
        [StringLength(10, ErrorMessage = "Şifreniz en az 7 en fazla 10 karakter uzunluğunda olabilir", MinimumLength = 7)]
        [Display(Name = "Şifre")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz")]
        [StringLength(10, ErrorMessage = "Şifreniz en az 7 en fazla 10 karakter uzunluğunda olabilir", MinimumLength = 7)]
        [Display(Name = "Şifre Tekrar")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor. Lütfen kontrol ediniz")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
