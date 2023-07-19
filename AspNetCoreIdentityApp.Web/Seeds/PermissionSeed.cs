using System.Security.Claims;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using AspNetCoreIdentityApp.Web.Permissions;
namespace AspNetCoreIdentityApp.Web.Seeds;

public class PermissionSeed
{
    public static async Task Seed(RoleManager<AppRole> roleManager)
    {
        var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
        var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
        var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");

        if (!hasBasicRole)
        {
            await roleManager.CreateAsync(new AppRole { Name = "BasicRole" });

            var basicRole = (await roleManager.FindByNameAsync("BasicRole"))!;

            await AddReadPermission(basicRole, roleManager);
        }

        if (!hasAdvancedRole)
        {
            await roleManager.CreateAsync(new AppRole { Name = "AdvancedRole" });
            var advancedRole = (await roleManager.FindByNameAsync("AdvancedRole"))!;
            await AddReadPermission(advancedRole, roleManager);
            await AddCreatePermission(advancedRole, roleManager);
        }

        if (!hasAdminRole)
        {
            await roleManager.CreateAsync(new AppRole { Name = "AdminRole" });
            var adminRole = (await roleManager.FindByNameAsync("AdminRole"))!;
            await AddReadPermission(adminRole, roleManager);
            await AddCreatePermission(adminRole, roleManager);
            await AddUpdatePermission(adminRole, roleManager);
            await AddDeletePermission(adminRole, roleManager);
        }



    }

    public static async Task AddReadPermission(AppRole basicRole, RoleManager<AppRole> roleManager)
    {
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Catalog.ListCatalogs));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Stock.ListStocks));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Order.ListOrders));
    }

    public static async Task AddCreatePermission(AppRole basicRole, RoleManager<AppRole> roleManager)
    {
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Catalog.CreateCatalog));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Order.CreateOrder));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Stock.CreateStock));
    }

    public static async Task AddUpdatePermission(AppRole basicRole, RoleManager<AppRole> roleManager)
    {
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Catalog.EditCatalog));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Order.EditOrder));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Stock.ListStocks));
    }

    public static async Task AddDeletePermission(AppRole basicRole, RoleManager<AppRole> roleManager)
    {
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Catalog.DeleteCatalog));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Order.DeleteOrder));
        await roleManager.AddClaimAsync(basicRole, new Claim("Permission", Permission.Stock.DeleteStock));
    }


}

