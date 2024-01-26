using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.PostCategory.Request;
using ViewModels.PostPosts.Request;
using ViewModels.Read.ReadCategory;
using ViewModels.Read.ReadPost;
using ViewModels.Read.ReadResult;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class TaiLieuDaoTaoController : Controller
    {
        private readonly IReadCategoryApiClient _readCategoryApiClient;
        private readonly IReadPostApiClient _readPostApiClient;
        private readonly IReadResultApiClient _readResultApiClient;
        private readonly IDeptApiClient _deptApiClient;
        public TaiLieuDaoTaoController(IReadCategoryApiClient readCategoryApiClient, IReadPostApiClient readPostApiClient,
            IDeptApiClient deptApiClient, IReadResultApiClient readResultApiClient)
        {
            _readCategoryApiClient = readCategoryApiClient;
            _readPostApiClient = readPostApiClient;
            _deptApiClient = deptApiClient;
            _readResultApiClient = readResultApiClient;
        }

        //Category
        public async Task<IActionResult> Category()
        {
            ViewBag.thisPage = "Danh sách chủ đề tài liệu đào tạo";
            var result = await _readCategoryApiClient.GetAll();
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(ReadCategoryCreateRequest request)
        {
            var result = await _readCategoryApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _readCategoryApiClient.GetById(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, data = result.ResultObj });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoryName(int id, ReadCategoryUpdateRequest request)
        {
            var result = await _readCategoryApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _readCategoryApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }


        //Post
        public async Task<IActionResult> List(int readCategoryId)
        {
            ViewBag.thisPage = "Danh sách tài liệu đào tạo";
            var result = await _readCategoryApiClient.GetAll();
            ViewBag.selectChude = result.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            var listPostWithOutContent = await _readPostApiClient.GetAllByCategory(readCategoryId);
            ViewBag.SelectedCategoryId = readCategoryId.ToString();
            return View(listPostWithOutContent.ResultObj);
        }

        public async Task<IActionResult> CreateNewPost()
        {
            var result = await _readCategoryApiClient.GetAll();
            ViewBag.selectChude = result.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            ViewBag.thisPage = "Tạo bài đăng tin tức mới";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] ReadPostCreateRequest request)
        {
            var result = await _readPostApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateThumbImage(int id, ReadPostThumbImageUpdate request)
        {
            var result = await _readPostApiClient.UpdateThumbImage(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> UpdatePost(int id)
        {
            ViewBag.thisPage = "Sửa bài đăng tài liệu đào tạo";
            var resultChude = await _readCategoryApiClient.GetAll();
            ViewBag.selectChude = resultChude.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            var result = await _readPostApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostContent(int id, [FromBody] ReadPostUpdateRequest request)
        {
            var result = await _readPostApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var result = await _readPostApiClient.ChangeStatus(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _readPostApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }


        //Thống kê
        public async Task<IActionResult> ReadResult()
        {
            ViewBag.thisPage = "Thống kê dữ liệu lịch sử đọc tài liệu đào tạo";
            var result = await _readCategoryApiClient.GetAll();
            var listChude = result.ResultObj;
            var selectListChude = listChude.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Title
            }).ToList();
            ViewBag.SelectListChude = selectListChude;

            var resultlistDept = await _deptApiClient.GetAll();
            var listDept = resultlistDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.listDept = listDept;

            return View(result.ResultObj);
        }


        [HttpPost]
        public async Task<IActionResult> GetSelectListPost(int catId)
        {
            var listPost = await _readPostApiClient.GetAllByCategory(catId);
            var result = listPost.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> Search(int? PostId, DateTime? Date, int? boPhanId, string? name, string? userName, int? modelId, int? cellId)
        {
            var request = new ReadResultSearchRequest()
            {
                UserId = null,
                readPostId = PostId,
                Date = Date,
                boPhanId = boPhanId,
                name = name,
                userName = userName,
                modelId = modelId,
                cellId = cellId
            };
            var result = await _readResultApiClient.Search(request);
            return PartialView("PageAdmin/_listLogReadResult", result.ResultObj);
        }


    }
}
