using AspNetCoreIdentityApp.Web.Context;
using AspNetCoreIdentityApp.Web.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sql Server Connection
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});


// Identity Configuration
builder.Services.AddIdentityWithExtention();

// Cookie Configuration
builder.Services.ConfigureExternalCookie(opt =>
{
    // Burada CookieBuilder sýnýfý ile cookie ayarlarýný yapabiliriz.
    var cookieBuilder = new CookieBuilder { Name = "AspNetCoreIdentityApp" };
    opt.Cookie = cookieBuilder; // Cookie ayarlarýný yapýyoruz.
    opt.LoginPath = new PathString("/Home/SignIn"); // Kullanýcý giriþ yapmadan önceki sayfa. (Login sayfasý)
    opt.LogoutPath = new PathString("/Member/LogOut"); // Kullanýcý çýkýþ yaptýktan sonra yönlendirileceði sayfa.
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(5); // Cookie süresi
    opt.SlidingExpiration = true; // Cookie süresi uzatýlsýn mý?


    //opt.Cookie.Domain = "localhost"; // Cookie domain
    //opt.Cookie.HttpOnly = true; // Cookie'ye javascript ile eriþilemesin mi?
    //opt.Cookie.SameSite = SameSiteMode.Strict; // Cookie'ye sadece kendi domainimizden eriþilebilir.
    //opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Cookie'ye sadece https üzerinden eriþilebilir.
    //opt.AccessDeniedPath = "/Home/AccessDenied"; // Kullanýcýnýn yetkisi olmayan bir sayfaya eriþmeye çalýþmasý durumunda yönlendirileceði sayfa.
    //opt.ReturnUrlParameter = "returnUrl"; // Kullanýcýnýn yetkisi olmayan bir sayfaya eriþmeye çalýþmasý durumunda yönlendirileceði sayfa.
   
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Cookie'lerin kontrolü için kullanýlýr. ve kullanýcý giriþ yapmýþ mý diye kontrol eder.
app.UseAuthorization(); // Yetkilendirme için kullanýlýr. 

// Burada Area'lar için route tanýmlamasý yapýyoruz.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
