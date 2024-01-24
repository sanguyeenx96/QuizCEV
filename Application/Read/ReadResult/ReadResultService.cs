using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ExamResult.Response;
using ViewModels.LogExam.Response;
using ViewModels.LogExamTrinhtuthaotac.Response;
using ViewModels.Read.ReadResult;

namespace Application.Read.ReadResult
{
    public class ReadResultService : IReadResultService
    {
        private readonly TracNghiemCEVDbContext _context;
        public ReadResultService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Create(ReadResultCreateRequest request)
        {
            var newReadResult = new Data.Entities.ReadResult
            {
                UserId = request.UserId,
                Date = DateTime.Now,
                ReadPostId = request.ReadPostId
            };
            _context.readResults.Add(newReadResult);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newReadResult.Id };
        }

        public async Task<ApiResult<List<ReadResultVm>>> Search(ReadResultSearchRequest request)
        {
            if (request.UserId != null || request.readResultId != null || request.readPostId != null || 
                request.Date != null || request.boPhanId != null
               || request.userName != null || request.name != null)
            {
                // Sử dụng IQueryable để tận dụng deferred execution
                IQueryable<Data.Entities.ReadResult> resultQuery = _context.readResults                                              
                                                .Include(x => x.AppUser)
                                                    .ThenInclude(x => x.Cell)
                                                    .ThenInclude(x => x.Model)
                                                    .ThenInclude(x => x.Dept);
                if (request.UserId != null)
                    resultQuery = resultQuery.Where(x => x.UserId == request.UserId);
                if (request.readResultId != null)
                    resultQuery = resultQuery.Where(x => x.Id == request.readResultId);              
                if (request.Date != null)
                {
                    DateTime requestDate = request.Date.Value.Date; // Lấy ngày tháng từ request.Date
                    resultQuery = resultQuery.Where(x => x.Date.Date == requestDate);
                }
                if (request.boPhanId != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Cell.Model.Dept.Id == request.boPhanId);
                if (request.modelId != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Cell.Model.Id == request.modelId);
                if (request.cellId != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Cell.Id == request.cellId);
                if (request.userName != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.UserName.Contains(request.userName));
                if (request.name != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Name.Contains(request.name));

                // Sử dụng ToListAsync() để thực hiện truy vấn
                var resultList = await resultQuery.ToListAsync();
                // Chuyển đổi danh sách kết quả thành danh sách ExamResultVm
                var listReadResults = resultList.Select(result => new ReadResultVm
                {
                    Id = result.Id,
                    Date = result.Date,
                    ReadPostId = result.ReadPostId,

                    UserId = result.UserId,
                    Hoten = result.AppUser.Name,
                    Model = result.AppUser.Cell.Model.Name,
                    Cell = result.AppUser.Cell.Name,
                    Bophan = result.AppUser.Cell.Model.Dept.Name,                   
                }).ToList();
                return new ApiSuccessResult<List<ReadResultVm>>(listReadResults);
            }
            else
            {
                var emptyList = new List<ReadResultVm>();
                return new ApiSuccessResult<List<ReadResultVm>>(emptyList);
            }
        }
    }
}
