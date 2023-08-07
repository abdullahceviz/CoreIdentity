using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserViewModel { 
                Email = currentUser!.Email,
                PhoneNumber = currentUser!.PhoneNumber,
                UserName = currentUser!.UserName };
            return View(userViewModel);
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser!, passwordChangeViewModel.PasswordOld);
            if(!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz yanlış");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync (currentUser!, passwordChangeViewModel.PasswordOld,passwordChangeViewModel.PasswordNew);
            if (!resultChangePassword.Succeeded) {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x=>x.Description).ToList());
                return View();
            }
            await _userManager.UpdateSecurityStampAsync(currentUser!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser!, passwordChangeViewModel.PasswordNew, true, false);
            TempData["SuccessMessage"] = "Şifre değiştirme işleminiz başarıyla gerçekleşmiştir.";
            return View();
        }
    }
}
