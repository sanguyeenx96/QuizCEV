using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class ThongKeController : Controller
    {
        private readonly IExamResultApiClient _examResultApiClient;
        private readonly IDeptApiClient _deptApiClient;

        public ThongKeController(IExamResultApiClient examResultApiClient, IDeptApiClient deptApiClient)
        {
            _examResultApiClient = examResultApiClient;
            _deptApiClient = deptApiClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.thisPage = "Thống kê dữ liệu lịch sử bài thi";
            var request = new ExamResultSearchRequest()
            {
                UserId = null,
                CategoryId = null,
                examResultId = null,
                Date = null,
                boPhanId = null,
                name = null,
                userName = null,
            };
            var result = await _examResultApiClient.Search(request);
            var listPhongthi = result.ResultObj.DistinctBy(x => x.CategoryId).ToList();
            var selectListPhongThi = listPhongthi.Select(item => new SelectListItem
            {
                Value = item.CategoryId.ToString(),
                Text = item.CategoryName
            }).ToList();
            ViewBag.SelectListPhongThi = selectListPhongThi;

            var resultlistDept = await _deptApiClient.GetAll();
            var listDept = resultlistDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.listDept = listDept;
            Console.WriteLine(listDept);

            return View(result.ResultObj);
        }

        //Trang xem lịch sử
        [HttpPost]
        public async Task<IActionResult> Search(int? CategoryId, DateTime? Date, int? examResultId , int? boPhanId, string name, string? userName )
        {
            var request = new ExamResultSearchRequest()
            {
                UserId = null,
                CategoryId = CategoryId,
                examResultId = examResultId,
                Date = Date,
                boPhanId = boPhanId,
                name = name,
                userName = userName,
            };

            var result = await _examResultApiClient.Search(request);
            if (examResultId == null)
            {
                var listResult = result.ResultObj;
                List<string> ten = new List<string>();
                List<float> diem = new List<float>();
                foreach (var item in listResult)
                {
                    ten.Add(item.Hoten.ToString());
                    diem.Add(item.Score);
                }
                ViewBag.dataTen = ten;
                ViewBag.dataDiem = diem;
                return PartialView("PageAdmin/_listLogExam", result.ResultObj);
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
