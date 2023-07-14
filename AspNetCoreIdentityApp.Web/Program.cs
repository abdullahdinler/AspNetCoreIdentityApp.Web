using AspNetCoreIdentityApp.Web.ClaimProviders;
using AspNetCoreIdentityApp.Web.Context;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.OptionModels;
using AspNetCoreIdentityApp.Web.Requirements;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

// Burada wwwroot dosyas�na heryerden eri�mek i�in IFileProvider aray�z�n� kullanarak wwwroot eri�memizi sa�layacak.
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

// Identity Configuration
builder.Services.AddIdentityWithExtention();

// Burada appsettings i�indeki EmailSetting okuyoruz. 
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Burada EmailService s�n�f�n� kullanaca��m�z� belirtiyoruz. Ya�am d�ng�s� Scoped olacak.
builder.Services.AddScoped<IEmailService, EmailService>();

// Burada IClaimsTransformation aray�z�n� kullanarak UserClaimProvider s�n�f�n� kullanaca��m�z� belirtiyoruz.
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();

builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();


builder.Services.AddAuthorization(options =>
{
    // Burada CityPolicy isimli bir policy olu�turuyoruz. Bu policy'yi kullanabilmek i�in kullan�c�n�n city claim'ine sahip olmas� gerekiyor.
    options.AddPolicy("CityPolicy", policy =>
    {
        policy.RequireClaim("city", new List<string> {"Mardin", "�stanbul"});
    });
    // Burada ExchangeExpireRequirement s�n�f�n� kullanaca��m�z� belirtiyoruz.

    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });
    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement(){ThresholdAge = 18});
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
