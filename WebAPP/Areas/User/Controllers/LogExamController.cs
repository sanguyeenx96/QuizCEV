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
                Date = null
            };
            var resultPhongthi = await _examResultApiClient.Search(request);
            var listPhongthi = resultPhongthi.ResultObj.DistinctBy(x => x.CategoryId).ToList();

            var selectListPhongThi = listPhongthi.Select(item => new SelectListItem
            {
                Value = item.CategoryId.ToString(),
                Text = item.CategoryName
            }).ToList();
            ViewBag.SelectListPhongThi = selectListPhongThi;
            return View();
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
            if(examResultId == null)
            {
                return PartialView("PageUser/_listLogExam", result.ResultObj);
            }
            else
            {
                return PartialView("PageUser/_logExam", result.ResultObj);
            }
            
        }



    }
}
