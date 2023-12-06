using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Category.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.thisPage = "Danh sách Chủ đề & Phòng thi";
            var result = await _categoryApiClient.GetAll();
            return View(result.ResultObj);
        }

        public IActionResult Create()
        {
            ViewBag.thisPage = "Thêm mới Chủ đề & Phòng thi";
            return View();
        }

        public async Task<IActionResult> ChangeStatus()
        {
            ViewBag.thisPage = "Đóng / Mở phòng thi";
            var result = await _categoryApiClient.GetAll();
            return View(result.ResultObj); ;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {            
            var result = await _categoryApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new {success=true, message=result.Message});
        }

        [HttpPost]
        public async Task<IActionResult> GetById(int id)
        { 
            var result = await _categoryApiClient.GetById(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = result.Message ,data=result.ResultObj});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true});
        }

        [HttpPost]
        public async Task<IActionResult> UpdateName(int id,CategoryUpdateNameRequest request)
        {
            var result = await _categoryApiClient.UpdateName(id,request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var result = await _categoryApiClient.UpdateStatus(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTime(int id,CategoryUpdateTimeRequest request)
        {
            var result = await _categoryApiClient.UpdateTime(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
    }
}
