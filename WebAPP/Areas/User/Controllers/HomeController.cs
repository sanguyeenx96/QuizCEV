using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Question.Response;
using WebAPP.Services;

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

        public HomeController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient,
            ICauHoiTuLuanApiClient cauHoiTuLuanApiClient, ICauHoiTrinhTuThaoTacApiClient cauHoiTrinhTuThaoTacApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
            _cauHoiTuLuanApiClient = cauHoiTuLuanApiClient;
            _cauHoiTrinhTuThaoTacApiClient = cauHoiTrinhTuThaoTacApiClient;
        }

        //Chọn phòng thi:
        [HttpGet]
        public async Task<IActionResult> SelectRoom()
        {
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = "Bắt đầu kiểm tra";
            ViewBag.step1 = "Chọn phòng thi";

            var listCategory = await _categoryApiClient.GetAll();
            return View(listCategory.ResultObj);
        }

        //Lấy dữ liệu câu hỏi từ phòng thi:
        [HttpPost]
        public async Task<IActionResult> SelectRoom(int idPhong)
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
            var listTuLuan = await _cauHoiTuLuanApiClient.GetAllByCategory(idPhong);
            var dsCauHoiTuLuan = listTuLuan.ResultObj;
            dsCauHoiTuLuan = dsCauHoiTuLuan.OrderBy(x => random.Next()).ToList();
            List<CauHoiTuLuanVm> listQuestionTuLuan = dsCauHoiTuLuan;
            TempData["danhsachcauhoiTuLuan"] = JsonConvert.SerializeObject(listQuestionTuLuan);
            TempData["soluongcauhoiTuLuan"] = listQuestionTuLuan.Count();
            Queue<CauHoiTuLuanVm> queueTuluan = new Queue<CauHoiTuLuanVm>();
            foreach (CauHoiTuLuanVm a in listQuestionTuLuan)
            {
                queueTuluan.Enqueue(a);
            }
            TempData["questionsTuLuan"] = JsonConvert.SerializeObject(queueTuluan);


            //Common:
            int totalQuestion = listQuestionTracNghiem.Count() + listQuestionTuLuan.Count();
            TempData["totalQuestion"] = totalQuestion;
            TempData["idPhong"] = idPhong;
            TempData["score"] = 0;
            TempData["numQuestionTracnghiem"] = 0;
            TempData["numQuestionTuLuan"] = 0;

            TempData.Keep();
            return RedirectToAction("Test");
        }

        //Trang làm bài thi:
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            int idPhong = Convert.ToInt32(TempData["idPhong"].ToString());
            var phongThi = await _categoryApiClient.GetById(idPhong);
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();
            ViewBag.thisPage = phongThi.ResultObj.Name.ToString();
            ViewBag.totalQuestion = TempData["totalQuestion"];
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
                var resultTrinhTuThaoTac = await _cauHoiTrinhTuThaoTacApiClient.GetAllByCauHoiTuLuan(id);
                List<CauHoiTrinhTuThaoTacVm> listTrinhTuThaoTac = resultTrinhTuThaoTac.ResultObj;
                ViewBag.listTrinhTuThaoTac = listTrinhTuThaoTac;

                TempData.Keep();
                return PartialView("Testing/_examTuLuan", q);
            }
            else
            {
                TempData.Keep();
                return PartialView("Testing/_finishTuLuan");
            }
        }

        //Trả lời tự luận:
        [HttpPost]
        public async Task<IActionResult> TraloiTuLuan([FromBody] List<AnswerTrinhTuThaoTacRequest> request)
        {
            string jsonlistQuestionAndAnswerTrinhTuThaoTac = TempData["QuestionAndAnswerTrinhTuThaoTac"].ToString();
            List<QuestionAndAnswerTrinhTuThaoTacVm> listQuestionAndAnswerTrinhTuThaoTac = JsonConvert.DeserializeObject<List<QuestionAndAnswerTrinhTuThaoTacVm>>(jsonlistQuestionAndAnswerTrinhTuThaoTac);

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
                var cauhoivacautraloiTuluan = new QuestionAndAnswerTrinhTuThaoTacVm()
                {
                    Id = item.Id,
                    CauHoiTuLuanText = cauhoituluanText,
                    Text = item.Text,
                    ThuTu = item.ThuTu,
                    CauHoiTuLuanId = item.CauHoiTuLuanId,
                    Answer = request.Where(x => x.Id == item.Id).FirstOrDefault().ThuTu
                };
                listQuestionAndAnswerTrinhTuThaoTac.Add(cauhoivacautraloiTuluan);
            }
            TempData["QuestionAndAnswerTrinhTuThaoTac"] = JsonConvert.SerializeObject(listQuestionAndAnswerTrinhTuThaoTac);
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
            ViewBag.listQuestionAndAnswerTrinhTuThaoTac = listQuestionAndAnswerTrinhTuThaoTac;

            TempData.Keep();
            return PartialView("Testing/_LogQuestionAndAnswers", listQuestionAndAnswerTracNghiem);
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

        //Trang kết thúc
        [HttpGet]
        public async Task<IActionResult> ConfirmEndExam()
        {
            string jsonlistQuestionAndAnswerTracNghiem = TempData["QuestionAndAnswerTracNghiem"].ToString();
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);

            return View();
        }

    }
}
