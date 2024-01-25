using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewModels.Read.ReadPost;
using ViewModels.Read.ReadResult;
using WebAPP.Services;

namespace WebAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Policy = "userpolicy")]
    public class PostController : Controller
    {
        private readonly IReadCategoryApiClient _readCategoryApiClient;
        private readonly IReadPostApiClient _readPostApiClient;
        private readonly IReadResultApiClient _readResultApiClient;
        public PostController(IReadCategoryApiClient readCategoryApiClient, IReadPostApiClient readPostApiClient, IReadResultApiClient readResultApiClient)
        {
            _readCategoryApiClient = readCategoryApiClient;
            _readPostApiClient = readPostApiClient;
            _readResultApiClient = readResultApiClient;
        }
        public async Task<IActionResult> List()
        {
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = "Đọc tài liệu đào tạo";
            var result = await _readCategoryApiClient.GetAll();
            return View(result.ResultObj);
        }
        public async Task<IActionResult> GetListPostByCategory(int id)
        {
            Guid uid = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                uid = userId;
            };

            var request = new ReadPostUserGetAllByCategory() { userId = uid };

            var result = await _readPostApiClient.UserGetAllByCategory(id,request);
            return PartialView("pageuser/listpostdaotao/_noidungtab", result.ResultObj);
        }

        public async Task<IActionResult> ViewPost(int id)
        {
            var result = await _readPostApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> EndViewPost(int idPost)
        {
            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };

            var request = new ReadResultCreateRequest()
            {
                ReadPostId = idPost,
                UserId = id
            };
            var result = await _readResultApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> UserReadResult()
        {
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };
            var request = new ReadResultSearchRequest()
            {
                UserId = id
            };
            var result = await _readResultApiClient.Search(request);
            ViewBag.thisPage = "Lịch sử đọc tài liệu đào tạo";
            return View(result.ResultObj);
        }

    }
}
