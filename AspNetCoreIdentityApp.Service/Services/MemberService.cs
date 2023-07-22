using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;

        public MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task<UserViewModel> GetUserViewModelAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            return new UserViewModel()
            {
                Email = currentUser.Email,
                Name = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, currentPassword,newPassword);

            if (!resultChangePassword.Succeeded)
            {
                return (false, resultChangePassword.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser); // Kullanıcının güvenlik damgasını güncelliyoruz.
            await _signInManager.PasswordSignInAsync(currentUser, newPassword, true, false); // Kullanıcıyı sistemde tekrar giriş yaptırıyoruz.
            await _signInManager.SignOutAsync(); // Kullanıcıyı sistemden çıkartıyoruz.
            return (true, null);
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

           return await _userManager.CheckPasswordAsync(currentUser, password);
        }

        public SelectList GenderList()
        {
            return new SelectList(Enum.GetNames(typeof(Gender)));
        }

        public async Task<UserEditViewModel> GetUserEditViewModelAsync(string userName)
        {
            var user = (await _userManager.FindByNameAsync(userName))!;

            return  new UserEditViewModel()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                City = user.City,
            };
        }

        public async Task<(bool, IEnumerable<IdentityError>)> UpdateUserAsync(UserEditViewModel model, string userName)
        {

            var currentUser = (await _userManager.FindByNameAsync(userName))!;
            currentUser.UserName = model.UserName;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.BirthDate = model.BirthDate;
            currentUser.Gender = model.Gender;
            currentUser.City = model.City;


            if (model.Picture != null && model.Picture!.Length > 0)
            {
                // Burada wwwroot klasörün dizisini aldık
                var wwwRootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                // Burada benzersiz bir isim oluşturduk.
                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Picture.FileName)}";

                //Burada verilen "wwwRootFolder" adlı bir klasörün içindeki "images" adlı bir alt klasörün fiziksel yolunu alır. Daha sonra, bu yola, rastgele bir dosya adı olan "randomFileName" eklenir ve sonuç olarak yeni bir dosya yolu oluşturulur. 
                var newPicturePath = Path.Combine(wwwRootFolder!.First(x => x.Name == "images").PhysicalPath!,
                    randomFileName);

                await using var stream = new FileStream(newPicturePath, FileMode.Create);

                await model.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateUserResult = await _userManager.UpdateAsync(currentUser);
            if (!updateUserResult.Succeeded)
            {
                return (false, updateUserResult.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();


            if (model.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(currentUser, true,
                    new[] { new Claim("birthdate", currentUser.BirthDate.Value.ToString()) });
            }
            else
            {
                await _signInManager.SignInAsync(currentUser, true);
            }
            return (true, null);
        }

        public List<ClaimViewModel> GetClaimsViewModel(ClaimsPrincipal principal)
        {
            return principal.Claims.Select(x => new ClaimViewModel()
            {
                Value = x.Value,
                Type = x.Type,
                Issuer = x.OriginalIssuer
            }).ToList();
        }
    }
}
