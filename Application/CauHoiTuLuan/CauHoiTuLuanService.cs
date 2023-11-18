using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Question.Response;

namespace Application.CauHoiTuLuan
{
    public class CauHoiTuLuanService : ICauHoiTuLuanService
    {
        private readonly TracNghiemCEVDbContext _context;
        public CauHoiTuLuanService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Count(int categoryId)
        {
            int total = await _context.CauHoiTuLuans.Where(x => x.CategoryId == categoryId).CountAsync();
            return new ApiSuccessResult<int> { ResultObj = total };
        }

        public async Task<ApiResult<int>> Create(CauHoiTuLuanCreateRequest request)
        {
            var checkDuplicateQuestion = await _context.CauHoiTuLuans.Where(x => (x.Text == request.Text && x.CategoryId == request.CategoryId)).FirstOrDefaultAsync();
            if (checkDuplicateQuestion != null)
                return new ApiErrorResult<int> { Message = "Câu hỏi đã bị trùng" };
            var newQuestion = new Data.Entities.CauHoiTuLuan()
            {
                CategoryId = request.CategoryId,
                Text = request.Text,           
            };
            _context.CauHoiTuLuans.Add(newQuestion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newQuestion.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var question = await _context.CauHoiTuLuans.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            _context.CauHoiTuLuans.Remove(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = "Đã xoá thành công!" };
        }

        public async Task<ApiResult<List<CauHoiTuLuanVm>>> GetAllByCategory(int id)
        {
            var listquestions = await _context.CauHoiTuLuans.Where(x => x.CategoryId == id).Select(x => new CauHoiTuLuanVm
            {
                Id = x.Id,
                Text = x.Text,
                Score = x.Score,
                CategoryId = x.CategoryId
            }).ToListAsync();
            return new ApiSuccessResult<List<CauHoiTuLuanVm>>(listquestions);
        }

        public async Task<ApiResult<CauHoiTuLuanVm>> GetById(int id)
        {
            var result = await _context.CauHoiTuLuans.FindAsync(id);
            if (result == null)
                return new ApiErrorResult<CauHoiTuLuanVm> { Message = "Không tìm thấy câu hỏi" };
            var question = new CauHoiTuLuanVm
            {
                Id = result.Id,
                Text = result.Text,
                Score = result.Score,
                CategoryId = result.CategoryId
            };
            return new ApiSuccessResult<CauHoiTuLuanVm>(question);
        }

        public async Task<ApiResult<bool>> UpdateScore(int id, CauHoiTuLuanUpdateScoreRequest request)
        {
            var question = await _context.CauHoiTuLuans.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            question.Score = request.Score;
            _context.CauHoiTuLuans.Update(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> ();
        }
    }
}
