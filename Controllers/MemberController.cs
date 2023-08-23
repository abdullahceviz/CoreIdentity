using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Security.Claims;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Service.Services;
using Azure.Core;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private readonly IMemberService _memberService;
        private string userName => User.Identity!.Name!;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IMemberService memberService = null!)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _memberService.GetUserViewModelByUserNameAsync(userName));
        }
        public async Task Logout()
        {
            await _memberService.LogoutAsync();
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
            if (! await _memberService.CheckPasswordAync(userName,passwordChangeViewModel.PasswordOld))
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz yanlış");
                return View();
            }
            var (isSuccess,errors) = await _memberService.ChangePasswordAsync(userName, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew);
            if (!isSuccess)
            {
                ModelState.AddModelErrorList(errors!);
                return View();
            }
           
            TempData["SuccessMessage"] = "Şifre değiştirme işleminiz başarıyla gerçekleşmiştir.";
            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.gender = _memberService.GetGenderSelectList();
            
            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel userEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            var (isSuccess, errors) = await _memberService.EditUserAsync(userEditViewModel, userName);

            if (!isSuccess)
            {
                ModelState.AddModelErrorList(errors!);
                return View();
            }

            TempData["SuccessMessage"] = "Üye bilgileri başarıyla değiştirilmiştir";

            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }
        public IActionResult AccessDenied()
        {
            ViewBag.message = "Bu sayfayı görmeye yetkiniz yoktur. Yetki almak için yöneticiniz ile görüşebilirsiniz.";
            return View();
        }
        public IActionResult Claims()
        {
            return View(_memberService.GetClaims(User));
        }
        [Authorize(Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult AnkaraPage()
        {
            return View();
        }
        [Authorize(Policy = "ExchangePolicy")]
        [HttpGet]
        public IActionResult ExchangePage()
        {
            return View();
        }
        [Authorize(Policy = "ViolencePolicy")]
        [HttpGet]
        public IActionResult ViolencePage()
        {
            return View();
        }
    }
}
    