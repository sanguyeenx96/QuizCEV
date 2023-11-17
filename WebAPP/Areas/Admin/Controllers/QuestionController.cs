using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.Question.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IQuestionApiClient _questionApiClient;
        private readonly ICauHoiTuLuanApiClient _cauHoiTuLuanApiClient;

        public QuestionController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient, ICauHoiTuLuanApiClient cauHoiTuLuanApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
            _cauHoiTuLuanApiClient = cauHoiTuLuanApiClient;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.thisPage = "Danh sách câu hỏi";
            var listcategory = await _categoryApiClient.GetAll();
            ViewBag.listcat = listcategory.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var result = await _questionApiClient.GetAll();
            return View(result.ResultObj);
        }       

        public async Task<IActionResult> Create()
        {
            ViewBag.thisPage = "Thêm mới câu hỏi trắc nghiệm";
            var listcategory = await _categoryApiClient.GetAll();
            ViewBag.listcat = listcategory.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        public async Task<IActionResult> CreateTuLuan()
        {
            ViewBag.thisPage = "Thêm mới câu hỏi tự luận";
            var listcategory = await _categoryApiClient.GetAll();
            ViewBag.listcat = listcategory.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByCategoryId(int id)
        {
            var count = await _questionApiClient.Count(id);
            ViewBag.countTracNghiem = count.ResultObj;
            var result = await _questionApiClient.GetAllByCategory(id);
            return PartialView("_questionList", result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTuLuanByCategoryId(int id)
        {
            var result = await _cauHoiTuLuanApiClient.GetAllByCategory(id);
            return PartialView("_questionTuLuanList", result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTuLuan(CauHoiTuLuanCreateRequest request)
        {
            var result = await _cauHoiTuLuanApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> CreateManual( QuestionCreateRequest request)
        {
            var result = await _questionApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = result.Message });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file, int categoryId)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var result = await _questionApiClient.ImportExcel(fileStream, categoryId);
                if (result.IsSuccessed)
                {                  
                    return Json(new { success = true, data = result.ResultObj });
                }
                return Json(new { success = false, message = result.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetTotalScore (int categoryId)
        {
            var result = await _questionApiClient.GetTotalScore(categoryId);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, result = result.Score });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateScore(int id, QuestionUpdateScoreRequest request)
        {
            var result = await _questionApiClient.UpdateScore(id,request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateScoreTuluan(int id, CauHoiTuLuanUpdateScoreRequest request)
        {
            var result = await _cauHoiTuLuanApiClient.UpdateScore(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTracNghiem(int id)
        {
            var result = await _questionApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTuLuan(int id)
        {
            var result = await _cauHoiTuLuanApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
    }
}
