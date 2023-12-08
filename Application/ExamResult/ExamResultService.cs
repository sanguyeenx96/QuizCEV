using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.LogExam.Response;
using ViewModels.LogExamTrinhtuthaotac.Response;

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
                ThoiGianLamBai = request.ThoiGianLamBai,
                ThoiGianChoPhepLamBai = request.ThoiGianChoPhepLamBai,
                Score = request.Score,
                CategoryId = request.CategoryId,
                CategoryName = request.CategoryName,
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
                CategoryName = x.CategoryName,
                Date = x.Date,
                Score = x.Score
            }).ToListAsync();
            return new ApiSuccessResult<List<ExamResultVm>>(ExamResults);
        }

        public async Task<ApiResult<List<ExamResultVm>>> Search(ExamResultSearchRequest request)
        {
            if (request.UserId != null || request.examResultId != null || request.CategoryId != null || request.Date != null || request.boPhanId != null
                || request.userName != null || request.name != null)
            {
                // Sử dụng IQueryable để tận dụng deferred execution
                IQueryable<Data.Entities.ExamResult> resultQuery = _context.ExamResults
                                                .Include(x => x.LogExams)
                                                    .ThenInclude(x => x.LogExamTrinhtuthaotacs)
                                                .Include(x => x.AppUser)
                                                    .ThenInclude(x => x.Cell)
                                                                .ThenInclude(x => x.Model)
                                                                .ThenInclude(x => x.Dept);
                if (request.UserId != null)
                    resultQuery = resultQuery.Where(x => x.UserId == request.UserId);
                if (request.examResultId != null)
                    resultQuery = resultQuery.Where(x => x.Id == request.examResultId);
                if (request.CategoryId != null)
                    resultQuery = resultQuery.Where(x => x.CategoryId == request.CategoryId);
                if (request.Date != null)
                {
                    DateTime requestDate = request.Date.Value.Date; // Lấy ngày tháng từ request.Date
                    resultQuery = resultQuery.Where(x => x.Date.Date == requestDate);
                }
                if (request.boPhanId != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Cell.Model.Dept.Id == request.boPhanId);
                if (request.userName != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.UserName.Contains(request.userName));
                if (request.name != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Name.Contains(request.name));

                // Sử dụng ToListAsync() để thực hiện truy vấn
                var resultList = await resultQuery.ToListAsync();
                // Chuyển đổi danh sách kết quả thành danh sách ExamResultVm
                var listExamResults = resultList.Select(result => new ExamResultVm
                {
                    Id = result.Id,
                    ThoiGianLamBai = result.ThoiGianLamBai,
                    ThoiGianChoPhepLamBai = result.ThoiGianChoPhepLamBai,
                    Hoten = result.AppUser.Name,
                    Bophan = result.AppUser.Cell.Model.Dept.Name,
                    CategoryName = result.CategoryName,
                    UserId = result.UserId,
                    CategoryId = result.CategoryId,
                    Date = result.Date,
                    Score = result.Score,
                    LogExams = result.LogExams != null
                    ? result.LogExams.Select(logExam => new LogExamVm
                    {
                        Id = logExam.Id,
                        ExamResultId = logExam.ExamResultId,
                        LoaiCauHoi = logExam.LoaiCauHoi,
                        Cauhoi = logExam.Cauhoi,
                        QA = logExam.QA,
                        QB = logExam.QB,
                        QC = logExam.QC,
                        QD = logExam.QD,
                        Cautraloi = logExam.Cautraloi,
                        Dapandung = logExam.Dapandung,
                        Score = logExam.Score,
                        FinalScore = logExam.FinalScore,
                        LogExamTrinhtuthaotacs = logExam.LogExamTrinhtuthaotacs != null
                                ? logExam.LogExamTrinhtuthaotacs.Select(LogExamTrinhtuthaotac => new LogExamTrinhtuthaotacVm
                                {
                                    Id = LogExamTrinhtuthaotac.Id,
                                    Text = LogExamTrinhtuthaotac.Text,
                                    ThuTu = LogExamTrinhtuthaotac.ThuTu,
                                    Answer = LogExamTrinhtuthaotac.Answer,
                                    LogExamId = LogExamTrinhtuthaotac.LogExamId
                                }).ToList()
                                : new List<LogExamTrinhtuthaotacVm>()
                    }).ToList()
                    : new List<LogExamVm>()
                }).ToList();
                return new ApiSuccessResult<List<ExamResultVm>>(listExamResults);
            }
            else
            {
                var emptyList = new List<ExamResultVm>();
                return new ApiSuccessResult<List<ExamResultVm>>(emptyList);
            }
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
