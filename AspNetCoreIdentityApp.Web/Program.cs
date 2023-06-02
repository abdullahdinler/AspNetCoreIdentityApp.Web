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
    // Burada CookieBuilder s�n�f� ile cookie ayarlar�n� yapabiliriz.
    var cookieBuilder = new CookieBuilder { Name = "AspNetCoreIdentityApp" };
    opt.Cookie = cookieBuilder; // Cookie ayarlar�n� yap�yoruz.
    opt.LoginPath = new PathString("/Home/SignIn"); // Kullan�c� giri� yapmadan �nceki sayfa. (Login sayfas�)
    opt.LogoutPath = new PathString("/Member/LogOut"); // Kullan�c� ��k�� yapt�ktan sonra y�nlendirilece�i sayfa.
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(5); // Cookie s�resi
    opt.SlidingExpiration = true; // Cookie s�resi uzat�ls�n m�?


    //opt.Cookie.Domain = "localhost"; // Cookie domain
    //opt.Cookie.HttpOnly = true; // Cookie'ye javascript ile eri�ilemesin mi?
    //opt.Cookie.SameSite = SameSiteMode.Strict; // Cookie'ye sadece kendi domainimizden eri�ilebilir.
    //opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Cookie'ye sadece https �zerinden eri�ilebilir.
    //opt.AccessDeniedPath = "/Home/AccessDenied"; // Kullan�c�n�n yetkisi olmayan bir sayfaya eri�meye �al��mas� durumunda y�nlendirilece�i sayfa.
    //opt.ReturnUrlParameter = "returnUrl"; // Kullan�c�n�n yetkisi olmayan bir sayfaya eri�meye �al��mas� durumunda y�nlendirilece�i sayfa.
   
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
app.UseAuthentication(); // Cookie'lerin kontrol� i�in kullan�l�r. ve kullan�c� giri� yapm�� m� diye kontrol eder.
app.UseAuthorization(); // Yetkilendirme i�in kullan�l�r. 

// Burada Area'lar i�in route tan�mlamas� yap�yoruz.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
