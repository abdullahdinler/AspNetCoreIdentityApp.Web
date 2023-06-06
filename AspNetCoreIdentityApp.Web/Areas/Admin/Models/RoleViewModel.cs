using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; }
    }
}
