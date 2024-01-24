using Application.Category;
using Application.Read.ReadCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Category.Request;
using ViewModels.Read.ReadCategory;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadCategoryController : ControllerBase
    {
        private readonly IReadCategoryService _readCategoryService;
        public ReadCategoryController(IReadCategoryService readCategoryService)
        {
            _readCategoryService = readCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _readCategoryService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _readCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReadCategoryCreateRequest request)
        {
            var result = await _readCategoryService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _readCategoryService.Delete(id);
            return Ok(result);
        }

        [HttpPatch("updatename/{id}")]
        public async Task<IActionResult> UpdateName(int id, ReadCategoryUpdateRequest request)
        {
            var result = await _readCategoryService.Update(id, request);
            return Ok(result);
        }



    }
}
