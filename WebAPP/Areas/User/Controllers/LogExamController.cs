using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using ViewModels.Category.Response;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using WebAPP.Services;

namespace WebAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Policy = "userpolicy")]
    public class LogExamController : Controller
    {
        private readonly IExamResultApiClient _examResultApiClient;
        public LogExamController(IExamResultApiClient examResultApiClient)
        {
            _examResultApiClient = examResultApiClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = "Tra cứu lịch sử";

            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };

            var request = new ExamResultSearchRequest()
            {
                UserId = id,
                CategoryId = null,
                examResultId = null,
                Date = null,
                boPhanId =null,
                name =null,
                userName =null
            };

            var resultPhongthi = await _examResultApiClient.Search(request);
            var listPhongthi = resultPhongthi.ResultObj.DistinctBy(x => x.CategoryId).ToList();

            var selectListPhongThi = listPhongthi.Select(item => new SelectListItem
            {
                Value = item.CategoryId.ToString(),
                Text = item.CategoryName
            }).ToList();
            ViewBag.SelectListPhongThi = selectListPhongThi;
            return View(resultPhongthi.ResultObj);
        }

        //Trang xem lịch sử
        [HttpPost]
        public async Task<IActionResult> GetLogAfterExam(int? CategoryId, DateTime? Date, int? examResultId)
        {
            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };
            var request = new ExamResultSearchRequest()
            {
                UserId = id,
                CategoryId = CategoryId,
                examResultId = examResultId,
                Date = Date
            };
            var result = await _examResultApiClient.Search(request);
            if (examResultId == null)
            {
                return PartialView("PageUser/_listLogExam", result.ResultObj);
            }
            else
            {
                ExamResultVm log = result.ResultObj.FirstOrDefault();
                if (log == null)
                {
                    return View();
                }
                else
                {
                    int slCauhoiTN = log.LogExams.Where(x => x.LoaiCauHoi == "TN").Count();
                    int slCauhoiTL = log.LogExams.Where(x => x.LoaiCauHoi == "TL").Count();
                    int slDungTN = log.LogExams.Where(x => (x.LoaiCauHoi == "TN") && (x.Score == x.FinalScore)).Count();
                    int slDungTL = log.LogExams.Where(x => (x.LoaiCauHoi == "TL") && (x.Score == x.FinalScore)).Count();
                    float totalScoreTN = log.LogExams.Where(x => x.LoaiCauHoi == "TN").Sum(x => x.Score).Value;
                    float scoreTN = log.LogExams.Where(x => x.LoaiCauHoi == "TN").Sum(x => x.FinalScore).Value;
                    float totalScoreTL = log.LogExams.Where(x => x.LoaiCauHoi == "TL").Sum(x => x.Score).Value;
                    float scoreTL = log.LogExams.Where(x => x.LoaiCauHoi == "TL").Sum(x => x.FinalScore).Value;

                    ViewBag.slCauhoiTN = slCauhoiTN;
                    ViewBag.slCauhoiTL = slCauhoiTL;
                    ViewBag.slDungTN = slDungTN;
                    ViewBag.slDungTL = slDungTL;
                    ViewBag.totalScoreTN = totalScoreTN;
                    ViewBag.scoreTN = scoreTN;
                    ViewBag.totalScoreTL = totalScoreTL;
                    ViewBag.scoreTL = scoreTL;

                    return PartialView("PageUser/_logExam", log);
                }
            }
        }
    }
}
