using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamLoiTaiCongDoan.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace Application.LogExamLoiTaiCongDoan
{
    public class LogExamLoiTaiCongDoanService : ILogExamLoiTaiCongDoanService
    {
        private readonly TracNghiemCEVDbContext _context;
        public LogExamLoiTaiCongDoanService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateList(List<LogExamLoiTaiCongDoanCreateRequest> request)
        {
            foreach (var item in request)
            {
                var logExamLTCD = new Data.Entities.LogExamLoiTaiCongDoan()
                {
                    Text = item.Text,
                    Answer = item.Answer,
                    CorrectAnswer = item.CorrectAnswer,                 
                    LogExamId = item.LogExamId
                };
                _context.logExamLoiTaiCongDoans.Add(logExamLTCD);
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

    }
}
