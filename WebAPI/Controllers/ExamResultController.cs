using Application.ExamResult;
using Application.ExamResult.ExportExcel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExportExcel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ExamResultController : ControllerBase
    {
        private readonly IExamResultService _examResultService;
        private readonly IExportExcelService _exportExcelService;
        public ExamResultController(IExamResultService examResultService,IExportExcelService exportExcelService)
        {
            _examResultService = examResultService;
            _exportExcelService = exportExcelService;
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search(ExamResultSearchRequest request)
        {
            var result = await _examResultService.Search(request);
            return Ok(result);
        }

        [HttpGet("GetAll")] 
        public async Task<IActionResult> Getall()
        {
            var result = await _examResultService.Getall();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamResultCreateRequest request)
        {
            var result = await _examResultService.Create(request);
            return Ok(result);
        }

        [HttpPost("checkretest")]
        public async Task<IActionResult> CheckRetest(ExamResultCheckRetestRequest request)
        {
            var result = await _examResultService.CheckReTest(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DanhGia(int id, ExamResultDanhGiaRequest request)
        {
            var result = await _examResultService.DanhGia(id, request);
            return Ok(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await _examResultService.Delete(id);
        //    return Ok(result);
        //}

        [HttpPost("SearchForExport")]
        public async Task<IActionResult> SearchForExport(ExamResultSearchRequest request)
        {
            var result = await _exportExcelService.Searchdata(request);
            return Ok(result);
        }

        [HttpPost("ExportExcelFile")]
        public async Task<IActionResult> Export([FromForm] IFormFile file, List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> request)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Tệp Excel không được gửi.");
            }
            using (var fileStream = file.OpenReadStream())
            {
                var result = await _exportExcelService.ExportExcelFile(fileStream, request);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
        }

    }
}
