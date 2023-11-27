using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.thisPage = "Trang chủ";
            return View();
        }
    }
}
