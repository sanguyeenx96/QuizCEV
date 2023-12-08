using Application.Dept;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Cell.Request;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Model.Request;
using ViewModels.Model.Response;

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


        //Dept
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _deptService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeptCreateRequest request)
        {
            var result = await _deptService.Create(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] DeptUpdateRequest request)
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


        //Model
        [HttpGet("getallbydept/{id}")]
        public async Task<IActionResult> GetAllByDept(int id)
        {
            var result = await _deptService.GetAllByDept(id);
            return Ok(result);
        }

        [HttpPost("createmodel/{DeptId}")]
        public async Task<IActionResult> CreateModel(int DeptId,[FromBody] ModelCreateRequest request)
        {
            var result = await _deptService.CreateModel(DeptId,request);
            return Ok(result);
        }

        [HttpPut("editmodel/{id}")]
        public async Task<IActionResult> EditModel(int id, [FromBody] ModelUpdateRequest request)
        {
            var result = await _deptService.UpdateModel(id, request);
            return Ok(result);
        }

        [HttpDelete("deletemodel/{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var result = await _deptService.DeleteModel(id);
            return Ok(result);
        }


        //Cell
        [HttpGet("getallbymodel/{id}")]
        public async Task<IActionResult> GetAllByModel(int id)
        {
            var result = await _deptService.GetAllByMOdel(id);
            return Ok(result);
        }
        [HttpPost("createcell/{ModelId}")]
        public async Task<IActionResult> CreateCell(int ModelId, [FromBody] CellCreateRequest request)
        {
            var result = await _deptService.CreateCell(ModelId, request);
            return Ok(result);
        }

        [HttpPut("editcell/{id}")]
        public async Task<IActionResult> EditCell(int id, [FromBody] CellUpdateRequest request)
        {
            var result = await _deptService.UpdateCell(id, request);
            return Ok(result);
        }

        [HttpDelete("deletecell/{id}")]
        public async Task<IActionResult> DeleteCell(int id)
        {
            var result = await _deptService.DeleteCell(id);
            return Ok(result);
        }
    }
}
