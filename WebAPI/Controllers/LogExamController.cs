using Application.LogExam;
using Application.LogExamTrinhtuthaotac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.LogExam.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class LogExamController : ControllerBase
    {
        private readonly ILogExamService _logExamService;
        private readonly ILogExamTrinhtuthaotacService _logExamTrinhtuthaotacService;
        public LogExamController(ILogExamService logExamService, ILogExamTrinhtuthaotacService logExamTrinhtuthaotacService)
        {
            _logExamService = logExamService;
            _logExamTrinhtuthaotacService = logExamTrinhtuthaotacService;
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

        [HttpPost("createsingle")]
        public async Task<IActionResult> CreateSingle(LogExamCreateRequest request)
        {
            var result = await _logExamService.CreateSingle(request);
            return Ok(result);
        }


        [HttpPost("createlist")]
        public async Task<IActionResult> CreateList(List<LogExamCreateRequest> request)
        {
            var result = await _logExamService.CreateList(request);
            return Ok(result);
        }

        [HttpPost("createlistTTTT")]
        public async Task<IActionResult> CreateListTTTT(List<LogExamTrinhtuthaotacCreateRequest> request)
        {
            var result = await _logExamTrinhtuthaotacService.CreateList(request);
            return Ok(result);
        }
    }
}
