using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        public string? Name { get; set; }
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }
        public string? PictureUrl { get; set; }
    }
}
