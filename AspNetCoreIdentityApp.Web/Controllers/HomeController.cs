using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Service.Services;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            }, model.PasswordConfirm);

            if (!identityResult.Succeeded)
            {
                // AddModelErrorExtension ile hata mesajları ModelStateDictionary sınıfına eklenir.
                ModelState.AddModelErrorExtension(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            #region User Claim Ekleme - Bir sayfaya belirli bir süre erişmek.

            var exchangeExpireClaim = new Claim("ExchangeExpire", DateTime.Now.AddDays(10).ToString(CultureInfo.InvariantCulture));

            var user = await _userManager.FindByNameAsync(model.UserName);
            var addClaim = await _userManager.AddClaimAsync(user, exchangeExpireClaim);

            if (!addClaim.Succeeded)
            {
                ModelState.AddModelErrorExtension(addClaim.Errors.Select(x => x.Description).ToList());
                return View();
            }

            #endregion

            TempData["Message"] = "Kayıt Başarılı bir şekilde oluşturuldu.";
            return RedirectToAction(nameof(SignIn));




        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            returnUrl ??= Url.Action(nameof(Index));

            // FindByEmailAsync ile kullanıcı var mı yok mu kontrol edilir.
            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı");
                return View();
            }


            // PasswordSignInAsync ile kullanıcı girişi yapılır. Email ve şifre ile giriş yapılır. RememberMe ile kullanıcı bilgileri hatırlanır. 
            var result = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Hesabınız 6 dakika süreyle kilitlendi.");
                return View();
            }

            if (!result.Succeeded)
            {
                // GetAccessFailedCountAsync ile kullanıcının giriş yapma sayısı kontrol edilir.
                var failedAttempts = await _userManager.GetAccessFailedCountAsync(hasUser);

                // AddModelErrorExtension ile hata mesajları ModelStateDictionary sınıfına eklenir.
                ModelState.AddModelErrorExtension(new List<string>() { "Email veya şifre hatalı.", $"Başarısız giriş sayısı: {failedAttempts}" });
                return View();
            }

            if (hasUser.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe,
                    new[] {new Claim("birthdate", hasUser.BirthDate.Value.ToString())});
            }

            return Redirect(returnUrl!);


        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Eposta adresi bulunamadı.");
                return View();
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action(nameof(ResetPassword), "Home", new { userId = hasUser.Id, token = passwordResetToken }, Request.Scheme);

            await _emailService.ResetPasswordEmail(hasUser.Email!, passwordResetLink!);

            TempData["Message"] = "Şifre sıfırlama linkiniz eposta adresinize gönderildi";
            return RedirectToAction(nameof(ForgetPassword));
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Token veya kullanıcı bilgisi bulunamadı.");
                return View();
            }


            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            // ResetPasswordAsync methodu ile kullanıcı şifresi değiştirilir.
            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, model.Password);
            if (result.Succeeded)
            {
                TempData["Message"] = "Şifreniz başarılı bir şekilde değiştirildi.";
                return RedirectToAction(nameof(ResetPassword));
            }
            else
            {
                ModelState.AddModelErrorExtension(result.Errors.Select(x => x.Description).ToList());
                return View();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}