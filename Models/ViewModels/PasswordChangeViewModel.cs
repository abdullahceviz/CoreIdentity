using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Eski Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Eski Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakterli olabilir.")]
        public string PasswordOld { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakterli olabilir.")]
        public string PasswordNew { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni  Şifre Tekrar alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifre aynı değildir.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakterli olabilir.")]
        public string? PasswordNewConfirm { get; set; } = null!;
    }
}
