using System.Security.Claims;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityApp.Service.Services;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize] // Bu controller'a giriş yapmış kullanıcılar erişebilir.
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private readonly IMemberService _memberService;
        public string UserName => User.Identity!.Name!;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IMemberService memberService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _memberService.GetUserViewModelAsync(UserName));
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



            var result = await _memberService.CheckPasswordAsync(UserName, model.PasswordOld);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
            }

            var (isSuccess, errors) = await _memberService.ChangePasswordAsync(UserName, model.PasswordOld, model.PasswordNew);

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, errors!.First().Description);
            }


            TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
            return View();
        }

        public async Task LogOut()
        {
            await _memberService.LogOut();
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList = _memberService.GenderList();

            var userEditViewModel = await _memberService.GetUserEditViewModelAsync(UserName);

            return View(userEditViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isSuccess, errors) = await _memberService.UpdateUserAsync(model, UserName);
            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, errors!.First().Description); return View(model);
            }

            TempData["success"] = "Bilgileriniz başarılı birşekilde güncellendi.";
            return RedirectToAction(nameof(UserEdit));

        }


        [HttpGet]
        public IActionResult Claims()
        {
            return View(_memberService.GetClaimsViewModel(User));
        }

        [HttpGet]
        [Authorize(Policy = "CityPolicy")]
        public IActionResult CityPage()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult ExchangePage()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "PermissionOrderPolicy")]
        public IActionResult PermissionOrderPage()
        {
            return View();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.Message = "Bu sayfaya erişim yetkiniz yoktur.";

            return View();
        }
    }
}
