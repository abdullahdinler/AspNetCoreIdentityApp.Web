using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspNetCoreIdentityApp.Web.Areas.Admin.Models;

namespace AspNetCoreIdentityApp.Web.Context;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<AspNetCoreIdentityApp.Web.Areas.Admin.Models.RoleEditViewModel> RoleEditViewModel { get; set; } = default!;
}

