using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullancı Ad alanı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adi :")]
        public string? UserName { get; set; }
        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string? Phone { get; set; }
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Şehir")]
        public string? City { get; set; }
        [Display(Name = "ProfilResmi")]
        public IFormFile? Picture { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender? Gender { get; set; }
    }
}
