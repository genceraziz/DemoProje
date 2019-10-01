using System.ComponentModel.DataAnnotations;

namespace InGameDemo.Core.Models
{
    /// <summary>
    /// Kayıt olmak için kullanılan model.
    /// </summary>
    public class RegisterForDto
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz")]
        [Display(Name = "Şifre")]
        [StringLength(10, ErrorMessage = "Şifreniz en az 7 en fazla 10 karakter uzunluğunda olabilir", MinimumLength = 7)]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen e-mail bilginizi giriniz")]
        [Display(Name = "E-Mail")]
        [UIHint("email")]
        public string Email { get; set; }
    }
}
