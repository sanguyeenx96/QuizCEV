using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExam.Request;
using ViewModels.LogExam.Response;

namespace Application.LogExam
{
    public class LogExamService : ILogExamService
    {
        private readonly TracNghiemCEVDbContext _context;
        public LogExamService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Create(LogExamCreateRequest request)
        {
            var logExam = new Data.Entities.LogExam()
            {
                ExamResultId = request.ExamResultId,
                Cauhoi = request.Cauhoi,
                QA = request.QA,
                QB = request.QB,
                QC = request.QC,
                QD = request.QD,
                Cautraloi = request.Cautraloi,
                Dapandung = request.Dapandung
            };
            _context.LogExams.Add(logExam);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = logExam.Id };
        }

        public async Task<ApiResult<List<LogExamVm>>> GetByExamId(int id)
        {
            var result = await _context.LogExams.Where(x => x.ExamResultId == id)
                            .Select(x => new LogExamVm()
                            {
                                Id = x.Id,
                                Cauhoi = x.Cauhoi,
                                QA = x.QA,
                                QB = x.QB,
                                QC = x.QC,
                                QD = x.QD,
                                Cautraloi = x.Cautraloi,
                                Dapandung = x.Dapandung,
                                ExamResultId = x.ExamResultId
                            }).ToListAsync();
            if (result == null)
                return new ApiErrorResult<List<LogExamVm>>() { Message = "Không tìm thấy dữ liệu" };
            return new ApiSuccessResult<List<LogExamVm>>(result);
        }

        public async Task<ApiResult<LogExamVm>> GetById(int id)
        {
            var result = await _context.LogExams.FindAsync(id);
            if (result == null)
                return new ApiErrorResult<LogExamVm>() { Message = "Không tìm thấy dữ liệu" };
            var logExam = new LogExamVm()
            {
                Id = result.Id,
                Cauhoi = result.Cauhoi,
                QA = result.QA,
                QB = result.QB,
                QC = result.QC,
                QD = result.QD,
                Cautraloi = result.Cautraloi,
                Dapandung = result.Dapandung,
                ExamResultId = result.ExamResultId
            };
            return new ApiSuccessResult<LogExamVm>(logExam);

        }
    }
}
