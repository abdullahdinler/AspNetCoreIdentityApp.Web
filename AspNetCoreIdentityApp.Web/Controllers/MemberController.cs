using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize] // Bu controller'a giriş yapmış kullanıcılar erişebilir.
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel()
            {
                Name = user!.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PictureUrl = user.Picture
            };

            return View(userViewModel);
        }

        [HttpGet]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            if (currentUser == null)
            {
                return RedirectToAction("SignIn", "Home");
            }

            var result = await _userManager.CheckPasswordAsync(currentUser, model.PasswordOld);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, model.PasswordOld, model.PasswordNewConfirm);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorExtension(resultChangePassword.Errors.Select(x => x.Description).ToList());
            }
            await _userManager.UpdateSecurityStampAsync(currentUser); // Kullanıcının güvenlik damgasını güncelliyoruz.
            await _signInManager.PasswordSignInAsync(currentUser, model.PasswordNewConfirm, true, false); // Kullanıcıyı sistemde tekrar giriş yaptırıyoruz.
            await _signInManager.SignOutAsync(); // Kullanıcıyı sistemden çıkartıyoruz.



            TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
            return View();
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(Gender)));

            var user = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                City = user.City,
            };

            return View(userEditViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            currentUser.UserName = model.UserName;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.BirthDate = model.BirthDate;
            currentUser.Gender = model.Gender;
            currentUser.City = model.City;

            if (model.Picture != null || model.Picture.Length > 0)
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
                ModelState.AddModelErrorExtension(updateUserResult.Errors.Select(x=>x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["success"] = "Bilgileriniz başarılı birşekilde güncellendi.";
            return RedirectToAction(nameof(UserEdit));

        }


        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.Message = "Bu sayfaya erişim yetkiniz yoktur.";

            return View();
        }
    }
}
