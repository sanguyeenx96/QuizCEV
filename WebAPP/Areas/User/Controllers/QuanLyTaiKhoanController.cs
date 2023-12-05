using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewModels.Users.Request;
using WebAPP.Services;

namespace WebAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Policy = "userpolicy")]
    public class QuanLyTaiKhoanController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public QuanLyTaiKhoanController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = "Quản lý tài khoản";

            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> CheckPassword(string password)
        {
            var request = new UserCheckPasswordRequest()
            {
                pwd = password
            };
            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };
            var result = await _userApiClient.CheckPassWord(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });

        }
        [HttpPost]
        public async Task<IActionResult> UserResetPassword(string newPassword)
        {
            var request = new UserResetPasswordRequest()
            {
                Password = newPassword
            };
            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };
            var result = await _userApiClient.ResetPassword(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
    }
}
