using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel() { }
        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }



        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru bir biçimde değil.")]
        [Required(ErrorMessage = "Email alanı boş geçilemez.")]
        public string Email { get; set; }


        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
        
    }
}
