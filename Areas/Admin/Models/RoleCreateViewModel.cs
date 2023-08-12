using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role ismi alanı boş bırakılamaz")]
        [Display(Name = "Role ismim :")]
        public string Name { get; set; } = null!;
    }
}
