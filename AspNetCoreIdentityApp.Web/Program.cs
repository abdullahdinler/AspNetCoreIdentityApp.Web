using AspNetCoreIdentityApp.Web.ClaimProviders;
using AspNetCoreIdentityApp.Web.Context;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.OptionModels;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sql Server Connection
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

// Burada wwwroot dosyasýna heryerden eriþmek için IFileProvider arayüzünü kullanarak wwwroot eriþmemizi saðlayacak.
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

// Identity Configuration
builder.Services.AddIdentityWithExtention();

// Burada appsettings içindeki EmailSetting okuyoruz. 
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Burada EmailService sýnýfýný kullanacaðýmýzý belirtiyoruz. Yaþam döngüsü Scoped olacak.
builder.Services.AddScoped<IEmailService, EmailService>();

// Burada IClaimsTransformation arayüzünü kullanarak UserClaimProvider sýnýfýný kullanacaðýmýzý belirtiyoruz.
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CityPolicy", policy =>
    {
        policy.RequireClaim("city", new List<string> {"Mardin", "Ýnstanbul"});
    });
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
