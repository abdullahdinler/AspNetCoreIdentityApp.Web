using System;
using System.Text;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
    public class UserRoleListTagHelper : TagHelper
    {
        public string Id { get; set; }
        private readonly UserManager<AppUser> _userManager;
        public UserRoleListTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            var user = await _userManager.FindByIdAsync(Id) ?? throw new Exception("Role not found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var sb = new StringBuilder();

            userRoles.ToList().ForEach(x =>
            {
                sb.Append(@$"<span class='badge badge-secondary'>{x.ToLower()}</span>");
            });
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
