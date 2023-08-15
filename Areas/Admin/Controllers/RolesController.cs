using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityApp.Web.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();
            return View(roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel roleCreateViewModel)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = roleCreateViewModel.Name });
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            return RedirectToAction(nameof(RolesController.Index));
        }
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = (await _roleManager.FindByIdAsync(id))! ?? throw new Exception("Güncellenecek rol bulunamamıştır.");
            return View(new RoleUpdateViewModel() { Id =roleToUpdate.Id, Name=roleToUpdate.Name!});
        }
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel roleUpdateViewModel) 
        {
            var roleToUpdate = (await _roleManager.FindByIdAsync(roleUpdateViewModel.Id)) ?? throw new Exception("Güncellenecek rol bulunamamıştır.");
            roleToUpdate.Name = roleUpdateViewModel.Name;
            await _roleManager.UpdateAsync(roleToUpdate);
            ViewData["SuccessMessage"] = "Rol bilgisi güncellenmiştir.";
            return View();
        }
    }
}
