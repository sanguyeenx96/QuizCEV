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
        private readonly ICategoryApiClient _categoryApiClient;

        public ThongKeController(IExamResultApiClient examResultApiClient, IDeptApiClient deptApiClient, ICategoryApiClient categoryApiClient)
        {
            _examResultApiClient = examResultApiClient;
            _deptApiClient = deptApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.thisPage = "Thống kê dữ liệu lịch sử bài thi";       
            var result = await _categoryApiClient.GetAll();
            var listPhongthi = result.ResultObj;
            var selectListPhongThi = listPhongthi.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            }).ToList();
            ViewBag.SelectListPhongThi = selectListPhongThi;

            var resultlistDept = await _deptApiClient.GetAll();
            var listDept = resultlistDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.listDept = listDept;
            return View();
        }

        //Trang xem lịch sử
        [HttpPost]
        public async Task<IActionResult> Search(int? CategoryId, DateTime? Date, int? examResultId , int? boPhanId, string name, string? userName , int? modelId, int? cellId)
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
                modelId = modelId,
                cellId = cellId
            };

            var result = await _examResultApiClient.Search(request);
            if (!result.ResultObj.Any())
            {
                ViewBag.dataTen = new List<string>();
                ViewBag.dataDiem = new List<float>();
                return PartialView("PageAdmin/_listLogExam", result.ResultObj);
            }
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

                    return PartialView("PageAdmin/_logExam", log);
                }
            }
        }





    }
}
