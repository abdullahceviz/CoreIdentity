using AspNetCoreIdentityApp.Web.Models;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExtensions(this IServiceCollection services)
        {
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

            }).AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
