using elFinder.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.IO.Pipes;
using System.Security.Claims;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.ExportExcel;
using ViewModels.Question.Response;
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

        [HttpPost]
        public async Task<IActionResult> DanhGia(int id, string result)
        {
            var request = new ExamResultDanhGiaRequest()
            {
                Result = result == "ok" ? true : false
            };
            var kq = await _examResultApiClient.DanhGia(id, request);
            if (!kq.IsSuccessed)
            {
                return Json(new { success = false, message = kq.Message });
            }
            return Json(new { success = true });
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
        public async Task<IActionResult> Search(int? CategoryId, DateTime? Date, int? examResultId , int? boPhanId, string name, string? userName , int? modelId, int? cellId, int? mode)
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
                cellId = cellId,
                mode = mode
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

        //Trang xuất báo cáo kết quả đào tạo
        [HttpGet]
        public async Task<IActionResult> Xuatbaocao()
        {
            ViewBag.thisPage = "Xuất báo cáo kết quả đào tạo";
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

        [HttpPost]
        public async Task<IActionResult> SearchForExport(int? CategoryId, DateTime? Date, int? examResultId, int? boPhanId, string name, string? userName, int? modelId, int? cellId,int? mode)
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
                cellId = cellId,
                mode = mode
            };
            var result = await _examResultApiClient.Searchdata(request);
            if (result != null)
            {
                List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> listExport = new List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest>();
                TempData["listExport"] = JsonConvert.SerializeObject(listExport);

                List<ExamResultVm> listSearchResullt = result.ResultObj;
                int sothutu = 0;
                foreach (var item in listSearchResullt.OrderBy(x => x.Username))
                {
                    var itemListExport = new ExportExcelBaoCaoKetQuaDaoTaoCreateRequest()
                    {
                        noidungdaotao = item.CategoryName,
                        stt = sothutu++,
                        manv = item.Username,
                        hoten = item.Hoten,
                        bophan = item.Bophan,
                        diem = Convert.ToInt32(item.Score),
                        thoigian = item.Date.ToString("dd/MM/yyyy")
                    };
                    listExport.Add(itemListExport);
                }
                TempData["listExport"] = JsonConvert.SerializeObject(listExport);
                TempData.Keep();

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
                return PartialView("PageAdmin/_listLogExamExport", result.ResultObj);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult>ConfirmExport()
        {
            string jsonlistExport = TempData["listExport"].ToString();
            List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> listExport = JsonConvert.DeserializeObject<List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest>>(jsonlistExport);
            TempData.Keep();
            return View(listExport);
        }



        [HttpPost]
        public async Task<IActionResult> Export(string benyeucaudaotao, string benthuchiendaotao, string noidungdaotao,
            string diadiemdaotao, string thoigian, int thangdiem, int sodiemdat)
        {
            string jsonlistExport = TempData.Peek("listExport")?.ToString();
            List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> listExport = JsonConvert.DeserializeObject<List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest>>(jsonlistExport);
            foreach (var item in listExport)
            {
                item.benyeucaudaotao = benyeucaudaotao;
                item.benthuchiendaotao = benthuchiendaotao;
                item.noidungdaotao = noidungdaotao;
                item.diadiemdaotao = diadiemdaotao;
                item.thoigian = thoigian;
                item.thangdiem = thangdiem;
                item.sodiemdat = sodiemdat;
                if (item.diem >= sodiemdat)
                {
                    item.danhgia = "OK";
                }
                else
                {
                    item.danhgia = "NG";
                }
            }
            var request = listExport;

            string ExcelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "F04-Baocaoketquadaotao.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(ExcelFilePath))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Sheet1"];
                int startRow = 13; 
                int endRow = worksheet.Dimension.End.Row; // Hàng cuối cùng có dữ liệu
                var data = request;
                if (data != null)
                {
                    int rowCount = data.Count; // Số hàng dữ liệu mới sẽ được chèn vào
                    worksheet.InsertRow(startRow + 1, rowCount);
                    int currentRow = startRow;

                    //Info
                    worksheet.Cells["C" + 6].Value = data.FirstOrDefault()?.benyeucaudaotao;
                    worksheet.Cells["C" + 7].Value = data.FirstOrDefault()?.noidungdaotao;
                    worksheet.Cells["C" + 9].Value = data.FirstOrDefault()?.sodiemdat;
                    worksheet.Cells["J" + 6].Value = data.FirstOrDefault()?.benthuchiendaotao;
                    worksheet.Cells["J" + 7].Value = data.FirstOrDefault()?.diadiemdaotao;
                    worksheet.Cells["J" + 8].Value = data.FirstOrDefault()?.thoigian;
                    worksheet.Cells["J" + 9].Value = data.FirstOrDefault()?.thangdiem;

                    //List
                    foreach (var item in data)
                    {
                        worksheet.Cells["A" + currentRow].Value = item.stt;
                        worksheet.Cells["B" + currentRow].Value = item.manv;
                        // Merge hai cột C và D lại với nhau
                        worksheet.Cells["C" + currentRow + ":D" + currentRow].Merge = true;
                        worksheet.Cells["C" + currentRow].Value = item.hoten;
                        worksheet.Cells["E" + currentRow].Value = "□";                        
                        worksheet.Cells["F" + currentRow].Value = item.bophan;
                        worksheet.Cells["G" + currentRow].Value = item.diem;
                        worksheet.Cells["H" + currentRow].Value = item.danhgia;
                        worksheet.Cells["K" + currentRow].Value = "Đã ký";
                        using (var range = worksheet.Cells["A" + currentRow + ":K" + currentRow])
                        {
                            //range.Style.Font.Bold = true; // Đặt chữ in đậm
                            range.Style.Font.Size = 14; // Đặt kích cỡ chữ
                            range.Style.Font.Name = "Times New Roman"; // Đặt tên font chữ
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa ngang cho cả dòng
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc cho cả dòng
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        //  worksheet.Cells["B" + currentRow].Value = item.Property2;
                        currentRow++;
                    }
                    // Convert ExcelPackage thành mảng byte
                    byte[] fileContents = excelPackage.GetAsByteArray();

                    // Trả về file Excel như là phản hồi từ phương thức API
                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "F04-Baocaoketquadaotao.xlsx");
                }
            }
            return Ok();
        }


       
    }
}
