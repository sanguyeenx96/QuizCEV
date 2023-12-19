using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace Application.LogExamDiemChuY
{
    public class LogExamDiemChuYService : ILogExamDiemChuYService
    {
        private readonly TracNghiemCEVDbContext _context;
        public LogExamDiemChuYService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateList(List<LogExamDiemChuYCreateRequest> request)
        {
            foreach (var item in request)
            {
                var logExamDCY = new Data.Entities.LogExamDiemChuY()
                {
                    Text = item.Text,
                    Answer = item.Answer,
                    CorrectAnswer = item.CorrectAnswer,
                    LogExamId = item.LogExamId
                };
                _context.logExamDiemChuYs.Add(logExamDCY);
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
