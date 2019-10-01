using System.ComponentModel.DataAnnotations;

namespace InGameDemo.Core.Models
{
    public class ForgotPasswordForDto
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
    }
}
