using Application.LogExam;
using Application.LogExamDiemChuY;
using Application.LogExamDoiSach;
using Application.LogExamLoiTaiCongDoan;
using Application.LogExamTrinhtuthaotac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.LogExam.Request;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamDoiSach.Request;
using ViewModels.LogExamLoiTaiCongDoan.Request;
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
        private readonly ILogExamDiemChuYService _logExamDiemChuYService;
        private readonly ILogExamLoiTaiCongDoanService _logExamLoiTaiCongDoanService;
        private readonly ILogExamDoiSachService _logExamDoiSachService;

        public LogExamController(ILogExamService logExamService, ILogExamTrinhtuthaotacService logExamTrinhtuthaotacService,ILogExamDiemChuYService logExamDiemChuYService,
            ILogExamLoiTaiCongDoanService logExamLoiTaiCongDoanService, ILogExamDoiSachService logExamDoiSachService)
        {
            _logExamService = logExamService;
            _logExamTrinhtuthaotacService = logExamTrinhtuthaotacService;
            _logExamDiemChuYService = logExamDiemChuYService;
            _logExamLoiTaiCongDoanService = logExamLoiTaiCongDoanService;
            _logExamDoiSachService = logExamDoiSachService;
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

        [HttpPost("createlistDCY")]
        public async Task<IActionResult> CreateListDCY(List<LogExamDiemChuYCreateRequest> request)
        {
            var result = await _logExamDiemChuYService.CreateList(request);
            return Ok(result);
        }

        [HttpPost("createlistLTCD")]
        public async Task<IActionResult> CreateListLTCD(List<LogExamLoiTaiCongDoanCreateRequest> request)
        {
            var result = await _logExamLoiTaiCongDoanService.CreateList(request);
            return Ok(result);
        }

        [HttpPost("createlistDS")]
        public async Task<IActionResult> CreateListDS(List<LogExamDoiSachCreateRequest> request)
        {
            var result = await _logExamDoiSachService.CreateList(request);
            return Ok(result);
        }
    }
}
