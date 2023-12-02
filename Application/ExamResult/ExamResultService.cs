using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;

namespace Application.ExamResult
{
    public class ExamResultService : IExamResultService
    {
        private readonly TracNghiemCEVDbContext _context;
        public ExamResultService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Create(ExamResultCreateRequest request)
        {
            var newExamResult = new Data.Entities.ExamResult
            {
                Score = request.Score,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
                Date = DateTime.Now
            };
            _context.ExamResults.Add(newExamResult);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newExamResult.Id };
        }

        public async Task<ApiResult<List<ExamResultVm>>> Getall()
        {
            var ExamResults = await _context.ExamResults.Select(x => new ExamResultVm
            {
                Id = x.Id,
                UserId = x.UserId,
                CategoryId = x.CategoryId,
                Date = x.Date,
                Score = x.Score
            }).ToListAsync();
            return new ApiSuccessResult<List<ExamResultVm>>(ExamResults);
        }


        //public async Task<ApiResult<int>> Create(ExamResultCreateRequest request)
        //{
        //    var newExamResult = new Data.Entities.ExamResult
        //    {
        //        Score = request.Score,
        //        CategoryId = request.CategoryId,
        //        UserId = request.UserId,
        //        Date = DateTime.Now
        //    };
        //    _context.ExamResults.Add(newExamResult);
        //    await _context.SaveChangesAsync();
        //    return new ApiSuccessResult<int> { Id = newExamResult.Id };
        //}

        //public async Task<ApiResult<bool>> Delete(int id)
        //{
        //    var ExamResult = await _context.ExamResults.FindAsync(id);
        //    if (ExamResult == null)
        //        return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
        //    _context.ExamResults.Remove(ExamResult);
        //    await _context.SaveChangesAsync();
        //    return new ApiSuccessResult<bool>();
        //}

        //public async Task<ApiResult<List<ExamResultVm>>> Search(ExamResultSearchRequest request)
        //{
        //    // Sử dụng IQueryable để tận dụng deferred execution
        //    IQueryable<Data.Entities.ExamResult> resultQuery = _context.ExamResults;
        //    if (request.Id != null)
        //        resultQuery = resultQuery.Where(x => x.Id == request.Id);

        //    if (request.Date != null)
        //        resultQuery = resultQuery.Where(x => x.Date == request.Date);

        //    if (request.Score != null)
        //        resultQuery = resultQuery.Where(x => x.Score == request.Score);

        //    if (request.UserId != null)
        //        resultQuery = resultQuery.Where(x => x.UserId == request.UserId);

        //    if (request.CategoryId != null)
        //        resultQuery = resultQuery.Where(x => x.CategoryId == request.CategoryId);

        //    // Sử dụng ToListAsync() để thực hiện truy vấn
        //    var resultList = await resultQuery.ToListAsync();

        //    // Chuyển đổi danh sách kết quả thành danh sách ExamResultVm
        //    var listExamResults = resultList.Select(result => new ExamResultVm
        //    {
        //        Id = result.Id,
        //        UserId = result.UserId,
        //        CategoryId = result.CategoryId,
        //        Date = result.Date,
        //        Score = result.Score
        //    }).ToList();

        //    return new ApiSuccessResult<List<ExamResultVm>>(listExamResults);
        //}


        //public async Task<ApiResult<List<ExamResultVm>>> Getall()
        //{
        //    var ExamResults = await _context.ExamResults.Select(x => new ExamResultVm
        //    {
        //        Id = x.Id,
        //        UserId = x.UserId,
        //        CategoryId = x.CategoryId,
        //        Date = x.Date,
        //        Score = x.Score
        //    }).ToListAsync();

        //    return new ApiSuccessResult<List<ExamResultVm>>(ExamResults);
        //}

        //public async Task<ApiResult<int>> Update(int id, ExamResultUpdateRequest request)
        //{
        //    var ExamResult = await _context.ExamResults.FindAsync(id);
        //    if (ExamResult == null)
        //        return new ApiErrorResult<int> { Message = "Không tìm thấy dữ liệu" };
        //    ExamResult.Score = request.Score;
        //    ExamResult.UserId = request.UserId;
        //    ExamResult.CategoryId = request.CategoryId;
        //    ExamResult.Date = DateTime.Now;
        //    _context.ExamResults.Update(ExamResult);
        //    await _context.SaveChangesAsync();
        //    return new ApiSuccessResult<int> { Id = ExamResult.Id };
        //}
    }
}
