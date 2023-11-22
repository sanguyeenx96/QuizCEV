using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Users.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }
        public async Task<IActionResult> List(string keyword, int pageIndex =1, int pageSize = 10)
        {
            ViewBag.thisPage = "Danh sách tài khoản";
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = await _userApiClient.GetuserPaging(request);
            return View(result.ResultObj);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.thisPage = "Thêm mới tài khoản";
            return View();
        }
    }
}
