using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.Question.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IQuestionApiClient _questionApiClient;

        public QuestionController(ICategoryApiClient categoryApiClient, IQuestionApiClient questionApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _questionApiClient = questionApiClient;
        }

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

        [HttpPost]
        public async Task<IActionResult> GetAllByCategoryId(int id)
        {
            var count = await _questionApiClient.Count(id);
            ViewBag.countTracNghiem = count.ResultObj;
            var result = await _questionApiClient.GetAllByCategory(id);
            return PartialView( "_questionList",result.ResultObj);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.thisPage = "Thêm mới câu hỏi";
            var listcategory = await _categoryApiClient.GetAll();
            ViewBag.listcat = listcategory.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateManual( QuestionCreateRequest request)
        {
            var result = await _questionApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = result.Message });
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



    }
}
