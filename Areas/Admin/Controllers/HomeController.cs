using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserList()
        {
            return View();
        }
    }
}
