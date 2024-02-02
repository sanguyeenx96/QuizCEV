using Application.Dept;
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
using ViewModels.Model.Response;
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

        public async Task<ApiResult<int>> CountPerson(ReadResultCountPersonRequest request)
        {
            int soluongtaikhoan = 0;
            var result = _context.Users;
            if (request.deptId != null)
            {
                soluongtaikhoan = await result.Where(x => x.Cell.Model.Dept.Id == request.deptId).CountAsync();
            };
            if (request.modelId != null)
            {
                soluongtaikhoan = await result.Where(x => x.Cell.Model.Id == request.modelId).CountAsync();
            };
            if (request.cellId != null)
            {
                soluongtaikhoan = await result.Where(x => x.Cell.Id == request.cellId).CountAsync();
            };
            return new ApiSuccessResult<int> { ResultObj = soluongtaikhoan };
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
            if (request.UserId != null || request.readPostId != null || request.Date != null || request.boPhanId != null || request.userName != null || request.name != null)
            {
                // Sử dụng IQueryable để tận dụng deferred execution
                IQueryable<Data.Entities.ReadResult> resultQuery = _context.readResults
                                                .Include(x => x.ReadPost)
                                                .ThenInclude(x => x.readCategory)
                                                .Include(x => x.AppUser)
                                                    .ThenInclude(x => x.Cell)
                                                    .ThenInclude(x => x.Model)
                                                    .ThenInclude(x => x.Dept);

                IQueryable<Data.Entities.AppUser> resultUser = _context.Users
                                                .Include(x => x.Cell)
                                                .ThenInclude(x => x.Model)
                                                .ThenInclude(x => x.Dept);

                if (request.UserId != null)
                {
                    resultQuery = resultQuery.Where(x => x.UserId == request.UserId);
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
                        CategoryTitle = result.ReadPost.readCategory.Title,
                        PostTitle = result.ReadPost.Title
                    }).ToList();
                    return new ApiSuccessResult<List<ReadResultVm>>(listReadResults);
                }
                else
                {
                    if (request.boPhanId != null)
                    {
                        resultQuery = resultQuery.Where(x => x.AppUser.Cell.Model.Dept.Id == request.boPhanId);
                        resultUser = resultUser.Where(x => x.Cell.Model.Dept.Id == request.boPhanId);
                    }

                    if (request.modelId != null)
                    {
                        resultQuery = resultQuery.Where(x => x.AppUser.Cell.Model.Id == request.modelId);
                        resultUser = resultUser.Where(x => x.Cell.Model.Id == request.modelId);
                    }

                    if (request.cellId != null)
                    {
                        resultQuery = resultQuery.Where(x => x.AppUser.Cell.Id == request.cellId);
                        resultUser = resultUser.Where(x => x.Cell.Id == request.cellId);
                    }

                    if (request.readPostId != null)
                    {
                        resultQuery = resultQuery.Where(x => x.ReadPostId == request.readPostId);
                    }

                    if (request.Date != null)
                    {
                        DateTime requestDate = request.Date.Value.Date; // Lấy ngày tháng từ request.Date
                        resultQuery = resultQuery.Where(x => x.Date.Date == requestDate);
                    }

                    if (request.userName != null)
                    {
                        resultQuery = resultQuery.Where(x => x.AppUser.UserName.Contains(request.userName));
                        resultUser = resultUser.Where(x => x.UserName.Contains(request.userName));
                    }

                    if (request.name != null)
                    {
                        resultQuery = resultQuery.Where(x => x.AppUser.Name.Contains(request.name));
                        resultUser = resultUser.Where(x => x.Name.Contains(request.name));
                    }

                    // Sử dụng ToListAsync() để thực hiện truy vấn
                    var resultList = await resultQuery.ToListAsync();
                    var resultUserList = await resultUser.ToListAsync();

                    var combinedList = resultUserList.Select(user =>
                    {
                        var userResult = resultList.FirstOrDefault(result => result.UserId == user.Id);
                        if (userResult != null)
                        {
                            // Nếu có dữ liệu từ resultQuery cho người này, thêm vào thông tin
                            return new ReadResultVm
                            {
                                Date = userResult.Date,
                                ReadPostId = userResult.ReadPostId,
                                UserId = userResult.UserId,
                                Hoten = user.Name,
                                Model = user.Cell.Model.Name,
                                Cell = user.Cell.Name,
                                Bophan = user.Cell.Model.Dept.Name,
                                CategoryTitle = userResult.ReadPost?.readCategory.Title,
                                PostTitle = userResult.ReadPost?.Title,
                                Username = user.UserName
                            };
                        }
                        else
                        {
                            // Nếu không có dữ liệu từ resultQuery cho người này, tạo mới với các trường là null
                            var defaultResult = resultList.FirstOrDefault();
                            return new ReadResultVm
                            {
                                Date = null,
                                ReadPostId = defaultResult?.ReadPostId,
                                UserId = user.Id,
                                Hoten = user.Name,
                                Model = user.Cell.Model.Name,
                                Cell = user.Cell.Name,
                                Bophan = user.Cell.Model.Dept.Name,
                                CategoryTitle = defaultResult?.ReadPost?.readCategory?.Title,
                                PostTitle = defaultResult?.ReadPost?.Title,
                                Username = user.UserName
                            };
                        }
                    }).ToList();

                    if(request.status != null)
                    {
                        if (request.status == 1) //Đã đọc
                        {
                            combinedList = combinedList.Where(x => x.Date != null).ToList();
                        }
                        if(request.status == 2)
                        {
                            combinedList = combinedList.Where(x => x.Date == null).ToList();
                        }
                    }                     
                    return new ApiSuccessResult<List<ReadResultVm>>(combinedList);
                }                
            }
            else
            {
                var emptyList = new List<ReadResultVm>();
                return new ApiSuccessResult<List<ReadResultVm>>(emptyList);
            }
        }
    }
}
