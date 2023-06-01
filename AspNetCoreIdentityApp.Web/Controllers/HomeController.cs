using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SignIn()
        {



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            // CreateAsync metodu ile kullanıcı oluşturulur. Önce user daha sonra password eklenir. Password has için ayrı eklenir.
            var IdentityResult = await _userManager.CreateAsync(new()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            }, model.PasswordConfirm);

            if (IdentityResult.Succeeded)
            {
                TempData["Message"] = "Kayıt Başarılı bir şekilde oluşturuldu.";
                return RedirectToAction(nameof(SignUp));
            }

            // AddModelErrorExtension ile hata mesajları ModelStateDictionary sınıfına eklenir.
            ModelState.AddModelErrorExtension(IdentityResult.Errors.Select(x => x.Description).ToList());
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var returnUrl1 = returnUrl ?? Url.Action(nameof(Index));
            
            // FindByEmailAsync ile kullanıcı var mı yok mu kontrol edilir.
            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı");
                return View();
            }

            // PasswordSignInAsync ile kullanıcı girişi yapılır. Email ve şifre ile giriş yapılır. RememberMe ile kullanıcı bilgileri hatırlanır. 
            var result = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                return Redirect(returnUrl1);
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Hesabınız 6 dakika süreyle kilitlendi.");
                return View();
            }

            // GetAccessFailedCountAsync ile kullanıcının giriş yapma sayısı kontrol edilir.
            var failedAttempts = await _userManager.GetAccessFailedCountAsync(hasUser);

            // AddModelErrorExtension ile hata mesajları ModelStateDictionary sınıfına eklenir.
            ModelState.AddModelErrorExtension(new List<string>() {"Email veya şifre hatalı.", $"Başarısız giriş sayısı: {failedAttempts}"});
                        
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}