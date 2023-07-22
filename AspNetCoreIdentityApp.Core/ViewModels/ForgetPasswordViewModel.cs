using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru bir biçimde değil.")]
        [Required(ErrorMessage = "Email alanı boş geçilemez.")]
        public string Email { get; set; }
    }
}
