using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleListViewModel { Id = x.Id, Name = x.Name! }).ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var role = new AppRole
            {
                Name = roleViewModel.Name
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorExtension(result.Errors.Select(x => x.Description).ToList());
            }
            return RedirectToAction(nameof(RolesController.Index));
        }

        [HttpGet]
        public async Task<IActionResult> RoleEdit(string id)
        {
            // Burada id null gelirse hata fırlatırız.
            var role = await _roleManager.FindByIdAsync(id) ?? throw new Exception("Role not found");
            var roleEditViewModel = new RoleEditViewModel
            {
                Id = role.Id,
                Name = role.Name!
            };


            return View(roleEditViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditViewModel editViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var role = await _roleManager.FindByIdAsync(editViewModel.Id) ?? throw new Exception("Role not found");
            role.Name = editViewModel.Name;
            await _roleManager.UpdateAsync(role);
            TempData["success"] = "Role updated successfully";
            return RedirectToAction(nameof(RolesController.Index));

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id) ?? throw new Exception("Role not found");

            TempData["success"] = "Role deleted successfully";
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(RolesController.Index));

        }
    }
}
