using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
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
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }    
        [Display(Name = "Şifre Tekrar")]
        public string PasswordConfirm { get; set; }
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
    }
}
