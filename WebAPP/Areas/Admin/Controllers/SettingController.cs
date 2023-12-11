using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Common;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class SettingController : Controller
    {
        private readonly ISettingApiClient _settingApiClient;
        public SettingController(ISettingApiClient settingApiClient)
        {
            _settingApiClient = settingApiClient;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.thisPage = "Cài đặt chức năng";
            var listSetting = await _settingApiClient.GetAll();
            return View(listSetting.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeSetting(int id)
        {
            var result = await _settingApiClient.ChangeStatus(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
    }
}
