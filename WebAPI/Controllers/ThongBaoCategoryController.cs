using Application.Post.PostCategory;
using Application.ThongBao.ThongBaoCategory;
using Application.ThongBao.ThongBaoPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.PostCategory.Request;
using ViewModels.ThongBaoCategory.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThongBaoCategoryController : Controller
    {
        private readonly IThongBaoCategoryService _thongBaoCategoryService;
        public ThongBaoCategoryController(IThongBaoCategoryService thongBaoCategoryService)
        {
            _thongBaoCategoryService = thongBaoCategoryService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _thongBaoCategoryService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _thongBaoCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThongBaoCategoryCreateRequest request)
        {
            var result = await _thongBaoCategoryService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _thongBaoCategoryService.Delete(id);
            return Ok(result);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateName(int id, ThongBaoCategoryUpdateRequest request)
        {
            var result = await _thongBaoCategoryService.Update(id, request);
            return Ok(result);
        }
    }
}
