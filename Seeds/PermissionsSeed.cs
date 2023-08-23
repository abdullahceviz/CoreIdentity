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
            var hasAdvencedRole = await roleManager.RoleExistsAsync("AdvencedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");
            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });
                var basicRole = await roleManager.FindByNameAsync("BasicRole");
                await AddReadPermission(basicRole!, roleManager);
            }
            if (!hasAdvencedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvencedRole" });
                var advencedRole = await roleManager.FindByNameAsync("AdvencedRole");
                await AddReadPermission(advencedRole!, roleManager);
                await AddUpdateAndCreatePermission(advencedRole!, roleManager);
            }
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });
                var adminRole = await roleManager.FindByNameAsync("AdminRole");
                await AddReadPermission(adminRole!, roleManager);
                await AddUpdateAndCreatePermission(adminRole!, roleManager);
                await AddDeletePermission(adminRole!, roleManager);
            }
        }
        public static async Task AddReadPermission(AppRole role,RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role!,
                    new Claim("Permissions", Core.PermissionsRoot.Permissions.Stock.Read));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Order.Read));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Catalog.Read));
        }
        public static async Task AddUpdateAndCreatePermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role!,
                    new Claim("Permissions", Core.PermissionsRoot.Permissions.Stock.Create));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Order.Create));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Catalog.Create));
            await roleManager.AddClaimAsync(role!,
                    new Claim("Permissions", Core.PermissionsRoot.Permissions.Stock.Update));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Order.Update));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Catalog.Update));
        }
        public static async Task AddDeletePermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role!,
                    new Claim("Permissions", Core.PermissionsRoot.Permissions.Stock.Delete));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Order.Delete));
            await roleManager.AddClaimAsync(role!,
                new Claim("Permissions", Core.PermissionsRoot.Permissions.Catalog.Delete));
        }
    }
}
