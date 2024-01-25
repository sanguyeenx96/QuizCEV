using Application.ExamResult;
using Application.Read.ReadResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.ExamResult.Request;
using ViewModels.Read.ReadResult;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadResultController : ControllerBase
    {
        private readonly IReadResultService _readResultService;
        public ReadResultController(IReadResultService readResultService)
        {
            _readResultService = readResultService;
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search(ReadResultSearchRequest request)
        {
            var result = await _readResultService.Search(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReadResultCreateRequest request)
        {
            var result = await _readResultService.Create(request);
            return Ok(result);
        }

      
    }
}
