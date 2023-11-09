using Application.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Category.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            var result = await _categoryService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);
            return Ok(result);
        }

        [HttpPatch("updatename/{id}")]
        public async Task<IActionResult> UpdateName(int id, CategoryUpdateNameRequest request)
        {
            var result = await _categoryService.UpdateName(id,request);
            return Ok(result);
        }

        [HttpPatch("updatestatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, CategoryUpdateStatusRequest request)
        {
            var result = await _categoryService.UpdateStatus(id, request);
            return Ok(result);
        }
    }
}
