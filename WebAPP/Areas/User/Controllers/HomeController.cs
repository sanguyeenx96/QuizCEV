using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json;
using System.Security.Claims;
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
        public HomeController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient, ICauHoiTuLuanApiClient cauHoiTuLuanApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
            _cauHoiTuLuanApiClient = cauHoiTuLuanApiClient;
        }

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



            int totalQuestion = listQuestionTracNghiem.Count() + listQuestionTuLuan.Count();
            TempData["totalQuestion"] = totalQuestion;
            TempData["idPhong"] = idPhong;
            TempData["score"] = 0;
            TempData["numQuestion"] = 0;
            TempData.Keep();
            return RedirectToAction("Test");
        }

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
            ViewBag.numQuestion = TempData["numQuestion"];

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
                return PartialView("Testing/_examTracNghiem", q); //Khi hết câu hỏi trả về partial view báo đã trả lời hết
            }            
        }

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

            int count = Convert.ToInt32(TempData["numQuestion"].ToString()) + 1;
            TempData["numQuestion"] = count;

            TempData.Keep();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetLogQuestionAndAnswers()
        {
            string jsonlistQuestionAndAnswerTracNghiem = TempData["QuestionAndAnswerTracNghiem"].ToString();
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);
            TempData.Keep();
            return PartialView("Testing/_LogQuestionAndAnswers", listQuestionAndAnswerTracNghiem);
        }

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

        [HttpGet]
        public async Task<IActionResult> ConfirmEndExam()
        {
            string jsonlistQuestionAndAnswerTracNghiem = TempData["QuestionAndAnswerTracNghiem"].ToString();
            List<QuestionAndAnswerVm> listQuestionAndAnswerTracNghiem = JsonConvert.DeserializeObject<List<QuestionAndAnswerVm>>(jsonlistQuestionAndAnswerTracNghiem);

            return View();
        }

    }
}
