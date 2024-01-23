using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.PostCategory.Request;
using ViewModels.PostPosts.Request;
using ViewModels.ThongBaoCategory.Request;
using ViewModels.ThongBaoPost.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class ThongBaoController : Controller
    {
        private readonly IThongBaoCategoryApiClient _thongBaoCategoryApiClient;
        private readonly IThongBaoPostApiClient _thongBaoPostApiClient;
        public ThongBaoController(IThongBaoCategoryApiClient thongBaoCategoryApiClient, IThongBaoPostApiClient thongBaoPostApiClient)
        {
            _thongBaoCategoryApiClient = thongBaoCategoryApiClient;
            _thongBaoPostApiClient = thongBaoPostApiClient;
        }
        //Chủ đề
        public async Task<IActionResult> Category()
        {
            ViewBag.thisPage = "Quản lý danh sách chủ đề thông báo";
            var result = await _thongBaoCategoryApiClient.GetAll();
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(ThongBaoCategoryCreateRequest request)
        {
            var result = await _thongBaoCategoryApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _thongBaoCategoryApiClient.GetById(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = result.Message, data = result.ResultObj });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoryName(int id, ThongBaoCategoryUpdateRequest request)
        {
            var result = await _thongBaoCategoryApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _thongBaoCategoryApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }


        ////POST
        public async Task<IActionResult> CreateNewPost()
        {
            var result = await _thongBaoCategoryApiClient.GetAll();
            ViewBag.selectChude = result.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            ViewBag.thisPage = "Tạo bài đăng thông báo mới";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] ThongBaoPostCreateRequest request)
        {
            var result = await _thongBaoPostApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> ListPost(int thongbaoCategoryId)
        {
            var result = await _thongBaoCategoryApiClient.GetAll();
            ViewBag.selectChude = result.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            ViewBag.thisPage = "Danh sách bài đăng thông báo";
            var listPostWithOutContent = await _thongBaoPostApiClient.GetAllByCategory(thongbaoCategoryId);
            ViewBag.SelectedCategoryId = thongbaoCategoryId.ToString();
            return View(listPostWithOutContent.ResultObj);
        }

        public async Task<IActionResult> UpdatePost(int id)
        {
            ViewBag.thisPage = "Sửa bài đăng thông báo";
            var resultChude = await _thongBaoCategoryApiClient.GetAll();
            ViewBag.selectChude = resultChude.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            var result = await _thongBaoPostApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostContent(int id, [FromBody] ThongBaoPostUpdateRequest request)
        {
            var result = await _thongBaoPostApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _thongBaoPostApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        ///UPLOAD
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Thông báo");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                // Kết hợp đường dẫn thư mục và tên tệp tin gốc để có đường dẫn đầy đủ.
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                //string uniqueFileName = originalFileName + "_" + Guid.NewGuid().ToString() + fileExtension;
                if (System.IO.File.Exists(filePath))
                {
                    return Json(new { success = false, message = "Tệp tin đã tồn tại với cùng tên" });          
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Ok(new { success = true, fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
