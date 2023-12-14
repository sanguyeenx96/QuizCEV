using Application.CauHoiTrinhTuThaoTac.DiemChuY;
using Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.Common;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoiSachController : ControllerBase
    {
        private readonly IDoiSachService _doiSachService;
        public DoiSachController(IDoiSachService doiSachService)
        {
            _doiSachService = doiSachService;
        }

        [HttpGet("GetAllByLoiTaiCongDoanId/{id}")]
        public async Task<IActionResult> GetAllByLoiTaiCongDoanId(int id)
        {
            var result = await _doiSachService.GetAllByLoiTaiCongDoanId(id);
            return Ok(result);
        }

        [HttpPost("Create/{loitaicongdoanId}")]
        public async Task<IActionResult> Create(int loitaicongdoanId, DoiSachCreateRequest request)
        {
            var result = await _doiSachService.Create(loitaicongdoanId, request);
            return Ok(result);
        }

        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update(int id, DoiSachUpdateRequest request)
        {
            var result = await _doiSachService.Update(id, request);
            return Ok(result);
        }

        [HttpPatch("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doiSachService.Delete(id);
            return Ok(result);
        }
    }
}
