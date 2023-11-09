using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Question.Response;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Question
{
    public class QuestionService : IQuestionService
    {
        private readonly TracNghiemCEVDbContext _context;
        public QuestionService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Create(QuestionCreateRequest request)
        {
            var checkDuplicateQuestion = await _context.Questions.Where(x => (x.Text == request.Text && x.CategoryId == request.CategoryId)).FirstOrDefaultAsync();
            if (checkDuplicateQuestion != null)
                return new ApiErrorResult<int> { Message = "Câu hỏi đã bị trùng" };
            var newQuestion = new Data.Entities.Question()
            {
                CategoryId = request.CategoryId,
                Text = request.Text,
                QA = request.QA,
                QB = request.QB,
                QC = request.QC,
                QD = request.QD,
                QCorrectAns = request.QCorrectAns
            };
            _context.Questions.Add(newQuestion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newQuestion.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = "Đã xoá thành công!" };
        }

        public async Task<ApiResult<List<QuestionVm>>> GetAll()
        {
            var result = await _context.Questions.Select(x => new QuestionVm
            {
                Id = x.Id,
                Text = x.Text,
                QA = x.QA,
                QB = x.QB,
                QC = x.QC,
                QD = x.QD,
                QCorrectAns = x.QCorrectAns,
                CategoryId = x.CategoryId
            }).ToListAsync();
            return new ApiSuccessResult<List<QuestionVm>>(result);
        }

        public async Task<ApiResult<List<QuestionVm>>> GetAllByCategory(int categoryId)
        {
            var listquestions = await _context.Questions.Where(x => x.CategoryId == categoryId).Select(x => new QuestionVm
            {
                Id = x.Id,
                Text = x.Text,
                QA = x.QA,
                QB = x.QB,
                QC = x.QC,
                QD = x.QD,
                QCorrectAns = x.QCorrectAns,
                CategoryId = x.CategoryId
            }).ToListAsync();
            return new ApiSuccessResult<List<QuestionVm>>(listquestions);
        }

        public async Task<ApiResult<QuestionVm>> GetById(int id)
        {
            var result = await _context.Questions.FindAsync(id);
            if (result == null)
                return new ApiErrorResult<QuestionVm> { Message = "Không tìm thấy câu hỏi" };
            var question = new QuestionVm
            {
                Id = result.Id,
                Text = result.Text,
                QA = result.QA,
                QB = result.QB,
                QC = result.QC,
                QD = result.QD,
                QCorrectAns = result.QCorrectAns,
                CategoryId = result.CategoryId
            };
            return new ApiSuccessResult<QuestionVm>(question);
        }

        public async Task<ApiResult<int>> Update(int id, QuestionUpdateRequest request)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<int> { Message = "Không tìm thấy câu hỏi" };
            question.Text = request.Text;
            question.QA = request.QA;
            question.QB = request.QB;
            question.QC = request.QC;
            question.QD = request.QD;
            question.QCorrectAns = request.QCorrectAns;
            question.CategoryId = request.CategoryId;
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = question.Id };

        }
    }
}
