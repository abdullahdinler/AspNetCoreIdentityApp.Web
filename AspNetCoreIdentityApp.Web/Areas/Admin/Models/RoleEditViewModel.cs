using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class RoleEditViewModel
    {
        public string Id { get; set; } = null!;
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; } = null!;
    }
}
