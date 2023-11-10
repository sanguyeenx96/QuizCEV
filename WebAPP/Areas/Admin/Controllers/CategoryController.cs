using Microsoft.AspNetCore.Mvc;
using ViewModels.Category.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> List()
        {
            ViewBag.thisPage = "Danh sách chủ đề & Phòng thi";
            var result = await _categoryApiClient.GetAll();
            return View(result.ResultObj);
        }
        public IActionResult Create()
        {
            ViewBag.thisPage = "Thêm chủ đề & Phòng thi";
            return View();
        }
        public async Task<IActionResult> ChangeStatus()
        {
            ViewBag.thisPage = "Đóng / Mở phòng thi";
            var result = await _categoryApiClient.GetAll();
            return View(result.ResultObj); ;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var request = new CategoryCreateRequest()
            {
                Name = name
            };
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
    }
}
