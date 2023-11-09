using Microsoft.AspNetCore.Mvc;

namespace WebAPP.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.thisPage = "Trang chủ";
            return View();
        }
    }
}
