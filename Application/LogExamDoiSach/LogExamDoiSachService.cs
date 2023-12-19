using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamDoiSach.Request;

namespace Application.LogExamDoiSach
{
    public class LogExamDoiSachService : ILogExamDoiSachService
    {
        private readonly TracNghiemCEVDbContext _context;
        public LogExamDoiSachService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateList(List<LogExamDoiSachCreateRequest> request)
        {
            foreach (var item in request)
            {
                var logExamDS = new Data.Entities.LogExamDoiSach()
                {
                    Text = item.Text,
                    Answer = item.Answer,
                    CorrectAnswer = item.CorrectAnswer,
                    LogExamId = item.LogExamId
                };
                _context.logExamDoiSaches.Add(logExamDS);
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
