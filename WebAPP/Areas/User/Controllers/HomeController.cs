using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        public HomeController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
        }

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
            var result = await _questionApiClient.GetAllByCategory(idPhong);
            var random = new Random();
            var dsCauHoi = result.ResultObj;
            dsCauHoi = dsCauHoi.OrderBy(x => random.Next()).ToList();

            List<QuestionVm> listQuestion = dsCauHoi;
            TempData["danhsachcauhoi"] = listQuestion;
            Queue<QuestionVm> queue = new Queue<QuestionVm>();
            foreach (QuestionVm a in listQuestion)
            {
                queue.Enqueue(a);
            }
            //TempData["questions"] = queue;
            TempData.Keep();
            return RedirectToAction("Testing");
        }

        [HttpGet]
        public async Task<IActionResult> Testing()
        {
            ViewBag.hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            ViewBag.bophan = User.FindFirst(ClaimTypes.Country).Value.ToString();

            QuestionVm q = null;
            if (TempData["questions"] != null)
            {
                Queue<QuestionVm> qlist = (Queue<QuestionVm>)TempData["questions"];
                if (qlist.Count > 0)
                {
                    q = qlist.Peek();
                    qlist.Dequeue();
                    TempData["questions"] = qlist;
                    TempData.Keep();
                }
                else
                {
                    return RedirectToAction("EndExam");
                }
            }
            else
            {
                return RedirectToAction("SelectRoom");
            }
            TempData.Keep();
            return View(q);

            //var phongThi = await _categoryApiClient.GetById(idPhong);
            //ViewBag.thisPage = phongThi.ResultObj.Name.ToString();
            //var questions = await _questionApiClient.GetAllByCategory(idPhong);
            //var dsCauHoi = questions.ResultObj.ToList();

            //// Trộn ngẫu nhiên danh sách câu hỏi
            //var random = new Random();
            //dsCauHoi = dsCauHoi.OrderBy(x => random.Next()).ToList();
        }


    }
}
