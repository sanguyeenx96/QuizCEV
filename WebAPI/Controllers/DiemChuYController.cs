using Application.CauHoiTrinhTuThaoTac;
using Application.CauHoiTrinhTuThaoTac.DiemChuY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiemChuYController : ControllerBase
    {
        private readonly IDiemChuYService _diemChuYService;
        public DiemChuYController(IDiemChuYService diemChuYService)
        {
            _diemChuYService = diemChuYService;
        }

        [HttpGet("GetAllByCauHoiTrinhTuThaoTacId/{id}")]
        public async Task<IActionResult> GetAllByCauHoiTrinhTuThaoTacId(Guid id)
        {
            var result = await _diemChuYService.GetAllByCauHoiTrinhTuThaoTacId(id);
            return Ok(result);
        }

        [HttpPost("Create/{cauhoitrinhtuthaotacId}")]
        public async Task<IActionResult> Create(Guid cauhoitrinhtuthaotacId, DiemChuYCreateRequest request)
        {
            var result = await _diemChuYService.Create(cauhoitrinhtuthaotacId,request);
            return Ok(result);
        }

        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update(int id, DiemChuYUpdateRequest request)
        {
            var result = await _diemChuYService.Update(id, request);
            return Ok(result);
        }

        [HttpPatch("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _diemChuYService.Delete(id);
            return Ok(result);
        }

    }
}
