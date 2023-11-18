using Application.CauHoiTrinhTuThaoTac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTuLuan.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CauHoiTrinhTuThaoTacController : ControllerBase
    {
        private readonly ICauHoiTrinhTuThaoTacService _cauHoiTrinhTuThaoTacService;
        public CauHoiTrinhTuThaoTacController(ICauHoiTrinhTuThaoTacService cauHoiTrinhTuThaoTacService)
        {
            _cauHoiTrinhTuThaoTacService = cauHoiTrinhTuThaoTacService;
        }


        [HttpGet("GetAllByCauHoiTuLuan/{id}")]
        public async Task<IActionResult> GetAllByCauHoiTuLuan(int id)
        {
            var result = await _cauHoiTrinhTuThaoTacService.GetAllByCauHoiTuLuan(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _cauHoiTrinhTuThaoTacService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CauHoiTrinhTuThaoTacCreateRequest request)
        {
            var result = await _cauHoiTrinhTuThaoTacService.Create(request);
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cauHoiTrinhTuThaoTacService.Delete(id);
            return Ok(result);
        }

        [HttpPut("updatescore/{id}")]
        public async Task<IActionResult> UpdateScore(int id, CauHoiTrinhTuThaoTacUpdateScoreRequest request)
        {
            var result = await _cauHoiTrinhTuThaoTacService.UpdateScore(id, request);
            return Ok(result);
        }

        [HttpGet("Count/{id}")]
        public async Task<IActionResult> Count(int id)
        {
            var result = await _cauHoiTrinhTuThaoTacService.Count(id);
            return Ok(result);
        }

        [HttpPut("ChangeOrder")]
        public async Task<IActionResult> ChangeOrder(List<CauHoiTrinhTuThaoTacChangeOrderRequest> request)
        {
            var result = await _cauHoiTrinhTuThaoTacService.ChangeOrder(request);
            return Ok(result);
        }

    }
}
