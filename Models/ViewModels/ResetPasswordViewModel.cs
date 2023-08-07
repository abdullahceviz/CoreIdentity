using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Tekrar alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        public string? PasswordConfirm { get; set; }
    }
}
