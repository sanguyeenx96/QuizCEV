using Application.Post.PostPosts;
using Application.ThongBao.ThongBaoPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.PostPosts.Request;
using ViewModels.ThongBaoPost.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThongBaoPostController : Controller
    {
        private readonly IThongBaoPostService _thongBaoPostService;
        public ThongBaoPostController(IThongBaoPostService thongBaoPostService)
        {
            _thongBaoPostService = thongBaoPostService;
        }

        [HttpGet("get6")]
        [AllowAnonymous]
        public async Task<IActionResult> Get6()
        {
            var result = await _thongBaoPostService.Get6();
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _thongBaoPostService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetAllByCategory/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByCategory(int id)
        {
            var result = await _thongBaoPostService.GetAllByCategory(id);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _thongBaoPostService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThongBaoPostCreateRequest request)
        {
            var result = await _thongBaoPostService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _thongBaoPostService.Delete(id);
            return Ok(result);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateName(int id, ThongBaoPostUpdateRequest request)
        {
            var result = await _thongBaoPostService.Update(id, request);
            return Ok(result);
        }
    }
}
