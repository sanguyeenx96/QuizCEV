using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using ViewModels.Users.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _usersService.Authenticate(request);
            return Ok(result);
        }

        //[HttpGet("paging")]
        //public async Task<IActionResult> GetAllPaging(int id, [FromQuery] GetUserPagingRequest request)
        //{
        //    var result = await _usersService.GetUsetPaging(id, request);
        //    return Ok(result);
        //}

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _usersService.Register(request);
            return Ok(result);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            var result = await _usersService.RoleAssign(id, request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _usersService.GetById(id);
            return Ok(user);
        }

        [HttpGet("GetAllByDeptId/{id}")]
        public async Task<IActionResult> GetAllByDeptId(int id)
        {
            var result = await _usersService.GetAllByDeptId(id);
            return Ok(result);
        }

        [HttpPut("phanquyen/{id}")]
        public async Task<IActionResult> Phanquyen(Guid id, [FromBody] UserPhanquyenRequest request)
        {
            var result = await _usersService.Phanquyen(id,request);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            var result = await _usersService.Update(id, request);
            return Ok(result);
        }

        [HttpPut("resetpassword/{id}")]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] UserResetPasswordRequest request)
        {
            var result = await _usersService.ResetPassword(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _usersService.Delete(id);
            return Ok(result);
        }

        [HttpGet("count/{id}")]
        public async Task<IActionResult> Count(int id)
        {
            var result = await _usersService.Count(id);
            return Ok(result);
        }
        [HttpPost("checkpw/{id}")]
        public async Task<IActionResult> CheckPassword(Guid id, [FromBody] UserCheckPasswordRequest request)
        {
            var result = await _usersService.CheckPassWord(id,request);
            return Ok(result);
        }


        [HttpPost("importexcel/{role}/{cellId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file, string role, int cellId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Tệp Excel không được gửi.");
                }
                using (var fileStream = file.OpenReadStream())
                {
                    var danhsachuser = await _usersService.ReadExcelFile(fileStream);
                    var result = await _usersService.ImportExcelFile(danhsachuser, role, cellId);
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
            catch (Exception ex)
            {
                return BadRequest("Lỗi xử lý tệp Excel: " + ex.Message);
            }
        }
    }
}
