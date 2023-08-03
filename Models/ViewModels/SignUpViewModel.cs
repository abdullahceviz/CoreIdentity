using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.Models.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
            
        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }
        [Required(ErrorMessage ="Kullancı Ad alanı boş bırakılamaz.")]
        [Display(Name="Kullanıcı Adi :")]
        public string UserName { get; set; }
        [Display(Name = "Email :")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage ="Email formatı yanlıştır.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Şifre :")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Şifre Tekrar alanı boş bırakılamaz.")]
        [Display(Name = "Şifre Tekrar :")]
        [Compare(nameof(Password),ErrorMessage ="Şifre aynı değildir.")]
        public string PasswordConfirm { get; set; }
    }
}
