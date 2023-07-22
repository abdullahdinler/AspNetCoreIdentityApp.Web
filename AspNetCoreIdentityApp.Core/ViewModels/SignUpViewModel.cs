using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel() { }

        public SignUpViewModel(string userName, string email, string password, string phoneNumber)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage ="Kullanıcı Ad alanı boş geçilemez.")]
        public string UserName { get; set; }


        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru bir biçimde değil.")]
        [Required(ErrorMessage = "Email alanı boş geçilemez.")]
        public string Email { get; set; }


        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        public string Password { get; set; }   
        

        [Display(Name = "Şifre Tekrar")]
        [Compare(otherProperty: nameof(Password), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş geçilemez.")]
        public string PasswordConfirm { get; set; }


        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon alanı boş geçilemez.")]
        public string PhoneNumber { get; set; }
    }
}
