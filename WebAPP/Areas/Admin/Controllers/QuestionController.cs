using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.Question.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IQuestionApiClient _questionApiClient;
        private readonly ICauHoiTuLuanApiClient _cauHoiTuLuanApiClient;
        private readonly ICauHoiTrinhTuThaoTacApiClient _cauHoiTrinhTuThaoTacApiClient;
        public QuestionController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient, ICauHoiTuLuanApiClient cauHoiTuLuanApiClient, ICauHoiTrinhTuThaoTacApiClient cauHoiTrinhTuThaoTacApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
            _cauHoiTuLuanApiClient = cauHoiTuLuanApiClient;
            _cauHoiTrinhTuThaoTacApiClient = cauHoiTrinhTuThaoTacApiClient;
        }


        //PAGE:
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


        //GET:
        [HttpPost]
        public async Task<IActionResult> GetCauHoiTracNghiemById(int id)
        {
            var result = await _questionApiClient.GetById(id);
            return PartialView("_questionCauHoiTracNghiemInfo", result.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllByCategoryId(int id)
        {
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
        public async Task<IActionResult> CountTracNghiem(int id)
        {
            var count = await _questionApiClient.Count(id);
            var result = count.ResultObj;
            return Json(new { success = true, data = result });
        }
        [HttpPost]
        public async Task<IActionResult> CountTuLuan(int id)
        {
            var countTuLuan = await _cauHoiTuLuanApiClient.Count(id);
            var result = countTuLuan.ResultObj;
            return Json(new { success = true, data = result });
        }
        [HttpPost]
        public async Task<IActionResult> GetTotalScore(int categoryId)
        {
            var result = await _questionApiClient.GetTotalScore(categoryId);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, result = result.Score });
        }
        [HttpPost]
        public async Task<IActionResult> GetAllByCauHoiTuLuan(int id)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.GetAllByCauHoiTuLuan(id);
            return PartialView("_CauHoiTrinhTuThaoTac", result.ResultObj);
        }


        //CREATE:
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
        public async Task<IActionResult> CreateManual(QuestionCreateRequest request)
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
        public async Task<IActionResult> CreateNoiDungTrinhTuThaoTac(CauHoiTrinhTuThaoTacCreateRequest request)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }


        //UPDATE:
        [HttpPost]
        public async Task<IActionResult> UpdateScore(int id, QuestionUpdateScoreRequest request)
        {
            var result = await _questionApiClient.UpdateScore(id, request);
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
        public async Task<ActionResult> UpdatePositions([FromBody] List<CauHoiTrinhTuThaoTacChangeOrderRequest> parsedNewOrder)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.ChangeOrder(parsedNewOrder);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCauHoiTracNghiem(int id, QuestionUpdateRequest request)
        {
            var result = await _questionApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTextCauHoiTuLuan(int id, CauHoiTuLuanUpdateTextRequest request)
        {
            var result = await _cauHoiTuLuanApiClient.UpdateText(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTextCauHoiTrinhTuThaoTac(int id, CauHoiTrinhTuThaoTacUpdateTextRequest request)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.UpdateText(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }


        //DELETE:
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
        [HttpPost]
        public async Task<IActionResult> DeleteCauHoiTrinhTuThaoTac(int id, CauHoiTrinhTuThaoTacDeleteRequest request)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.Delete(id,request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }      
    }
}
