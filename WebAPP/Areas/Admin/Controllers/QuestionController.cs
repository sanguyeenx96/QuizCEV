using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.Question.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class QuestionController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IQuestionApiClient _questionApiClient;
        private readonly ICauHoiTuLuanApiClient _cauHoiTuLuanApiClient;
        private readonly ICauHoiTrinhTuThaoTacApiClient _cauHoiTrinhTuThaoTacApiClient;

        private readonly IDiemChuYApiClient _diemChuYApiClient;
        private readonly ILoiTaiCongDoanApiClient _loiTaiCongDoanApiClient;
        private readonly IDoiSachApiClient _doiSachApiClient;

        public QuestionController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient,
            ICauHoiTuLuanApiClient cauHoiTuLuanApiClient, ICauHoiTrinhTuThaoTacApiClient cauHoiTrinhTuThaoTacApiClient,
            IDiemChuYApiClient diemChuYApiClient, ILoiTaiCongDoanApiClient loiTaiCongDoanApiClient,
            IDoiSachApiClient doiSachApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
            _cauHoiTuLuanApiClient = cauHoiTuLuanApiClient;
            _cauHoiTrinhTuThaoTacApiClient = cauHoiTrinhTuThaoTacApiClient;
            _diemChuYApiClient = diemChuYApiClient;
            _loiTaiCongDoanApiClient = loiTaiCongDoanApiClient;
            _doiSachApiClient = doiSachApiClient;
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
            ViewBag.thisPage = "Tạo câu hỏi trắc nghiệm";
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
            ViewBag.thisPage = "Tạo câu hỏi tự luận";
            var listcategory = await _categoryApiClient.GetAll();
            ViewBag.listcat = listcategory.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCauHoiNullScore(int id)
        {
            var resultTN = await _questionApiClient.GetAllByCategory(id);
            var nullScoreTN = resultTN.ResultObj.Where(x => (x.Score == null || x.Score == 0)).Count();
            var resultTL = await _cauHoiTuLuanApiClient.GetAllByCategory(id);
            var nullScoreTL = resultTL.ResultObj.Where(x => (x.Score == null || x.Score == 0)).Count();
            int total = nullScoreTN + nullScoreTL;
            return Json(new { success = true, sl = total });
        }

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
        public async Task<IActionResult> GetAllByCauHoiTuLuan(int id,int catId)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.GetAllByCauHoiTuLuan(id);
            ViewBag.catId = catId;
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

        //IMPORT TRINH TU THAO TAC FROM EXCEL
        [HttpPost]
        public async Task<IActionResult> GetListQuestionTuLuanToSelect(int id)
        {
            var dschtl = await _cauHoiTuLuanApiClient.GetAllByCategory(id);
            var result = dschtl.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Id.ToString()
            });
            return Json(result);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportExcelTTTT([FromForm] IFormFile file, int cauhoituluanId)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var result = await _cauHoiTrinhTuThaoTacApiClient.ImportExcel(fileStream, cauhoituluanId);
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
        public async Task<IActionResult> UpdateScoreTTTT(Guid id, CauHoiTrinhTuThaoTacUpdateScoreRequest request)
        { 
            var result = await _cauHoiTrinhTuThaoTacApiClient.UpdateScore(id, request);
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
        public async Task<IActionResult> UpdateTextCauHoiTrinhTuThaoTac(Guid id, CauHoiTrinhTuThaoTacUpdateTextRequest request)
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
        public async Task<IActionResult> DeleteCauHoiTrinhTuThaoTac(Guid id, CauHoiTrinhTuThaoTacDeleteRequest request)
        {
            var result = await _cauHoiTrinhTuThaoTacApiClient.Delete(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        //Điểm chú ý
        [HttpPost]
        public async Task<IActionResult> CreateDiemChuY(Guid cauhoitrinhtuthaotacId, DiemChuYCreateRequest request)
        {
            var result = await _diemChuYApiClient.Create(cauhoitrinhtuthaotacId, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> OpenModalEditDCY(Guid id)
        {
            var result = await _diemChuYApiClient.GetAllByCauHoiTrinhTuThaoTacId(id);
            return PartialView("_editDiemChuY", result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectListDCY(Guid id)
        {
            var result = await _diemChuYApiClient.GetAllByCauHoiTrinhTuThaoTacId(id);
            var selectList= result.ResultObj.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Id.ToString()  // Giả sử Id là trường chứa giá trị bạn muốn
            }).ToList();
            return Json(selectList);
        }

        [HttpPost]
        public async Task<IActionResult> EditDCY(int id, DiemChuYUpdateRequest request)
        {
            var result = await _diemChuYApiClient.Update(id,request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDCY(int id)
        {
            var result = await _diemChuYApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        //Lỗi tại công đoạn
        [HttpPost]
        public async Task<IActionResult> CreateLoiTaiCongDoan(Guid cauhoitrinhtuthaotacId, LoiTaiCongDoanCreateRequest request)
        {
            var result = await _loiTaiCongDoanApiClient.Create(cauhoitrinhtuthaotacId, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        //Đối sách
        [HttpPost]
        public async Task<IActionResult> OpenModalCreateDoiSach(Guid cauhoitrinhtuthaotacId)
        {
            var result = await _loiTaiCongDoanApiClient.GetAllByCauHoiTrinhTuThaoTacId(cauhoitrinhtuthaotacId);
            return PartialView("_createDoiSach", result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectListLTCD(Guid cauhoitrinhtuthaotacId)
        {
            var result = await _loiTaiCongDoanApiClient.GetAllByCauHoiTrinhTuThaoTacId(cauhoitrinhtuthaotacId);
            var selectList = result.ResultObj.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Id.ToString()  // Giả sử Id là trường chứa giá trị bạn muốn
            }).ToList();
            return Json(selectList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoiSach(int loitaicongdoanId, DoiSachCreateRequest request)
        {
            var result = await _doiSachApiClient.Create(loitaicongdoanId, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> OpenModalEditLoiDoiSach(int loitaicongdoanId)
        {
            var result = await _loiTaiCongDoanApiClient.GetById(loitaicongdoanId);
            return PartialView("_editLoiDoiSach", result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectListLoiDoiSach(int loitaicongdoanId)
        {
            var result = await _loiTaiCongDoanApiClient.GetById(loitaicongdoanId);
            var selectList = result.ResultObj.doiSaches.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Id.ToString()  // Giả sử Id là trường chứa giá trị bạn muốn
            }).ToList();
            return Json(selectList);
        }

        [HttpPost]
        public async Task<IActionResult> EditDoiSach(int id, DoiSachUpdateRequest request)
        {
            var result = await _doiSachApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> EditLTCD(int id, LoiTaiCongDoanUpdateRequest request)
        {
            var result = await _loiTaiCongDoanApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLTCD(int id)
        {
            var result = await _loiTaiCongDoanApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDS(int id)
        {
            var result = await _doiSachApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
    }
}
