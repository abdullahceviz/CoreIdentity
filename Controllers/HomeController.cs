using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            returnUrl ??= Url.Action("Index", "Home")!;
            var hasUser = await _userManager.FindByEmailAsync(model.Email!);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password!, model.RememberMe, true);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl!);
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriş yapamasınız!" });
                return View();
            }
            ModelState.AddModelErrorList(new List<string>() { $"Email veya şifre yanlış" });
            ModelState.AddModelErrorList(new List<string> { $"(Başarısız giriş sayısı ={await _userManager.GetAccessFailedCountAsync(hasUser)})" });
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var identityresult = await _userManager.CreateAsync(new()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone

            }, request.PasswordConfirm!);
            if (identityresult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıtla gerçekleşmiştir.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }
            ModelState.AddModelErrorList(identityresult.Errors.Select(x => x.Description).ToList());
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel resetPasswordViewModel)
        {
            var hasUser = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email!); 
            if(hasUser == null) 
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine ait kullanıcı bulunamamıştır.");
                return View();
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwwordResetLink = Url.Action("ResetPassword","Home",new { userId = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);
            await _emailService.SendResetPasswordEmail(passwwordResetLink!, hasUser.Email!);
            TempData["SuccessMessage"] = "Şifre yenileme linki, eposta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];
            if(userId == null || token==null)
            {
                throw new Exception("Bir hata meydana geldi");
            }
            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulanamamıştır");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(hasUser,token!.ToString()!,resetPasswordViewModel.Password!);
            if(result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz yenilenmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x=>x.Description).ToList());
            }

            return View();
        }
    }
}