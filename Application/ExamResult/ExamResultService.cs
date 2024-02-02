using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
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

        public async Task<ApiResult<bool>> CheckReTest(ExamResultCheckRetestRequest request)
        {
            var result = await _context.ExamResults.Where(x=>(x.UserId == request.UserId && x.CategoryName == request.CategoryName)).AnyAsync();
            if (!result)
            {
                return new ApiErrorResult<bool>();
            }
            return new ApiSuccessResult<bool>();
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

        public async Task<ApiResult<bool>> DanhGia(int id, ExamResultDanhGiaRequest request)
        {
            var examResult = await _context.ExamResults.FindAsync(id);
            if (examResult == null)
                return new ApiErrorResult<bool>("Không tìm thấy bài thi");
            examResult.Result = request.Result;
            _context.Update(examResult);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
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
                                                    .ThenInclude(x=>x.LogExamTrinhtuthaotacs)
                                                .Include(x => x.LogExams)
                                                    .ThenInclude(x => x.logExamDiemChuYs)
                                                .Include(x => x.LogExams)
                                                    .ThenInclude(x => x.logExamLoiTaiCongDoans)
                                                .Include(x => x.LogExams)
                                                    .ThenInclude(x => x.logExamDoiSaches)
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
                if (request.modelId != null )
                    resultQuery = resultQuery.Where(x => x.AppUser.Cell.Model.Id == request.modelId);
                if (request.cellId != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Cell.Id == request.cellId);
                if (request.userName != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.UserName.Contains(request.userName));
                if (request.name != null)
                    resultQuery = resultQuery.Where(x => x.AppUser.Name.Contains(request.name));
                if (request.mode != null)
                {
                    if (request.mode == 1) // Lọc theo bài thi mới nhất
                    {
                        resultQuery = resultQuery.GroupBy(x => x.UserId)
                                .Select(group => group.OrderByDescending(x => x.Date)
                                                    .FirstOrDefault());
                    }
                    if (request.mode == 2) // Lọc theo bài thi điểm cao nhất
                    {
                        resultQuery = resultQuery.GroupBy(x => x.UserId)
                                .Select(group => group.OrderByDescending(x => x.Score)
                                                    .FirstOrDefault());
                    }
                }
                // Sử dụng ToListAsync() để thực hiện truy vấn
                var resultList = await resultQuery.ToListAsync();
                // Chuyển đổi danh sách kết quả thành danh sách ExamResultVm
                var listExamResults = resultList.Select(result => new ExamResultVm
                {
                    Id = result.Id,
                    Result = result.Result,
                    ThoiGianLamBai = result.ThoiGianLamBai,
                    ThoiGianChoPhepLamBai = result.ThoiGianChoPhepLamBai,
                    Hoten = result.AppUser.Name,
                    Username = result.AppUser.UserName,
                    CategoryName = result.CategoryName,
                    UserId = result.UserId,
                    CategoryId = result.CategoryId,
                    Date = result.Date,
                    Score = result.Score,
                    Model = result.AppUser.Cell.Model.Name,
                    Cell = result.AppUser.Cell.Name,
                    Bophan = result.AppUser.Cell.Model.Dept.Name,
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
                                    Score= LogExamTrinhtuthaotac.Score,
                                    FinalScore = LogExamTrinhtuthaotac.FinalScore,
                                    LogExamId = LogExamTrinhtuthaotac.LogExamId
                                }).ToList()
                                : new List<LogExamTrinhtuthaotacVm>(),

                        logExamDiemChuYs = logExam.logExamDiemChuYs != null
                                ? logExam.logExamDiemChuYs.Select(LogExamDiemChuY => new ViewModels.LogExamDiemChuY.Response.LogExamDiemChuYVm
                                {
                                    Id = LogExamDiemChuY.Id,
                                    Text = LogExamDiemChuY.Text,
                                    Answer = LogExamDiemChuY.Answer,
                                    CorrectAnswer = LogExamDiemChuY.CorrectAnswer,
                                    LogExamId = LogExamDiemChuY.LogExamId
                                }).ToList()
                                : new List<ViewModels.LogExamDiemChuY.Response.LogExamDiemChuYVm>(),

                        logExamLoiTaiCongDoans = logExam.logExamLoiTaiCongDoans != null
                                ? logExam.logExamLoiTaiCongDoans.Select(LogExamLoiTaiCongDoan => new ViewModels.LogExamLoiTaiCongDoan.Response.LogExamLoiTaiCongDoanVm
                                {
                                    Id = LogExamLoiTaiCongDoan.Id,
                                    Text = LogExamLoiTaiCongDoan.Text,
                                    Answer = LogExamLoiTaiCongDoan.Answer,
                                    CorrectAnswer = LogExamLoiTaiCongDoan.CorrectAnswer,
                                    LogExamId = LogExamLoiTaiCongDoan.LogExamId
                                }).ToList()
                                : new List<ViewModels.LogExamLoiTaiCongDoan.Response.LogExamLoiTaiCongDoanVm>(),

                        logExamDoiSaches = logExam.logExamDoiSaches != null
                                ? logExam.logExamDoiSaches.Select(LogExamDoiSach => new ViewModels.LogExamDoiSach.Response.LogExamDoiSachVm
                                {
                                    Id = LogExamDoiSach.Id,
                                    Text = LogExamDoiSach.Text,
                                    Answer = LogExamDoiSach.Answer,
                                    CorrectAnswer = LogExamDoiSach.CorrectAnswer,
                                    LogExamId = LogExamDoiSach.LogExamId
                                }).ToList()
                                : new List<ViewModels.LogExamDoiSach.Response.LogExamDoiSachVm>()
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
    }
}
