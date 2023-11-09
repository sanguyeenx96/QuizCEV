﻿using Application.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Question.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _QuestionService;
        public QuestionController(IQuestionService questionService)
        {
            _QuestionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _QuestionService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _QuestionService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetAllByCategory/{id}")]
        public async Task<IActionResult> GetAllByCategory(int id)
        {
            var result = await _QuestionService.GetAllByCategory(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, QuestionUpdateRequest request)
        {
            var result = await _QuestionService.Update(id, request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionCreateRequest request)
        {
            var result = await _QuestionService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _QuestionService.Delete(id);
            return Ok(result);
        }
    }
}
