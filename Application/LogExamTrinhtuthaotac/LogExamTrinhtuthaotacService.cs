using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace Application.LogExamTrinhtuthaotac
{
    public class LogExamTrinhtuthaotacService : ILogExamTrinhtuthaotacService
    {
        private readonly TracNghiemCEVDbContext _context;
        public LogExamTrinhtuthaotacService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateList(List<LogExamTrinhtuthaotacCreateRequest> request)
        {
            foreach (var item in request)
            {
                var logExam = new Data.Entities.LogExamTrinhtuthaotac()
                {
                    Text = item.Text,
                    ThuTu = item.ThuTu,
                    Answer = item.Answer,
                    LogExamId = item.LogExamId,
                    Score = item.Score,
                    FinalScore = item.FinalScore
                };
                _context.logExamTrinhtuthaotacs.Add(logExam);
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
