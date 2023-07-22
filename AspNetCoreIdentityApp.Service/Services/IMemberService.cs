using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreIdentityApp.Service.Services
{
    public interface IMemberService
    {
        Task<UserViewModel> GetUserViewModelAsync(string userName);
        Task LogOut();
        Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(string userName, string password);
        SelectList GenderList();
        Task<UserEditViewModel> GetUserEditViewModelAsync(string userName);

        Task<(bool, IEnumerable<IdentityError>)> UpdateUserAsync(UserEditViewModel model, string userName);

        List<ClaimViewModel> GetClaimsViewModel(ClaimsPrincipal principal);

    }
}
