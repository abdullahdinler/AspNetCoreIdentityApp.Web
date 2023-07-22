using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordNoContainUserName", Description = "Şifre alanı kullanıcı adı içeremez."});
            }

            if(password.ToLower().Contains("1234"))
            {
                errors.Add(new IdentityError { Code = "PasswordNoContain1234", Description = "Şifre alanı ardışık sayı içeremez."});
            }

            if (password.ToLower().Contains(user.Email!.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordNoContainEmail", Description = "Şifre alanı e-posta adresi içeremez."});
            }

            return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}
