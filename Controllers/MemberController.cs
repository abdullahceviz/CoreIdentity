﻿using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserViewModel
            {
                Email = currentUser!.Email,
                PhoneNumber = currentUser!.PhoneNumber,
                UserName = currentUser!.UserName
            };
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
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz yanlış");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser!, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew);
            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
                return View();
            }
            await _userManager.UpdateSecurityStampAsync(currentUser!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser!, passwordChangeViewModel.PasswordNew, true, false);
            TempData["SuccessMessage"] = "Şifre değiştirme işleminiz başarıyla gerçekleşmiştir.";
            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender
            };
            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel userEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(); 
            }
            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            currentUser.UserName = userEditViewModel.UserName;
            currentUser.Email = userEditViewModel.Email;
            currentUser.BirthDate = userEditViewModel.BirthDate;
            currentUser.City = userEditViewModel.City;
            currentUser.Gender = userEditViewModel.Gender;
            currentUser.PhoneNumber = userEditViewModel.Phone;
            if(userEditViewModel.Picture != null && userEditViewModel.Picture.Length>=0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                string randomFileName = $"{Guid.NewGuid().ToString()}{ Path.GetExtension(userEditViewModel.Picture.FileName)}";
                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures")
                    .PhysicalPath!, randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await userEditViewModel.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;

            }
            var updateToUserResult = await _userManager.UpdateAsync(currentUser);
            if(!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
                return View();
            }
            await _userManager.UpdateAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);
            TempData["SuccessMessage"] = "Üye bilgileri başarıyla değiştirilmiştir.";
            
            return View(userEditViewModel);
        }
    }
}
    