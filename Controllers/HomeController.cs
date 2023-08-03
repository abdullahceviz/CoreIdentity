﻿using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var identityresult = await _userManager.CreateAsync(new()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone

            }, request.PasswordConfirm);
            if (identityresult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıtla gerçekleşmiştir.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }
            foreach(IdentityError item in identityresult.Errors)
            {
                ModelState.AddModelError(string.Empty,item.Description);
            }
            return View();
        }
    }
}