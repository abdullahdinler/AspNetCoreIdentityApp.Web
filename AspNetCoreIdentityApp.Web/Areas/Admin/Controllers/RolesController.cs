using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id) ?? throw new Exception("Role not found");

            TempData["success"] = "Role deleted successfully";
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(RolesController.Index));

        }


        [HttpGet]
        public async Task<IActionResult> AssignRole(string id)
        {
            ViewBag.UserId = id;
            var user = await _userManager.FindByIdAsync(id) ?? throw new Exception("User not found");
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var roleAssignViewModel = new List<AssignRoleToUserViewModel>();

            foreach (var role in allRoles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel()
                {
                    Id = role.Id,
                    Name = role.Name!,
                    HasRole = userRoles.Contains(role.Name!)

                };

                roleAssignViewModel.Add(assignRoleToUserViewModel);
            }


            return View(roleAssignViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, List<AssignRoleToUserViewModel> model)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found");

            foreach (var role in model)
            {
                if (role.HasRole)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }
    }
}
