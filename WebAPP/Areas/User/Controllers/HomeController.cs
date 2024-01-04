using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Security.Claims;
using ViewModels.Category.Response;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.ExamResult.Request;
using ViewModels.LogExam.Request;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamDoiSach.Request;
using ViewModels.LogExamLoiTaiCongDoan.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;
using ViewModels.Question.Response;
using WebAPP.Services;
using static System.Formats.Asn1.AsnWriter;

namespace WebAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Policy = "userpolicy")]
    public class HomeController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IQuestionApiClient _questionApiClient;
        private readonly ICauHoiTuLuanApiClient _cauHoiTuLuanApiClient;
        private readonly ICauHoiTrinhTuThaoTacApiClient _cauHoiTrinhTuThaoTacApiClient;
        private readonly IExamResultApiClient _examResultApiClient;
        private readonly ILogExamApiClient _logExamApiClient;
        private readonly ILogExamTrinhtuthaotacApiClient _logExamTrinhtuthaotacApiClient;
        private readonly ISettingApiClient _settingApiClient;

        private readonly ILogExamLTCDApiClient _logExamLTCDApiClient;
        private readonly ILogExamDCYApiClient _logExamDCYApiClient;
        private readonly ILogExamDSApiClient _logExamDSApiClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient,
            ICauHoiTuLuanApiClient cauHoiTuLuanApiClient, ICauHoiTrinhTuThaoTacApiClient cauHoiTrinhTuThaoTacApiClient,
            IExamResultApiClient examResultApiClient, ILogExamApiClient logExamApiClient,
            ILogExamTrinhtuthaotacApiClient logExamTrinhtuthaotacApiClient, ISettingApiClient settingApiClient,
            ILogExamDCYApiClient logExamDCYApiClient, ILogExamLTCDApiClient logExamLTCDApiClient,
            ILogExamDSApiClient logExamDSApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
            _cauHoiTuLuanApiClient = cauHoiTuLuanApiClient;
            _cauHoiTrinhTuThaoTacApiClient = cauHoiTrinhTuThaoTacApiClient;
            _examResultApiClient = examResultApiClient;
            _logExamApiClient = logExamApiClient;
            _logExamTrinhtuthaotacApiClient = logExamTrinhtuthaotacApiClient;
            _settingApiClient = settingApiClient;
            _logExamDCYApiClient = logExamDCYApiClient;
            _logExamLTCDApiClient = logExamLTCDApiClient;
            _logExamDSApiClient = logExamDSApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        //Check setting re-test
        private async Task<bool> CheckRetest()
        {
            var result = await _settingApiClient.GetAll();
            var listSetting = result.ResultObj;
            var rerestSetting = listSetting.Where(x => x.Name == "Retest").FirstOrDefault();
            return rerestSetting.Status;
        }
        private async Task<bool> CheckBaithiRetest(string categoryName)
        {
            Guid id = Guid.Empty;
            var userIdString = User.FindFirstValue("UserId");
            if (Guid.TryParse(userIdString, out var userId))
            {
                id = userId;
            };
            var request = new ExamResultCheckRetestRequest()
            {
                UserId = id,
                CategoryName = categoryName
            };

            var result = await _examResultApiClient.CheckRetest(request);
            if (result.IsSuccessed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Chọn phòng thi:
        [HttpGet]
        public async Task<IActionResult> SelectRoom()
        {
            bool modeCheck = CheckRetest().Result;
            ViewBag.settingRetest = modeCheck;
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = "Bắt đầu kiểm tra";
            ViewBag.step1 = "Chọn phòng thi";

            var listCategory = await _categoryApiClient.GetAll();
            return View(listCategory.ResultObj);
        }

        //Lấy dữ liệu câu hỏi từ phòng thi:
        [HttpPost]
        public async Task<IActionResult> SelectRoom(int idPhong, string categoryName)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            // Sử dụng session
            session.SetString("Chedothi", "thithat");
            //TempData["Chedothithu"] = 0;
            bool modeCheck = CheckRetest().Result;
            if (modeCheck == false) //Nếu không cho thi lại
            {
                var check = await CheckBaithiRetest(categoryName);
                if (check)
                {
                    return Json(new { success = false, message = "Bạn đã làm bài thi này rồi!" });
                }
                else
                {
                    //Trắc nghiệm
                    List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = new List<QuestionAndAnswerVm>();
                    TempData["QuestionAndAnswerTracNghiem"] = JsonConvert.SerializeObject(listQuestionAndAnswerTracNghiem);
                    var listTracNghiem = await _questionApiClient.GetAllByCategory(idPhong);
                    var dsCauHoiTracNghiem = listTracNghiem.ResultObj;
                    var random = new Random();
                    dsCauHoiTracNghiem = dsCauHoiTracNghiem.OrderBy(x => random.Next()).ToList();
                    List<QuestionVm> listQuestionTracNghiem = dsCauHoiTracNghiem;
                    TempData["danhsachcauhoiTracNghiem"] = JsonConvert.SerializeObject(listQuestionTracNghiem);
                    TempData["soluongcauhoiTracNghiem"] = listQuestionTracNghiem.Count();
                    Queue<QuestionVm> queue = new Queue<QuestionVm>();
                    foreach (QuestionVm a in listQuestionTracNghiem)
                    {
                        queue.Enqueue(a);
                    }
                    TempData["questionsTracNghiem"] = JsonConvert.SerializeObject(queue);


                    //Tự luận:
                    List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = new List<QuestionAndAnswerTrinhTuThaoTacVm>();
                    TempData["QuestionAndAnswerTrinhTuThaoTac"] = JsonConvert.SerializeObject(listQuestionAndAnswerTrinhTuThaoTac);

                    List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = new List<QuestionAndAnswerDiemChuYVm>();
                    TempData["QuestionAndAnswerDiemChuY"] = JsonConvert.SerializeObject(listQuestionAndAnswerDiemChuY);

                    List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = new List<QuestionAndAnswerLoiTaiCongDoanVm>();
                    TempData["QuestionAndAnswerLoiTaiCongDoan"] = JsonConvert.SerializeObject(listQuestionAndAnswerLoiTaiCongDoan);

                    List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = new List<QuestionAndAnswerDoiSachVm>();
                    TempData["QuestionAndAnswerDoiSach"] = JsonConvert.SerializeObject(listQuestionAndAnswerDoiSach);



                    var listTuLuan = await _cauHoiTuLuanApiClient.GetAllByCategory(idPhong);
                    var dsCauHoiTuLuan = listTuLuan.ResultObj;
                    dsCauHoiTuLuan = dsCauHoiTuLuan.OrderBy(x => random.Next()).ToList();
                    List<CauHoiTuLuanVm> listQuestionTuLuan = dsCauHoiTuLuan;
                    TempData["dsCauHoiTL"] = JsonConvert.SerializeObject(listQuestionTuLuan);
                    TempData["danhsachcauhoiTuLuan"] = JsonConvert.SerializeObject(listQuestionTuLuan);
                    TempData["soluongcauhoiTuLuan"] = listQuestionTuLuan.Count();
                    Queue<CauHoiTuLuanVm> queueTuluan = new Queue<CauHoiTuLuanVm>();
                    foreach (CauHoiTuLuanVm a in listQuestionTuLuan)
                    {
                        queueTuluan.Enqueue(a);
                    }
                    TempData["questionsTuLuan"] = JsonConvert.SerializeObject(queueTuluan);

                    //Common:
                    var phongThi = await _categoryApiClient.GetById(idPhong);
                    int thoiGianThi = phongThi.ResultObj.Time;
                    int totalQuestion = listQuestionTracNghiem.Count() + listQuestionTuLuan.Count();
                    TempData["totalQuestion"] = totalQuestion;
                    TempData["idPhong"] = idPhong;
                    TempData["thoiGianThi"] = thoiGianThi;

                    TempData["score"] = 0;
                    TempData["numQuestionTracnghiem"] = 0;
                    TempData["numQuestionTuLuan"] = 0;
                    TempData["StatusTN"] = 0;
                    TempData["StatusTL"] = 0;

                    TempData.Keep();
                    return Json(new { success = true });
                }
            }
            else
            {
                //Trắc nghiệm
                List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = new List<QuestionAndAnswerVm>();
                TempData["QuestionAndAnswerTracNghiem"] = JsonConvert.SerializeObject(listQuestionAndAnswerTracNghiem);
                var listTracNghiem = await _questionApiClient.GetAllByCategory(idPhong);
                var dsCauHoiTracNghiem = listTracNghiem.ResultObj;
                var random = new Random();
                dsCauHoiTracNghiem = dsCauHoiTracNghiem.OrderBy(x => random.Next()).ToList();
                List<QuestionVm> listQuestionTracNghiem = dsCauHoiTracNghiem;
                TempData["danhsachcauhoiTracNghiem"] = JsonConvert.SerializeObject(listQuestionTracNghiem);
                TempData["soluongcauhoiTracNghiem"] = listQuestionTracNghiem.Count();
                Queue<QuestionVm> queue = new Queue<QuestionVm>();
                foreach (QuestionVm a in listQuestionTracNghiem)
                {
                    queue.Enqueue(a);
                }
                TempData["questionsTracNghiem"] = JsonConvert.SerializeObject(queue);


                //Tự luận:
                List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = new List<QuestionAndAnswerTrinhTuThaoTacVm>();
                TempData["QuestionAndAnswerTrinhTuThaoTac"] = JsonConvert.SerializeObject(listQuestionAndAnswerTrinhTuThaoTac);

                List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = new List<QuestionAndAnswerDiemChuYVm>();
                TempData["QuestionAndAnswerDiemChuY"] = JsonConvert.SerializeObject(listQuestionAndAnswerDiemChuY);

                List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = new List<QuestionAndAnswerLoiTaiCongDoanVm>();
                TempData["QuestionAndAnswerLoiTaiCongDoan"] = JsonConvert.SerializeObject(listQuestionAndAnswerLoiTaiCongDoan);

                List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = new List<QuestionAndAnswerDoiSachVm>();
                TempData["QuestionAndAnswerDoiSach"] = JsonConvert.SerializeObject(listQuestionAndAnswerDoiSach);

                var listTuLuan = await _cauHoiTuLuanApiClient.GetAllByCategory(idPhong);
                var dsCauHoiTuLuan = listTuLuan.ResultObj;
                dsCauHoiTuLuan = dsCauHoiTuLuan.OrderBy(x => random.Next()).ToList();
                List<CauHoiTuLuanVm> listQuestionTuLuan = dsCauHoiTuLuan;
                TempData["dsCauHoiTL"] = JsonConvert.SerializeObject(listQuestionTuLuan);

                TempData["danhsachcauhoiTuLuan"] = JsonConvert.SerializeObject(listQuestionTuLuan);
                TempData["soluongcauhoiTuLuan"] = listQuestionTuLuan.Count();
                Queue<CauHoiTuLuanVm> queueTuluan = new Queue<CauHoiTuLuanVm>();
                foreach (CauHoiTuLuanVm a in listQuestionTuLuan)
                {
                    queueTuluan.Enqueue(a);
                }
                TempData["questionsTuLuan"] = JsonConvert.SerializeObject(queueTuluan);


                //Common:
                var phongThi = await _categoryApiClient.GetById(idPhong);
                int thoiGianThi = phongThi.ResultObj.Time;
                int totalQuestion = listQuestionTracNghiem.Count() + listQuestionTuLuan.Count();
                TempData["totalQuestion"] = totalQuestion;
                TempData["idPhong"] = idPhong;
                TempData["thoiGianThi"] = thoiGianThi;

                TempData["score"] = 0;
                TempData["numQuestionTracnghiem"] = 0;
                TempData["numQuestionTuLuan"] = 0;
                TempData["StatusTN"] = 0;
                TempData["StatusTL"] = 0;

                TempData.Keep();
                return Json(new { success = true });
            }
        }

        //Trang làm bài thi:
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            int idPhong = Convert.ToInt32(TempData["idPhong"].ToString());
            var phongThi = await _categoryApiClient.GetById(idPhong);
            string tenPhongthi = phongThi.ResultObj.Name.ToString();
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();

            var session = _httpContextAccessor.HttpContext.Session;
            var chedothi = session.GetString("Chedothi");
            ViewBag.chedothi = chedothi;

            ViewBag.thisPage = tenPhongthi;
            TempData["Tenphongthi"] = tenPhongthi;
            ViewBag.totalQuestion = TempData["totalQuestion"];
            ViewBag.thoiGianThi = TempData["thoiGianThi"];
            TempData.Keep();
            return View();
        }

        //Trang làm bài thi Trắc nghiệm:
        [HttpGet]
        public async Task<IActionResult> TestingTracNghiem()
        {
            int totalTracNghiem = Convert.ToInt32(TempData["soluongcauhoiTracNghiem"].ToString());
            int totalTuLuan = Convert.ToInt32(TempData["soluongcauhoiTuLuan"].ToString());

            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.totalQuestion = TempData["totalQuestion"];
            int idPhong = Convert.ToInt32(TempData["idPhong"].ToString());
            var phongThi = await _categoryApiClient.GetById(idPhong);
            ViewBag.thisPage = phongThi.ResultObj.Name.ToString();
            ViewBag.soluongcauhoiTracNghiem = totalTracNghiem;
            ViewBag.soluongcauhoiTuLuan = totalTuLuan;
            ViewBag.numQuestionTracnghiem = TempData["numQuestionTracnghiem"];

            QuestionVm q = null;

            string jsonQuestionsTracNghiem = TempData["questionsTracNghiem"].ToString();
            Queue<QuestionVm> qlistTracNghiem = JsonConvert.DeserializeObject<Queue<QuestionVm>>(jsonQuestionsTracNghiem);
            if (qlistTracNghiem.Count > 0)
            {
                q = qlistTracNghiem.Peek();// Nhìn vào phần tử ở đầu
                TempData.Keep();
                return PartialView("Testing/_examTracNghiem", q);
                //qlistTracNghiem.Dequeue(); // Loại bỏ phần tử ở đầu ( Cần sửa thành khi submit mới bỏ phần tử đầu đi)
                // Cập nhật TempData
                //TempData["questionsTracNghiem"] = JsonConvert.SerializeObject(qlistTracNghiem);
                //TempData.Keep();
            }
            else
            {
                TempData["StatusTN"] = 1;
                TempData.Keep();
                return PartialView("Testing/_finishTracNghiem"); //Khi hết câu hỏi trả về partial view báo đã trả lời hết
            }
        }

        //Trả lời trắc nghiệm:
        [HttpPost]
        public ActionResult TraloiTracNghiem(QuestionVm q, string answer)
        {
            string jsonlistQuestionAndAnswerTracNghiem = TempData["QuestionAndAnswerTracNghiem"].ToString();
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);
            var cauhoivacautraloiTracNghiem = new QuestionAndAnswerVm()
            {
                Id = q.Id,
                Text = q.Text,
                QA = q.QA,
                QB = q.QB,
                QC = q.QC,
                QD = q.QD,
                QCorrectAns = q.QCorrectAns,
                CategoryId = q.CategoryId,
                Score = q.Score,
                Answer = answer
            };
            listQuestionAndAnswerTracNghiem.Add(cauhoivacautraloiTracNghiem);
            TempData["QuestionAndAnswerTracNghiem"] = JsonConvert.SerializeObject(listQuestionAndAnswerTracNghiem);

            string jsonQuestionsTracNghiem = TempData["questionsTracNghiem"].ToString();
            Queue<QuestionVm> qlistTracNghiem = JsonConvert.DeserializeObject<Queue<QuestionVm>>(jsonQuestionsTracNghiem);
            qlistTracNghiem.Dequeue(); // Loại bỏ phần tử ở đầu ( Cần sửa thành khi submit mới bỏ phần tử đầu đi)                                       
            TempData["questionsTracNghiem"] = JsonConvert.SerializeObject(qlistTracNghiem);


            int count = Convert.ToInt32(TempData["numQuestionTracnghiem"].ToString()) + 1;
            TempData["numQuestionTracnghiem"] = count;

            TempData.Keep();
            return Json(new { success = true });
        }

        //Trang làm bài thi Tự luận:
        [HttpGet]
        public async Task<IActionResult> TestingTuLuan()
        {
            int totalTuLuan = Convert.ToInt32(TempData["soluongcauhoiTuLuan"].ToString());
            int idPhong = Convert.ToInt32(TempData["idPhong"].ToString());
            var phongThi = await _categoryApiClient.GetById(idPhong);
            ViewBag.thisPage = phongThi.ResultObj.Name.ToString();
            ViewBag.soluongcauhoiTuLuan = totalTuLuan;
            ViewBag.numQuestionTuLuan = TempData["numQuestionTuLuan"];
            CauHoiTuLuanVm q = null;
            string jsonQuestionsTuLuan = TempData["questionsTuLuan"].ToString();

            Queue<CauHoiTuLuanVm> qlistTuLuan = JsonConvert.DeserializeObject<Queue<CauHoiTuLuanVm>>(jsonQuestionsTuLuan);
            if (qlistTuLuan.Count > 0)
            {
                q = qlistTuLuan.Peek();

                int id = q.Id;
                //Lay ra duoc ca List<DiemChuYVm> va List<LoiTaiCongDoanVm> va List<DoiSachVm>:
                var danhsachcauhoitrinhtuthaotac = await _cauHoiTrinhTuThaoTacApiClient.GetAllByCauHoiTuLuan(id);
                List<CauHoiTrinhTuThaoTacVm> listdanhsachcauhoitrinhtuthaotac = danhsachcauhoitrinhtuthaotac.ResultObj;
                var random = new Random();
                listdanhsachcauhoitrinhtuthaotac = listdanhsachcauhoitrinhtuthaotac.OrderBy(x => random.Next()).ToList();
                ViewBag.listTrinhTuThaoTac = listdanhsachcauhoitrinhtuthaotac;

                TempData.Keep();
                return PartialView("Testing/_examTuLuan", q);
            }
            else
            {
                TempData["StatusTL"] = 1;
                TempData.Keep();
                return PartialView("Testing/_finishTuLuan");
            }
        }

        //Trả lời tự luận:
        public class TraloiTuLuanRequest
        {
            public List<AnswerTrinhTuThaoTacRequest> dataTTTT { get; set; }
            public List<AnswerOthersRequest> DataDCY { get; set; }
            public List<AnswerOthersRequest> DataLTCD { get; set; }
            public List<AnswerDoiSachRequest> DataDS { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> TraloiTuLuan([FromBody] TraloiTuLuanRequest request)
        {
            string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);

            string jsonlistQuestionAndAnswerDiemChuY = TempData["QuestionAndAnswerDiemChuY"].ToString();
            List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = JsonConvert.DeserializeObject<List<QuestionAndAnswerDiemChuYVm>>(jsonlistQuestionAndAnswerDiemChuY);

            string jsonlistQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonlistQuestionAndAnswerLoiTaiCongDoan);

            string jsonlistQuestionAndAnswerDoiSach = TempData["QuestionAndAnswerDoiSach"].ToString();
            List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = JsonConvert.DeserializeObject<List<QuestionAndAnswerDoiSachVm>>(jsonlistQuestionAndAnswerDoiSach);

            //Lấy ra câu hỏi tự luận đầu tiên trong Queue list và bỏ nó.
            string jsonQuestionsTuLuan = TempData["questionsTuLuan"].ToString();
            Queue<CauHoiTuLuanVm> qlistTuLuan = JsonConvert.DeserializeObject<Queue<CauHoiTuLuanVm>>(jsonQuestionsTuLuan);
            CauHoiTuLuanVm q = null;
            q = qlistTuLuan.Peek();
            int id = q.Id;
            string cauhoituluanText = q.Text;

            var resultTrinhTuThaoTac = await _cauHoiTrinhTuThaoTacApiClient.GetAllByCauHoiTuLuan(id);
            List<CauHoiTrinhTuThaoTacVm> listTrinhTuThaoTac = resultTrinhTuThaoTac.ResultObj;
            foreach (var item in listTrinhTuThaoTac)
            {
                var cauhoivacautraloiTTTT = new QuestionAndAnswerTrinhTuThaoTacVm()
                {
                    Id = item.Id,
                    CauHoiTuLuanText = cauhoituluanText,
                    Text = item.Text,
                    ThuTu = item.ThuTu,
                    Score = (float)(item.Score != null ? item.Score : 0),
                    CauHoiTuLuanId = item.CauHoiTuLuanId,
                    Answer = request.dataTTTT.Where(x => x.Id == item.Id).FirstOrDefault().ThuTu
                };
                listQuestionAndAnswerTrinhTuThaoTac.Add(cauhoivacautraloiTTTT);

                if (item.diemChuYs.Any())
                {
                    foreach (var dcy in item.diemChuYs)
                    {
                        var cauhoivacautraloiDCY = new QuestionAndAnswerDiemChuYVm()
                        {
                            Id = dcy.Id,
                            CauhoitrinhtuthaotacId = item.Id,
                            Text = dcy.Text,
                            Answer = request.DataDCY.Where(x => x.Id == dcy.Id).FirstOrDefault().Answer,
                            CorrectAnswer = item.ThuTu,
                            CauHoiTuLuanId = item.CauHoiTuLuanId,

                        };
                        listQuestionAndAnswerDiemChuY.Add(cauhoivacautraloiDCY);
                    }
                }

                if (item.loiTaiCongDoans.Any())
                {
                    foreach (var ltcd in item.loiTaiCongDoans)
                    {
                        var cauhoivacautraloiLTCD = new QuestionAndAnswerLoiTaiCongDoanVm()
                        {
                            Id = ltcd.Id,
                            CauhoitrinhtuthaotacId = item.Id,
                            Text = ltcd.Text,
                            Answer = request.DataLTCD.Where(x => x.Id == ltcd.Id).FirstOrDefault().Answer,
                            CauHoiTuLuanId = item.CauHoiTuLuanId,
                            CorrectAnswer = item.ThuTu
                        };
                        listQuestionAndAnswerLoiTaiCongDoan.Add(cauhoivacautraloiLTCD);

                        if (ltcd.doiSaches.Any())
                        {
                            foreach (var ds in ltcd.doiSaches)
                            {
                                string doisach = ltcd.doiSaches.Where(x => x.Id == ds.Id).FirstOrDefault().Text;
                                string cautraloi = request.DataDS.Where(x => x.Id == ds.Id).FirstOrDefault().Answer;
                                string dapandung = ltcd.Text;
                                var cauhoivacautraloiDS = new QuestionAndAnswerDoiSachVm()
                                {
                                    Id = ds.Id,
                                    LoiTaiCongDoanId = ltcd.Id,
                                    Text = ds.Text,
                                    Answer = cautraloi,
                                    CauHoiTuLuanId = item.CauHoiTuLuanId,
                                    CorrectAnswer = dapandung
                                };
                                listQuestionAndAnswerDoiSach.Add(cauhoivacautraloiDS);
                            }
                        }
                    }
                }
            }
            TempData["QuestionAndAnswerTrinhTuThaoTac"] = JsonConvert.SerializeObject(listQuestionAndAnswerTrinhTuThaoTac);
            TempData["QuestionAndAnswerDiemChuY"] = JsonConvert.SerializeObject(listQuestionAndAnswerDiemChuY);
            TempData["QuestionAndAnswerLoiTaiCongDoan"] = JsonConvert.SerializeObject(listQuestionAndAnswerLoiTaiCongDoan);
            TempData["QuestionAndAnswerDoiSach"] = JsonConvert.SerializeObject(listQuestionAndAnswerDoiSach);

            qlistTuLuan.Dequeue(); // Loại bỏ phần tử ở đầu                                    
            TempData["questionsTuLuan"] = JsonConvert.SerializeObject(qlistTuLuan);

            int count = Convert.ToInt32(TempData["numQuestionTuLuan"].ToString()) + 1;
            TempData["numQuestionTuLuan"] = count;

            TempData.Keep();
            return Json(new { success = true });
        }

        //Trang lịch sử câu trả lời
        [HttpGet]
        public async Task<IActionResult> GetLogQuestionAndAnswers()
        {
            string jsonlistQuestionAndAnswerTracNghiem = TempData["QuestionAndAnswerTracNghiem"].ToString();
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);

            string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);

            string jsonlistQuestionAndAnswerDiemChuY = TempData["QuestionAndAnswerDiemChuY"].ToString();
            List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = JsonConvert.DeserializeObject<List<QuestionAndAnswerDiemChuYVm>>(jsonlistQuestionAndAnswerDiemChuY);

            string jsonlistQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonlistQuestionAndAnswerLoiTaiCongDoan);

            string jsonlistQuestionAndAnswerDoiSach = TempData["QuestionAndAnswerDoiSach"].ToString();
            List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = JsonConvert.DeserializeObject<List<QuestionAndAnswerDoiSachVm>>(jsonlistQuestionAndAnswerDoiSach);

            ViewBag.listQuestionAndAnswerTrinhTuThaoTac = listQuestionAndAnswerTrinhTuThaoTac;
            ViewBag.listQuestionAndAnswerDiemChuY = listQuestionAndAnswerDiemChuY;
            ViewBag.listQuestionAndAnswerLoiTaiCongDoan = listQuestionAndAnswerLoiTaiCongDoan;
            ViewBag.listQuestionAndAnswerDoiSach = listQuestionAndAnswerDoiSach;

            TempData.Keep();
            return PartialView("Testing/_LogQuestionAndAnswers", listQuestionAndAnswerTracNghiem);
        }

        //Trang sửa câu trả lời bài thi Tự luận:
        [HttpGet]
        public async Task<IActionResult> ChangeChooseTuLuan(int cauhoituluanId)
        {
            string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);
            var result = listQuestionAndAnswerTrinhTuThaoTac.Where(x => x.CauHoiTuLuanId == cauhoituluanId).ToList();
            TempData.Keep();
            return PartialView("Testing/_thayDoiDapAnCauHoiTrinhTuThaoTac", result);
        }

        //Sửa câu trả lời bài thi Tự luận:
        [HttpPost]
        public async Task<IActionResult> PostChangeChooseTuLuan([FromBody] List<ChangeAnswerTrinhTuThaoTacRequest> request)
        {
            string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);
            var listToChange = listQuestionAndAnswerTrinhTuThaoTac.Where(x => x.CauHoiTuLuanId == request.FirstOrDefault().CauHoiTuLuanId).ToList();
            foreach (var item in listToChange)
            {
                item.Answer = request.Where(x => x.Id == item.Id).FirstOrDefault().ThuTu;
            }
            TempData["QuestionAndAnswerTrinhTuThaoTac"] = JsonConvert.SerializeObject(listToChange);
            TempData.Keep();
            return Json(new { success = true });
        }


        //Trang sửa câu trả lời phần Điểm chú ý:
        [HttpGet]
        public async Task<IActionResult> ChangeChooseDCY(int cauhoituluanId)
        {
            string jsonlistQuestionAndAnswerDiemChuY = TempData["QuestionAndAnswerDiemChuY"].ToString();
            List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = JsonConvert.DeserializeObject<List<QuestionAndAnswerDiemChuYVm>>(jsonlistQuestionAndAnswerDiemChuY);
            var result = listQuestionAndAnswerDiemChuY.Where(x => x.CauHoiTuLuanId == cauhoituluanId).ToList();
            TempData.Keep();
            return PartialView("Testing/_thayDoiDapAnDCY", result);
        }
        [HttpPost]
        public async Task<IActionResult> PostChangeChooseDCY([FromBody] List<ChangeAnswerDiemChuYRequest> request)
        {
            string jsonlistQuestionAndAnswerDiemChuY = TempData["QuestionAndAnswerDiemChuY"].ToString();
            List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = JsonConvert.DeserializeObject<List<QuestionAndAnswerDiemChuYVm>>(jsonlistQuestionAndAnswerDiemChuY);

            var listToChange = listQuestionAndAnswerDiemChuY.Where(x => x.CauHoiTuLuanId == request.FirstOrDefault().CauHoiTuLuanId).ToList();
            foreach (var item in listToChange)
            {
                item.Answer = request.Where(x => x.Id == item.Id).FirstOrDefault().Answer;
            }
            TempData["QuestionAndAnswerDiemChuY"] = JsonConvert.SerializeObject(listToChange);
            TempData.Keep();
            return Json(new { success = true });
        }

        //Trang sửa câu trả lời phần Lỗi tại công đoạn:
        [HttpGet]
        public async Task<IActionResult> ChangeChooseLTCD(int cauhoituluanId)
        {
            string jsonQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonQuestionAndAnswerLoiTaiCongDoan);
            var result = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauHoiTuLuanId == cauhoituluanId).ToList();
            TempData.Keep();
            return PartialView("Testing/_thayDoiDapAnLTCD", result);
        }
        [HttpPost]
        public async Task<IActionResult> PostChangeChooseLTCD([FromBody] List<ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.ChangeAnswerLoiTaiCongDoanRequest> request)
        {
            string jsonlistQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonlistQuestionAndAnswerLoiTaiCongDoan);

            var listToChange = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauHoiTuLuanId == request.FirstOrDefault().CauHoiTuLuanId).ToList();
            foreach (var item in listToChange)
            {
                item.Answer = request.Where(x => x.Id == item.Id).FirstOrDefault().Answer;
            }
            TempData["QuestionAndAnswerLoiTaiCongDoan"] = JsonConvert.SerializeObject(listToChange);
            TempData.Keep();
            return Json(new { success = true });
        }

        //Trang sửa câu trả lời phần Đối sách:
        [HttpGet]
        public async Task<IActionResult> ChangeChooseDS(int cauhoituluanId)
        {
            string jsonQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonQuestionAndAnswerLoiTaiCongDoan);
            var getSelectListLTCDFromId = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauHoiTuLuanId == cauhoituluanId).ToList();
            var selectListLTCD = new SelectList(getSelectListLTCDFromId, "Id", "Text");
            ViewBag.selectListLTCDFromId = selectListLTCD;

            string jsonQuestionAndAnswerDoiSach = TempData["QuestionAndAnswerDoiSach"].ToString();
            List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = JsonConvert.DeserializeObject<List<QuestionAndAnswerDoiSachVm>>(jsonQuestionAndAnswerDoiSach);
            var result = listQuestionAndAnswerDoiSach.Where(x => x.CauHoiTuLuanId == cauhoituluanId).ToList();

            TempData.Keep();
            return PartialView("Testing/_thayDoiDapAnDS", result);
        }
        [HttpPost]
        public async Task<IActionResult> PostChangeChooseDS([FromBody] List<ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach.ChangeAnswerDoiSachRequest> request)
        {
            string jsonlistQuestionAndAnswerDoiSach = TempData["QuestionAndAnswerDoiSach"].ToString();
            List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = JsonConvert.DeserializeObject<List<QuestionAndAnswerDoiSachVm>>(jsonlistQuestionAndAnswerDoiSach);

            var listToChange = listQuestionAndAnswerDoiSach.Where(x => x.CauHoiTuLuanId == request.FirstOrDefault().CauHoiTuLuanId).ToList();
            foreach (var item in listToChange)
            {
                item.Answer = request.Where(x => x.Id == item.Id).FirstOrDefault().Answer;
            }
            TempData["QuestionAndAnswerDoiSach"] = JsonConvert.SerializeObject(listToChange);
            TempData.Keep();
            return Json(new { success = true });
        }


        [HttpGet]
        public IActionResult GetTempDataStatusValue()
        {
            var tempDataTNValue = Convert.ToInt32(TempData["StatusTN"].ToString());
            var tempDataTLValue = Convert.ToInt32(TempData["StatusTL"].ToString());
            TempData.Keep();

            if (tempDataTNValue == 1 && tempDataTLValue == 1)
            {
                string value = "done";
                return Json(value);
            }
            else
            {
                string value = "notyet";
                return Json(value);
            }
        }
        //Đổi lịch sử câu trả lời
        [HttpPost]
        public IActionResult ChangeAnswer(int id, string newAns)
        {
            string jsonlistQuestionAndAnswerTracNghiem = TempData.Peek("QuestionAndAnswerTracNghiem")?.ToString();
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);
            var question = listQuestionAndAnswerTracNghiem.Where(x => x.Id == id).FirstOrDefault();
            if (question != null)
            {
                question.Answer = newAns;
            }
            TempData["QuestionAndAnswerTracNghiem"] = JsonConvert.SerializeObject(listQuestionAndAnswerTracNghiem);
            TempData.Keep();
            return Json(new { success = true });
        }


        //Lưu kết quả
        [HttpPost]
        public async Task<IActionResult> Save(string time)
        {
            //int chedothi = Convert.ToInt32(TempData["Chedothithu"].ToString());
            var session = _httpContextAccessor.HttpContext.Session;
            var chedothi = session.GetString("Chedothi");

            if (chedothi == "thithu") //thi thử
            {
                int thoiGianChoPhepLamBai = Convert.ToInt32(TempData["thoiGianThi"].ToString());
                int thoiGianConLai = Convert.ToInt32(time);
                int thoiGianLamBai = thoiGianChoPhepLamBai - (thoiGianConLai / 60 / 1000);
                TempData["thoiGianLamBai"] = thoiGianLamBai;
                TempData.Keep();
                return Json(new { success = true });
            }
            else //thi thật
            {
                int thoiGianChoPhepLamBai = Convert.ToInt32(TempData["thoiGianThi"].ToString());
                int thoiGianConLai = Convert.ToInt32(time);
                int thoiGianLamBai = thoiGianChoPhepLamBai - (thoiGianConLai / 60 / 1000);
                TempData["thoiGianLamBai"] = thoiGianLamBai;
                Guid id = Guid.Empty;
                var userIdString = User.FindFirstValue("UserId");
                if (Guid.TryParse(userIdString, out var userId))
                {
                    id = userId;
                };
                int idPhong = Convert.ToInt32(TempData["idPhong"].ToString());
                string tenPhongthi = TempData["Tenphongthi"].ToString();

                //Tính điểm phần thi trắc nghiệm
                float totalScoreTracNghiem = 0;
                float scoreTracNghiem = 0;
                int slTraLoiDungTracNghiem = 0;
                string jsonlistQuestionAndAnswerTracNghiem = TempData.Peek("QuestionAndAnswerTracNghiem")?.ToString();
                List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);
                foreach (var item in listQuestionAndAnswerTracNghiem)
                {
                    totalScoreTracNghiem += item.Score ?? 0;
                    if (item.Answer == item.QCorrectAns.ToUpper())
                    {
                        scoreTracNghiem += item.Score ?? 0;
                        slTraLoiDungTracNghiem++;
                    }
                }
                //Tính điểm phần thi tự luận
                float totalScoreTuLuan = 0;
                float scoreTuluan = 0;
                int slTraLoiDungTuLuan = 0;

                string jsonQuestionsTuLuan = TempData["dsCauHoiTL"].ToString();
                List<CauHoiTuLuanVm> listQuestionsTuLuan = JsonConvert.DeserializeObject<List<CauHoiTuLuanVm>>(jsonQuestionsTuLuan);

                //Trình tự thao tác
                string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
                List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);
                //Điểm chú ý
                string jsonlistQuestionAndAnswerDiemChuY = TempData["QuestionAndAnswerDiemChuY"].ToString();
                List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = JsonConvert.DeserializeObject<List<QuestionAndAnswerDiemChuYVm>>(jsonlistQuestionAndAnswerDiemChuY);
                //Lỗi tại công đoạn
                string jsonQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
                List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonQuestionAndAnswerLoiTaiCongDoan);
                //Đối sách
                string jsonQuestionAndAnswerDoiSach = TempData["QuestionAndAnswerDoiSach"].ToString();
                List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = JsonConvert.DeserializeObject<List<QuestionAndAnswerDoiSachVm>>(jsonQuestionAndAnswerDoiSach);

                var groupListQuestionAndAnswerTrinhTuThaoTac = listQuestionAndAnswerTrinhTuThaoTac.GroupBy(x => x.CauHoiTuLuanId);
                foreach (var group in groupListQuestionAndAnswerTrinhTuThaoTac)
                {
                    foreach (var itemtttt in group)
                    {
                        float scoreCauhoiTTTT = itemtttt?.Score != null ? itemtttt.Score : 0;
                        totalScoreTuLuan += scoreCauhoiTTTT;
                        if (itemtttt.Answer == itemtttt.ThuTu)
                        {
                            var listDCY = listQuestionAndAnswerDiemChuY.Where(x => x.CauhoitrinhtuthaotacId == itemtttt.Id).ToList();
                            if (listDCY.Count > 0)
                            {
                                if (listDCY.All(item => item.Answer == item.CorrectAnswer))
                                {
                                    var listLTCD = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauhoitrinhtuthaotacId == itemtttt.Id).ToList();
                                    if (listLTCD.Count > 0)
                                    {
                                        if (listLTCD.All(item => item.Answer == item.CorrectAnswer))
                                        {
                                            foreach (var itemLTCD in listLTCD)
                                            {
                                                var listDS = listQuestionAndAnswerDoiSach.Where(x => x.LoiTaiCongDoanId == itemLTCD.Id).ToList();
                                                if (listDS.Count > 0)
                                                {
                                                    if (listDS.All(item => item.Answer == item.CorrectAnswer))
                                                    {
                                                        scoreTuluan += scoreCauhoiTTTT;
                                                        slTraLoiDungTuLuan++;
                                                    }
                                                }
                                                else
                                                {
                                                    scoreTuluan += scoreCauhoiTTTT;
                                                    slTraLoiDungTuLuan++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        scoreTuluan += scoreCauhoiTTTT;
                                        slTraLoiDungTuLuan++;
                                    }
                                }
                            }
                            else
                            {
                                scoreTuluan += scoreCauhoiTTTT;
                                slTraLoiDungTuLuan++;
                            }
                        }
                    }
                }
                #region old code
                //if (group.All(item => item.Answer == item.ThuTu))
                //{
                //    var listDCY = listQuestionAndAnswerDiemChuY.Where(x => x.CauHoiTuLuanId == cauhoituluan.Id).ToList();
                //    if (listDCY.All(item => item.Answer == item.CorrectAnswer))
                //    {
                //        var listLTCD = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauHoiTuLuanId == cauhoituluan.Id).ToList();
                //        if (listLTCD.All(item => item.Answer == item.CorrectAnswer))
                //        {
                //            var listDS = listQuestionAndAnswerDoiSach.Where(x => x.CauHoiTuLuanId == cauhoituluan.Id).ToList();
                //            if (listDS.All(item => item.Answer == item.CorrectAnswer))
                //            {
                //                scoreTuluan += scoreCauhoituluan;
                //                slTraLoiDungTuLuan++;
                //            }
                //        }
                //    }                       
                //}
                #endregion

                //Lưu lịch sử bài thi
                float totalScore = scoreTuluan + scoreTracNghiem;
                var request = new ExamResultCreateRequest()
                {
                    UserId = id,
                    CategoryId = idPhong,
                    CategoryName = tenPhongthi,
                    Score = totalScore,
                    ThoiGianLamBai = thoiGianLamBai,
                    ThoiGianChoPhepLamBai = thoiGianChoPhepLamBai
                };

                var result = await _examResultApiClient.Create(request);
                if (!result.IsSuccessed)
                {
                    return Json(new { success = false });
                }
                //Lấy Id
                int examResultId = result.Id ?? -1;
                if (examResultId == -1)
                {
                    return Json(new { success = false });
                }

                //Lưu lịch sử chi tiết các câu hỏi và đáp án
                //Phần trắc nghiệm
                List<LogExamCreateRequest> listlichsuTN = new List<LogExamCreateRequest>();
                foreach (var item in listQuestionAndAnswerTracNghiem)
                {
                    float diem = 0;
                    if (item.Answer == item.QCorrectAns.ToUpper())
                    {
                        diem = item.Score ?? 0;
                    }
                    var requestLogExam = new LogExamCreateRequest()
                    {
                        ExamResultId = examResultId,
                        LoaiCauHoi = "TN",
                        Cauhoi = item.Text,
                        QA = item.QA,
                        QB = item.QB,
                        QC = item.QC,
                        QD = item.QD,
                        Cautraloi = item.Answer,
                        Dapandung = item.QCorrectAns,
                        Score = item.Score ?? 0,
                        FinalScore = diem
                    };
                    listlichsuTN.Add(requestLogExam);
                }
                var resultluulsTN = await _logExamApiClient.CreateList(listlichsuTN);
                if (!resultluulsTN.IsSuccessed)
                {
                    return Json(new { success = false });
                }

                //Phần tự luận
                foreach (var item in listQuestionsTuLuan) // Voi moi cau hoi trong list thi:
                {
                    float diem = 0;
                    float cauhoituluanScore = 0;
                    //Dung het thi tinh diem:
                    List<QuestionAndAnswerTrinhTuThaoTacVm> group = listQuestionAndAnswerTrinhTuThaoTac.Where(x => x.CauHoiTuLuanId == item.Id).ToList();
                    foreach (var itemtttt in group)
                    {
                        cauhoituluanScore += itemtttt.Score;
                        if (itemtttt.Answer == itemtttt.ThuTu)
                        {
                            var listDCY = listQuestionAndAnswerDiemChuY.Where(x => x.CauhoitrinhtuthaotacId == itemtttt.Id).ToList();
                            if (listDCY.Count > 0)
                            {
                                if (listDCY.All(item => item.Answer == item.CorrectAnswer))
                                {
                                    var listLTCD = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauhoitrinhtuthaotacId == itemtttt.Id).ToList();
                                    if (listLTCD.Count > 0)
                                    {
                                        if (listLTCD.All(item => item.Answer == item.CorrectAnswer))
                                        {
                                            foreach (var itemLTCD in listLTCD)
                                            {
                                                var listDS = listQuestionAndAnswerDoiSach.Where(x => x.LoiTaiCongDoanId == itemLTCD.Id).ToList();
                                                if (listDS.Count > 0)
                                                {
                                                    if (listDS.All(item => item.Answer == item.CorrectAnswer))
                                                    {
                                                        diem += itemtttt.Score;
                                                    }
                                                }
                                                else
                                                {
                                                    diem += itemtttt.Score;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        diem += itemtttt.Score;
                                    }
                                }
                            }
                            else
                            {
                                diem += itemtttt.Score;
                            }
                        }
                    }

                    var requestCauHoiTuLuan = new LogExamCreateRequest()
                    {
                        ExamResultId = examResultId,
                        LoaiCauHoi = "TL",
                        Cauhoi = item.Text,
                        Score = cauhoituluanScore,
                        FinalScore = diem
                    };
                    var resultluucauhoiTL = await _logExamApiClient.CreateSingle(requestCauHoiTuLuan);
                    if (!resultluucauhoiTL.IsSuccessed)
                    {
                        return Json(new { success = false });
                    }

                    //Lấy Id cua cau hoi vua luu log
                    int logEId = resultluucauhoiTL.Id ?? -1;
                    if (logEId == -1)
                    {
                        return Json(new { success = false });
                    }

                    //Luu trinh tu thao tac
                    List<LogExamTrinhtuthaotacCreateRequest> listlogtttt = new List<LogExamTrinhtuthaotacCreateRequest>();
                    foreach (var itemTTTT in group)
                    {
                        float finalScr = 0;
                        if (itemTTTT.Answer == itemTTTT.ThuTu)
                        {
                            var listDCY = listQuestionAndAnswerDiemChuY.Where(x => x.CauhoitrinhtuthaotacId == itemTTTT.Id).ToList();
                            if (listDCY.Count > 0)
                            {
                                if (listDCY.All(item => item.Answer == item.CorrectAnswer))
                                {
                                    var listLTCD = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauhoitrinhtuthaotacId == itemTTTT.Id).ToList();
                                    if (listLTCD.Count > 0)
                                    {
                                        if (listLTCD.All(item => item.Answer == item.CorrectAnswer))
                                        {
                                            foreach (var itemLTCD in listLTCD)
                                            {
                                                var listDS = listQuestionAndAnswerDoiSach.Where(x => x.LoiTaiCongDoanId == itemLTCD.Id).ToList();
                                                if (listDS.Count > 0)
                                                {
                                                    if (listDS.All(item => item.Answer == item.CorrectAnswer))
                                                    {
                                                        finalScr = itemTTTT.Score;
                                                    }
                                                }
                                                else
                                                {
                                                    finalScr = itemTTTT.Score;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        finalScr = itemTTTT.Score;
                                    }
                                }
                            }
                            else
                            {
                                finalScr = itemTTTT.Score;
                            }
                        }

                        var requestTTTT = new LogExamTrinhtuthaotacCreateRequest()
                        {
                            LogExamId = logEId,
                            Text = itemTTTT.Text,
                            Answer = itemTTTT.Answer,
                            ThuTu = itemTTTT.ThuTu,
                            Score = itemTTTT.Score,
                            FinalScore = finalScr
                        };
                        listlogtttt.Add(requestTTTT);
                    }
                    var resultluulistcauhoiTTTT = await _logExamTrinhtuthaotacApiClient.CreateList(listlogtttt);
                    if (!resultluulistcauhoiTTTT.IsSuccessed)
                    {
                        return Json(new { success = false });
                    }

                    //Luu diem chu y:
                    List<QuestionAndAnswerDiemChuYVm> groupDCY = listQuestionAndAnswerDiemChuY.Where(x => x.CauHoiTuLuanId == item.Id).ToList();
                    List<LogExamDiemChuYCreateRequest> listlogdcy = new List<LogExamDiemChuYCreateRequest>();

                    foreach (var itemDCY in groupDCY)
                    {
                        var requestDCY = new LogExamDiemChuYCreateRequest()
                        {
                            LogExamId = logEId,
                            Text = itemDCY.Text,
                            Answer = itemDCY.Answer,
                            CorrectAnswer = itemDCY.CorrectAnswer
                        };
                        listlogdcy.Add(requestDCY);
                    }
                    var resultluulistcauhoiDCY = await _logExamDCYApiClient.CreateList(listlogdcy);
                    if (!resultluulistcauhoiDCY.IsSuccessed)
                    {
                        return Json(new { success = false });
                    }

                    //Luu loi tai cong doan:
                    List<QuestionAndAnswerLoiTaiCongDoanVm> groupLTCD = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauHoiTuLuanId == item.Id).ToList();
                    List<LogExamLoiTaiCongDoanCreateRequest> listlogLTCD = new List<LogExamLoiTaiCongDoanCreateRequest>();

                    foreach (var itemLTCD in groupLTCD)
                    {
                        var requestLTCD = new LogExamLoiTaiCongDoanCreateRequest()
                        {
                            LogExamId = logEId,
                            Text = itemLTCD.Text,
                            Answer = itemLTCD.Answer,
                            CorrectAnswer = itemLTCD.CorrectAnswer
                        };
                        listlogLTCD.Add(requestLTCD);
                    }
                    var resultluulistcauhoiLTCD = await _logExamLTCDApiClient.CreateList(listlogLTCD);
                    if (!resultluulistcauhoiLTCD.IsSuccessed)
                    {
                        return Json(new { success = false });
                    }

                    //Luu doi sach:
                    List<QuestionAndAnswerDoiSachVm> groupDS = listQuestionAndAnswerDoiSach.Where(x => x.CauHoiTuLuanId == item.Id).ToList();
                    List<LogExamDoiSachCreateRequest> listlogDS = new List<LogExamDoiSachCreateRequest>();

                    foreach (var itemDS in groupDS)
                    {
                        var requestDS = new LogExamDoiSachCreateRequest()
                        {
                            LogExamId = logEId,
                            Text = itemDS.Text,
                            Answer = itemDS.Answer,
                            CorrectAnswer = itemDS.CorrectAnswer
                        };
                        listlogDS.Add(requestDS);
                    }
                    var resultluulistcauhoiDS = await _logExamDSApiClient.CreateList(listlogDS);
                    if (!resultluulistcauhoiDS.IsSuccessed)
                    {
                        return Json(new { success = false });
                    }
                }
                TempData.Keep();
                return Json(new { success = true });
            }
        }

        //Trang kết thúc
        [HttpGet]
        public async Task<IActionResult> ConfirmEndExam()
        {
            int thoiGianChoPhepLamBai = Convert.ToInt32(TempData["thoiGianThi"].ToString());
            ViewBag.tenPhongthi = TempData["Tenphongthi"].ToString();
            int idPhong = Convert.ToInt32(TempData["idPhong"].ToString());
            var phongThi = await _categoryApiClient.GetById(idPhong);
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = phongThi.ResultObj.Name.ToString();
            ViewBag.totalQuestion = TempData["totalQuestion"];
            ViewBag.thoiGianLamBai = TempData["thoiGianLamBai"];
            ViewBag.thoiGianChoPhepLamBai = thoiGianChoPhepLamBai;

            //Tính điểm phần thi trắc nghiệm
            float totalScoreTracNghiem = 0;
            float scoreTracNghiem = 0;
            int slTraLoiDungTracNghiem = 0;
            string jsonlistQuestionAndAnswerTracNghiem = TempData.Peek("QuestionAndAnswerTracNghiem")?.ToString();

            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);
            foreach (var item in listQuestionAndAnswerTracNghiem)
            {
                totalScoreTracNghiem += item.Score ?? 0;
                if (item.Answer == item.QCorrectAns.ToUpper())
                {
                    scoreTracNghiem += item.Score ?? 0;
                    slTraLoiDungTracNghiem++;
                }
            }

            //Tính điểm phần thi tự luận
            float totalScoreTuLuan = 0;
            float scoreTuluan = 0;
            int slTraLoiDungTuLuan = 0;

            string jsonQuestionsTuLuan = TempData["dsCauHoiTL"].ToString();
            List<CauHoiTuLuanVm> listQuestionsTuLuan = JsonConvert.DeserializeObject<List<CauHoiTuLuanVm>>(jsonQuestionsTuLuan);

            //Trình tự thao tác
            string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);
            //Điểm chú ý
            string jsonlistQuestionAndAnswerDiemChuY = TempData["QuestionAndAnswerDiemChuY"].ToString();
            List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = JsonConvert.DeserializeObject<List<QuestionAndAnswerDiemChuYVm>>(jsonlistQuestionAndAnswerDiemChuY);
            //Lỗi tại công đoạn
            string jsonQuestionAndAnswerLoiTaiCongDoan = TempData["QuestionAndAnswerLoiTaiCongDoan"].ToString();
            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = JsonConvert.DeserializeObject<List<QuestionAndAnswerLoiTaiCongDoanVm>>(jsonQuestionAndAnswerLoiTaiCongDoan);
            //Đối sách
            string jsonQuestionAndAnswerDoiSach = TempData["QuestionAndAnswerDoiSach"].ToString();
            List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = JsonConvert.DeserializeObject<List<QuestionAndAnswerDoiSachVm>>(jsonQuestionAndAnswerDoiSach);
            var groupListQuestionAndAnswerTrinhTuThaoTac = listQuestionAndAnswerTrinhTuThaoTac.GroupBy(x => x.CauHoiTuLuanId);
            foreach (var group in groupListQuestionAndAnswerTrinhTuThaoTac)
            {
                //Xử lý tính điểm:
                foreach (var itemtttt in group)
                {
                    float scoreCauhoiTTTT = itemtttt?.Score != null ? itemtttt.Score : 0;
                    totalScoreTuLuan += scoreCauhoiTTTT;
                    if (itemtttt.Answer == itemtttt.ThuTu)
                    {
                        var listDCY = listQuestionAndAnswerDiemChuY.Where(x => x.CauhoitrinhtuthaotacId == itemtttt.Id).ToList();
                        if (listDCY.Count > 0)
                        {
                            if (listDCY.All(item => item.Answer == item.CorrectAnswer))
                            {
                                var listLTCD = listQuestionAndAnswerLoiTaiCongDoan.Where(x => x.CauhoitrinhtuthaotacId == itemtttt.Id).ToList();
                                if (listLTCD.Count > 0)
                                {
                                    if (listLTCD.All(item => item.Answer == item.CorrectAnswer))
                                    {
                                        foreach (var itemLTCD in listLTCD)
                                        {
                                            var listDS = listQuestionAndAnswerDoiSach.Where(x => x.LoiTaiCongDoanId == itemLTCD.Id).ToList();
                                            if (listDS.Count > 0)
                                            {
                                                if (listDS.All(item => item.Answer == item.CorrectAnswer))
                                                {
                                                    scoreTuluan += scoreCauhoiTTTT;
                                                    slTraLoiDungTuLuan++;
                                                }
                                            }
                                            else
                                            {
                                                scoreTuluan += scoreCauhoiTTTT;
                                                slTraLoiDungTuLuan++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    scoreTuluan += scoreCauhoiTTTT;
                                    slTraLoiDungTuLuan++;
                                }
                            }
                        }
                        else
                        {
                            scoreTuluan += scoreCauhoiTTTT;
                            slTraLoiDungTuLuan++;
                        }
                    }
                }
            }

            float totalScore = scoreTuluan + scoreTracNghiem;
            ViewBag.tracnghiemScore = scoreTracNghiem;
            ViewBag.tuluanScore = scoreTuluan;
            ViewBag.totalTracNghiemScore = totalScoreTracNghiem;
            ViewBag.totalTuLuanScore = totalScoreTuLuan;

            ViewBag.totalScore = totalScore;
            ViewBag.slTraLoiDungTracNghiem = slTraLoiDungTracNghiem;
            ViewBag.slTraLoiDungTuLuan = slTraLoiDungTuLuan;
            ViewBag.slTotalTracNghiem = Convert.ToInt32(TempData["soluongcauhoiTracNghiem"].ToString());
            ViewBag.slTotalTuLuan = Convert.ToInt32(TempData["soluongcauhoiTuLuan"].ToString());

            ViewBag.dsTN = listQuestionAndAnswerTracNghiem;
            //Tu luan
            ViewBag.chTL = listQuestionsTuLuan;
            ViewBag.dsTL = listQuestionAndAnswerTrinhTuThaoTac;

            ViewBag.dsDCY = listQuestionAndAnswerDiemChuY;
            ViewBag.dsLTCD = listQuestionAndAnswerLoiTaiCongDoan;
            ViewBag.dsDS = listQuestionAndAnswerDoiSach;

            TempData.Keep();
            return View();
        }



        //CHế độ luyện tập
        [HttpPost]
        public async Task<IActionResult> SelectRoomTest(int idPhong, string categoryName)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            // Sử dụng session
            session.SetString("Chedothi", "thithu");
            //TempData["Chedothithu"] = 1;

            //Trắc nghiệm
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = new List<QuestionAndAnswerVm>();
            TempData["QuestionAndAnswerTracNghiem"] = JsonConvert.SerializeObject(listQuestionAndAnswerTracNghiem);
            var listTracNghiem = await _questionApiClient.GetAllByCategory(idPhong);
            var dsCauHoiTracNghiem = listTracNghiem.ResultObj;
            var random = new Random();
            dsCauHoiTracNghiem = dsCauHoiTracNghiem.OrderBy(x => random.Next()).ToList();
            List<QuestionVm> listQuestionTracNghiem = dsCauHoiTracNghiem;
            TempData["danhsachcauhoiTracNghiem"] = JsonConvert.SerializeObject(listQuestionTracNghiem);
            TempData["soluongcauhoiTracNghiem"] = listQuestionTracNghiem.Count();
            Queue<QuestionVm> queue = new Queue<QuestionVm>();
            foreach (QuestionVm a in listQuestionTracNghiem)
            {
                queue.Enqueue(a);
            }
            TempData["questionsTracNghiem"] = JsonConvert.SerializeObject(queue);

            //Tự luận:
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = new List<QuestionAndAnswerTrinhTuThaoTacVm>();
            TempData["QuestionAndAnswerTrinhTuThaoTac"] = JsonConvert.SerializeObject(listQuestionAndAnswerTrinhTuThaoTac);

            List<QuestionAndAnswerDiemChuYVm> listQuestionAndAnswerDiemChuY = new List<QuestionAndAnswerDiemChuYVm>();
            TempData["QuestionAndAnswerDiemChuY"] = JsonConvert.SerializeObject(listQuestionAndAnswerDiemChuY);

            List<QuestionAndAnswerLoiTaiCongDoanVm> listQuestionAndAnswerLoiTaiCongDoan = new List<QuestionAndAnswerLoiTaiCongDoanVm>();
            TempData["QuestionAndAnswerLoiTaiCongDoan"] = JsonConvert.SerializeObject(listQuestionAndAnswerLoiTaiCongDoan);

            List<QuestionAndAnswerDoiSachVm> listQuestionAndAnswerDoiSach = new List<QuestionAndAnswerDoiSachVm>();
            TempData["QuestionAndAnswerDoiSach"] = JsonConvert.SerializeObject(listQuestionAndAnswerDoiSach);

            var listTuLuan = await _cauHoiTuLuanApiClient.GetAllByCategory(idPhong);
            var dsCauHoiTuLuan = listTuLuan.ResultObj;
            dsCauHoiTuLuan = dsCauHoiTuLuan.OrderBy(x => random.Next()).ToList();
            List<CauHoiTuLuanVm> listQuestionTuLuan = dsCauHoiTuLuan;
            TempData["dsCauHoiTL"] = JsonConvert.SerializeObject(listQuestionTuLuan);

            TempData["danhsachcauhoiTuLuan"] = JsonConvert.SerializeObject(listQuestionTuLuan);
            TempData["soluongcauhoiTuLuan"] = listQuestionTuLuan.Count();
            Queue<CauHoiTuLuanVm> queueTuluan = new Queue<CauHoiTuLuanVm>();
            foreach (CauHoiTuLuanVm a in listQuestionTuLuan)
            {
                queueTuluan.Enqueue(a);
            }
            TempData["questionsTuLuan"] = JsonConvert.SerializeObject(queueTuluan);

            //Common:
            var phongThi = await _categoryApiClient.GetById(idPhong);
            int thoiGianThi = phongThi.ResultObj.Time;
            int totalQuestion = listQuestionTracNghiem.Count() + listQuestionTuLuan.Count();
            TempData["totalQuestion"] = totalQuestion;
            TempData["idPhong"] = idPhong;
            TempData["thoiGianThi"] = thoiGianThi;

            TempData["score"] = 0;
            TempData["numQuestionTracnghiem"] = 0;
            TempData["numQuestionTuLuan"] = 0;
            TempData["StatusTN"] = 0;
            TempData["StatusTL"] = 0;

            TempData.Keep();
            return Json(new { success = true });
        }

    }
}
