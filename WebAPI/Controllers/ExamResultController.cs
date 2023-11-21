using Application.ExamResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.ExamResult.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : ControllerBase
    {
        private readonly IExamResultService _examResultService;
        public ExamResultController(IExamResultService examResultService)
        {
            _examResultService = examResultService;
        }

        //[HttpPost("Search")]
        //public async Task<IActionResult> Search( ExamResultSearchRequest request)
        //{
        //    var result = await _examResultService.Search(request);
        //    return Ok(result);
        //}

        //[HttpGet("GetAll")]
        //public async Task<IActionResult> Getall()
        //{
        //    var result = await _examResultService.Getall();
        //    return Ok(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(ExamResultCreateRequest request)
        //{
        //    var result = await _examResultService.Create(request);
        //    return Ok(result);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, ExamResultUpdateRequest request)
        //{
        //    var result = await _examResultService.Update(id,request);
        //    return Ok(result);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await _examResultService.Delete(id);
        //    return Ok(result);
        //}

    }
}
