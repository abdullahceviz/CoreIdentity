using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Seeds
{
    public static class PermissionsSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });
                var basicRole = await roleManager.FindByNameAsync("BasicRole");
                await roleManager.AddClaimAsync(basicRole!,
                    new Claim("Permissions", PermissionsRoot.Permissions.Stock.Read));
                await roleManager.AddClaimAsync(basicRole!,
                    new Claim("Permissions", PermissionsRoot.Permissions.Order.Read));
                await roleManager.AddClaimAsync(basicRole!,
                    new Claim("Permissions", PermissionsRoot.Permissions.Catalog.Read));
            }
        }
    }
}
