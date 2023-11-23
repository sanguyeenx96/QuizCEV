using Application.Dept;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Dept.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeptController : Controller
    {
        private readonly IDeptService _deptService;
        public DeptController(IDeptService deptService)
        {
            _deptService = deptService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _deptService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeptCreateRequest request)
        {
            var result = await _deptService.Create(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, DeptUpdateRequest request)
        {
            var result = await _deptService.Update(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deptService.Delete(id);
            return Ok(result);
        }
    }
}
