using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize] // Bu controller'a giriş yapmış kullanıcılar erişebilir.
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel()
            {
                Name = user!.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
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
               ModelState.AddModelErrorExtension(resultChangePassword.Errors.Select(x=>x.Description).ToList());
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
    }
}
