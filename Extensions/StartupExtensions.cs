using AspNetCoreIdentityApp.Web.CustomValidations;
using AspNetCoreIdentityApp.Web.Localization;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExtensions(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                //geçerli karakterler
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz1234567890_";
                //minimimum karakter sayısı
                options.Password.RequiredLength = 6;
                //alfanumeric karakter olsun mu
                options.Password.RequireNonAlphanumeric = false;
                //küçük harf gerekli mi
                options.Password.RequireLowercase = true;
                //büyük harf gerekeli mi
                options.Password.RequireUppercase = false;
                //sayısal karakter gerekli mi
                options.Password.RequireDigit = false;
                //kilitleme mekanizması 3 dakika kilitlesin 
                options.Lockout.DefaultLockoutTimeSpan =TimeSpan.FromSeconds(3);
                //3 kez yanlış girince kilitlesin.
                options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
