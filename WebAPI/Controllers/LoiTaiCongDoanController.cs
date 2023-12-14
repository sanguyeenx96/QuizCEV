using Application.CauHoiTrinhTuThaoTac.DiemChuY;
using Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.Common;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoiTaiCongDoanController : ControllerBase
    {
        private readonly ILoiTaiCongDoanService _loiTaiCongDoanService;
        public LoiTaiCongDoanController(ILoiTaiCongDoanService loiTaiCongDoanService)
        {
            _loiTaiCongDoanService = loiTaiCongDoanService;
        }

        [HttpGet("GetAllByCauHoiTrinhTuThaoTacId/{id}")]
        public async Task<IActionResult> GetAllByCauHoiTrinhTuThaoTacId(Guid id)
        {
            var result = await _loiTaiCongDoanService.GetAllByCauHoiTrinhTuThaoTacId(id);
            return Ok(result);
        }

        [HttpPost("Create/{cauhoitrinhtuthaotacId}")]
        public async Task<IActionResult> Create(Guid cauhoitrinhtuthaotacId, LoiTaiCongDoanCreateRequest request)
        {
            var result = await _loiTaiCongDoanService.Create(cauhoitrinhtuthaotacId, request);
            return Ok(result);
        }

        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update(int id, LoiTaiCongDoanUpdateRequest request)
        {
            var result = await _loiTaiCongDoanService.Update(id, request);
            return Ok(result);
        }

        [HttpPatch("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loiTaiCongDoanService.Delete(id);
            return Ok(result);
        }
    }
}
