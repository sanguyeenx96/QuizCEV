using Application.Post.PostPosts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Category.Request;
using ViewModels.PostPosts.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostPostsController : Controller
    {
        private readonly IPostPostService _postPostService;
        public PostPostsController(IPostPostService postPostService)
        {
            _postPostService = postPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _postPostService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetAllByCategory/{id}")]
        public async Task<IActionResult> GetAllByCategory(int id)
        {
            var result = await _postPostService.GetAllByCategory(id);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _postPostService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostPostsCreateRequest request)
        {
            var result = await _postPostService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postPostService.Delete(id);
            return Ok(result);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateName(int id, PostPostsUpdateRequest request)
        {
            var result = await _postPostService.Update(id, request);
            return Ok(result);
        }

    }
}
