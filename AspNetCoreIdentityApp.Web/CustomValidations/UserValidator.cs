using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>(); // Burada hata listesi oluşturuyoruz.
            var isDigit = int.TryParse(user.UserName[0].ToString(), out _); // Kullanıcı adının ilk harfini alıyoruz ve sayıya çevirmeye çalışıyoruz. Eğer sayıya çevirebilirsek isDigit değişkenine true değerini atıyoruz.

            if (isDigit)
            {
                errors.Add(new IdentityError() { Code = "UserNameContainFirstLetterDigit", Description = "Kullanıcı adı bir sayı ile başlıyamaz" }); // Eğer isDigit değişkeni true ise hata listesine yeni bir hata ekliyoruz.
            }

            return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success); // Eğer hata listesinde hata varsa hata listesini döndürüyoruz. Eğer hata yoksa IdentityResult.Success döndürüyoruz.

        }
    }
}
