using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize(Policy = "Permissions.Order.Read")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
