using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name = "Eski Şifre")]
        [Required(ErrorMessage = "Eski şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Eski şifre en az 6 karakter olmalıdır.")]
        public string PasswordOld { get; set; }


        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Yeni şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır.")]
        public string PasswordNew { get; set; }


        [Display(Name = "Yeni Şifre Tekrar")]
        [Required(ErrorMessage = "Yeni şifre tekrar alanı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("PasswordNew", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        [MinLength(6, ErrorMessage = "Yeni şifre tekrar en az 6 karakter olmalıdır.")]
        public string PasswordNewConfirm { get; set; }
    }
}
