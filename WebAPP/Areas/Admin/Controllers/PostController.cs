using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.Category.Request;
using ViewModels.PostCategory.Request;
using ViewModels.PostPosts.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class PostController : Controller
    {
        private readonly IPostCategoryApiClient _postCategoryApiClient;
        private readonly IPostPostsApiClient _postPostsApiClient;

        public PostController(IPostCategoryApiClient postCategoryApiClient, IPostPostsApiClient postPostsApiClient)
        {
            _postCategoryApiClient = postCategoryApiClient;
            _postPostsApiClient = postPostsApiClient;
        }

        //Chủ đề
        public async Task<IActionResult> Category()
        {
            ViewBag.thisPage = "Quản lý danh sách chủ đề tin tức";
            var result = await _postCategoryApiClient.GetAll();
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(PostCategoryCreateRequest request)
        {
            var result = await _postCategoryApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _postCategoryApiClient.GetById(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = result.Message, data = result.ResultObj });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoryName(int id, PostCategoryUpdateRequest request)
        {
            var result = await _postCategoryApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postCategoryApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        //Bài đăng 
        public async Task<IActionResult> CreateNewPost()
        {
            var result = await _postCategoryApiClient.GetAll();
            ViewBag.selectChude = result.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            ViewBag.thisPage = "Tạo bài đăng mới";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostPostsCreateRequest request)
        {
            var result = await _postPostsApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }




    }
}
