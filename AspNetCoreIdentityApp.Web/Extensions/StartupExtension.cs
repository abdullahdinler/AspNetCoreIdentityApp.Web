using AspNetCoreIdentityApp.Web.Context;
using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localizations;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Extensions
{
   // Bu sınıfımızda Startup.cs sınıfımızda yazdığımız Identity ayarlarını bir extension metot olarak yazdık.
    public static class StartupExtension
    {
        public static void AddIdentityWithExtention(this IServiceCollection servicesCollection)
        {

            // Bu ayar sayesinde kullanıcıya gönderilen token'ın süresini 5 dakika olarak ayarladık.
            servicesCollection.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(5);
            });


            // Cookie Configuration
            servicesCollection.ConfigureExternalCookie(opt =>
            {
                // Burada CookieBuilder sınıfı ile cookie ayarlarını yapabiliriz.
                var cookieBuilder = new CookieBuilder { Name = "AspNetCoreIdentityApp" };
                opt.Cookie = cookieBuilder; // Cookie ayarlarını yapıyoruz.
                opt.LoginPath = new PathString("/Home/SignIn"); // Kullanıcı giriş yapmadan önceki sayfa. (Login sayfası)
                opt.LogoutPath = new PathString("/Member/LogOut"); // Kullanıcı çıkış yaptıktan sonra yönlendirileceği sayfa.
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(5); // Cookie süresi
                opt.SlidingExpiration = true; // Cookie süresi uzatılsın mı?


                //opt.Cookie.Domain = "localhost"; // Cookie domain
                //opt.Cookie.HttpOnly = true; // Cookie'ye javascript ile erişilemesin mi?
                //opt.Cookie.SameSite = SameSiteMode.Strict; // Cookie'ye sadece kendi domainimizden erişilebilir.
                //opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Cookie'ye sadece https üzerinden erişilebilir.
                opt.AccessDeniedPath = new PathString("/Home/SignIn"); // Kullanıcının yetkisi olmayan bir sayfaya erişmeye çalışması durumunda yönlendirileceği sayfa.
                //opt.ReturnUrlParameter = "returnUrl"; // Kullanıcının yetkisi olmayan bir sayfaya erişmeye çalışması durumunda yönlendirileceği sayfa.

            });


            // Identity ayarlarını burada yazdık. Bu sayede Program.cs sınıfımızda kod kalabalığından kurtulduk.
            servicesCollection.AddIdentity<AppUser, AppRole>(options =>
            {
                //Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;

                // User
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;


                // Signin
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

            }).AddPasswordValidator<PasswordValidator>() // Kendi yazdığımız password validator sınıfını ekledik. Bu sayede password için kendi kurallarımızı yazabiliriz.
                .AddUserValidator<UserValidator>() // Kendi yazdığımız user validator sınıfını ekledik. Bu sayede user için kendi kurallarımızı yazabiliriz.
                .AddErrorDescriber<LocalizationsIdentityErrorDescriber>() // Kendi yazdığımız error describer sınıfını ekledik. Bu sayede hata mesajlarını kendimiz yazabiliriz.
                .AddDefaultTokenProviders() // Token provider ekledik. Bu sayede kullanıcıya token gönderilebilir. Örneğin kullanıcı şifresini unuttuğunda şifre yenileme linki gönderilebilir.
                .AddEntityFrameworkStores<AppDbContext>(); // Entity framework store ekledik. Bu sayede kullanıcı bilgileri veritabanına kaydedilebilir.
        }

       
    }
}
