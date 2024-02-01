using Data.EF;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.ExportExcel;
using ViewModels.LogExam.Response;
using ViewModels.LogExamTrinhtuthaotac.Response;
using Microsoft.AspNetCore.Hosting;

namespace Application.ExamResult.ExportExcel
{
    public class ExportExcelService : IExportExcelService
    {
        private readonly TracNghiemCEVDbContext _context;

        public ExportExcelService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> ExportExcelFile(Stream fileStream, List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> request)
        {
           
            using (ExcelPackage excelPackage = new ExcelPackage(fileStream))
            {
                // Chọn worksheet cần thêm dữ liệu
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Sheet1"];

                // Xác định hàng bắt đầu và kết thúc để chèn dữ liệu vào giữa
                int startRow = 14; // Hàng bắt đầu chèn dữ liệu
                int endRow = worksheet.Dimension.End.Row; // Hàng cuối cùng có dữ liệu

                // Lấy dữ liệu từ nguồn dữ liệu (ví dụ: từ database hoặc từ danh sách)
                var data = request;
                if(data != null)
                {
                    // Dịch chuyển các hàng dữ liệu xuống dưới để tạo chỗ trống cho dữ liệu mới
                    int rowCount = data.Count; // Số hàng dữ liệu mới sẽ được chèn vào
                    worksheet.InsertRow(startRow + 1, rowCount);

                    // Thêm dữ liệu vào file Excel
                    int currentRow = startRow;
                    foreach (var item in data)
                    {
                      //  worksheet.Cells["A" + currentRow].Value = item.Property1;
                      //  worksheet.Cells["B" + currentRow].Value = item.Property2;
                        currentRow++;
                    }
                    // Lưu lại các thay đổi vào file Excel
                    excelPackage.Save();
                }
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<ExamResultVm>>> Searchdata(ExamResultSearchRequest request)
        {
            if (request.UserId != null || request.examResultId != null || request.CategoryId != null || request.Date != null 
                || request.boPhanId != null || request.userName != null || request.name != null)
            {
                // Sử dụng IQueryable để tận dụng deferred execution
                IQueryable<Data.Entities.ExamResult> resultQuery = _context.ExamResults                                                
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
                    Bophan = result.AppUser.Cell.Model.Dept.Name                    
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
