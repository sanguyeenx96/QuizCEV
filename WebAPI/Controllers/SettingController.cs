using Application.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _settingService.GetAll();
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _settingService.ChangeSetting(id);
            return Ok(result);
        }
    }
}
