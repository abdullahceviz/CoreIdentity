using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityApp.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "admin,role-action")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();
            return View(roles);
        }
        [Authorize(Roles = "role-action")]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [Authorize(Roles ="role-action")]
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel roleCreateViewModel)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = roleCreateViewModel.Name });
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            TempData["SuccessMessage"] = "Rol oluuşturulmuştur.";
            return RedirectToAction(nameof(RolesController.Index));
        }
        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = (await _roleManager.FindByIdAsync(id))! ?? throw new Exception("Güncellenecek rol bulunamamıştır.");
            return View(new RoleUpdateViewModel() { Id = roleToUpdate.Id, Name = roleToUpdate.Name! });
        }
        [Authorize(Roles = "role-action")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel roleUpdateViewModel)
        {
            var roleToUpdate = (await _roleManager.FindByIdAsync(roleUpdateViewModel.Id)) ?? throw new Exception("Güncellenecek rol bulunamamıştır.");
            roleToUpdate.Name = roleUpdateViewModel.Name;
            await _roleManager.UpdateAsync(roleToUpdate);
            ViewData["SuccessMessage"] = "Rol bilgisi güncellenmiştir.";
            return View();
        }
        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = (await _roleManager.FindByIdAsync(id)) ?? throw new Exception("Silinecek rol bulunamamıştır.");
            var result = await _roleManager.DeleteAsync(roleToDelete);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());
            }
            TempData["SuccessMessage"] = "Rol silinmiştir.";
            return RedirectToAction(nameof(RolesController.Index));
        }
        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var curremtUser = await _userManager.FindByIdAsync(id);
            ViewBag.UserId = curremtUser!.Id;
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModelList = new List<AssignToUserViewModel>();
            var userRoles = await _userManager.GetRolesAsync(curremtUser!);
            foreach (var role in roles)
            {
                var assignRoleToUser = new AssignToUserViewModel() { Id = role.Id, Name = role.Name! };
                if (userRoles.Contains(role.Name!))
                {
                    assignRoleToUser.Exist = true;
                }
                roleViewModelList.Add(assignRoleToUser);
            }
            return View(roleViewModelList);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId,List<AssignToUserViewModel> assignToUserViewModelList)
        {
            var userToAssignToRoles = await _userManager.FindByIdAsync(userId);
            foreach(var role in assignToUserViewModelList)
            {
                //await _userManager.AddToRoleAsync(userToAssignToRoles!, role.Name);
                if(role.Exist)
                {
                    await _userManager.AddToRoleAsync(userToAssignToRoles!, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userToAssignToRoles!, role.Name);
                }
            }
            
            return RedirectToAction(nameof(HomeController.UserList),"Home");
        }
    }
}
