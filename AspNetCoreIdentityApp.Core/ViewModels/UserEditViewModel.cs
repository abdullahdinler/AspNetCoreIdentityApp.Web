using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentityApp.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Ad alanı boş geçilemez.")]
        public string UserName { get; set; } = null!;


        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru bir biçimde değil.")]
        [Required(ErrorMessage = "Email alanı boş geçilemez.")]
        public string Email { get; set; } = null!;


        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon alanı boş geçilemez.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;


        [Display(Name = "Şehir")]
        public string? City { get; set; }


        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }


        [Display(Name = "Cinsiyet")]
        public Gender? Gender { get; set; }



        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile? Picture { get; set; }
    }
}
