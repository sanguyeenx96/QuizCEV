﻿using Application.CauHoiTuLuan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.Question.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CauHoiTuLuanController : ControllerBase
    {
        private readonly ICauHoiTuLuanService _cauHoiTuLuanService;
        public CauHoiTuLuanController(ICauHoiTuLuanService cauHoiTuLuanService)
        {
            _cauHoiTuLuanService = cauHoiTuLuanService;
        }

        [HttpGet("GetAllByCategory/{id}")]
        public async Task<IActionResult> GetAllByCategory(int id)
        {
            var result = await _cauHoiTuLuanService.GetAllByCategory(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _cauHoiTuLuanService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CauHoiTuLuanCreateRequest request)
        {
            var result = await _cauHoiTuLuanService.Create(request);
            return Ok(result);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cauHoiTuLuanService.Delete(id);
            return Ok(result);
        }

        [HttpPut("updatescore/{id}")]
        public async Task<IActionResult> UpdateScore(int id, CauHoiTuLuanUpdateScoreRequest request)
        {
            var result = await _cauHoiTuLuanService.UpdateScore(id, request);
            return Ok(result);
        }

        [HttpGet("Count/{categoryId}")]
        public async Task<IActionResult> Count(int categoryId)
        {
            var result = await _cauHoiTuLuanService.Count(categoryId);
            return Ok(result);
        }

        [HttpPut("updatetext/{id}")]
        public async Task<IActionResult> UpdateText(int id, CauHoiTuLuanUpdateTextRequest request)
        {
            var result = await _cauHoiTuLuanService.UpdateText(id, request);
            return Ok(result);
        }

    }
}
