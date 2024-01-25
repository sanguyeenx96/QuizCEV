using Application.Post.PostPosts;
using Application.Read.ReadPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.PostPosts.Request;
using ViewModels.Read.ReadPost;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadPostController : ControllerBase
    {
        private readonly IReadPostService _readPostService;
        public ReadPostController(IReadPostService readPostService)
        {
            _readPostService = readPostService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _readPostService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetAllByCategory/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByCategory(int id)
        {
            var result = await _readPostService.GetAllByCategory(id);
            return Ok(result);
        }

        [HttpPost("UserGetAllByCategory/{id}")]
        public async Task<IActionResult> UserGetAllByCategory(int id , ReadPostUserGetAllByCategory request)
        {
            var result = await _readPostService.UserGetAllByCategory(id,request);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _readPostService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReadPostCreateRequest request)
        {
            var result = await _readPostService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _readPostService.Delete(id);
            return Ok(result);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateName(int id, ReadPostUpdateRequest request)
        {
            var result = await _readPostService.Update(id, request);
            return Ok(result);
        }
        [HttpPatch("updateThumbImage/{id}")]
        public async Task<IActionResult> UpdateThumbImage(int id, ReadPostThumbImageUpdate request)
        {
            var result = await _readPostService.UpdateThumbImage(id, request);
            return Ok(result);
        }

        [HttpPatch("updatestatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var result = await _readPostService.ChangeStatus(id);
            return Ok(result);
        }
    }
}
