using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        public string Password { get; set; }


        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare(otherProperty: nameof(Password), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş geçilemez.")]
        public string PasswordConfirm { get; set; }
    }
}
