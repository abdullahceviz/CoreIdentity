using AspNetCoreIdentityApp.Web.ClaimProvider;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Models.OptionsModels;
using AspNetCoreIdentityApp.Web.Requierments;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddIdentityWithExtensions();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequierementHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ankara");
    });
    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequierement());
    });
});
builder.Services.ConfigureApplicationCookie(options =>
{
    CookieBuilder cookieBuilder = new()
    {
        Name = "UdemyAppCookie"
    };
    options.LoginPath = new PathString("/Home/Signin");
    options.LogoutPath = new PathString("/Member/Logout");
    options.AccessDeniedPath = new PathString("/Member/AccessDenied");
    options.Cookie = cookieBuilder;
    //60 g�n cookie tut
    options.ExpireTimeSpan = TimeSpan.FromDays(60);
    //cookie yenile
    options.SlidingExpiration = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
