using Application.LogExam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.LogExam.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogExamController : ControllerBase
    {
        private readonly ILogExamService _logExamService;
        public LogExamController(ILogExamService logExamService)
        {
            _logExamService = logExamService;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _logExamService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetByExamId/{id}")]
        public async Task<IActionResult> GetByExamId(int id)
        {
            var result = await _logExamService.GetByExamId(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LogExamCreateRequest request)
        {
            var result = await _logExamService.Create(request);
            return Ok(result);
        }
    }
}
