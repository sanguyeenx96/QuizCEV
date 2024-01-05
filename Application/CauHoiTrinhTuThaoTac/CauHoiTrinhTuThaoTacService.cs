using Application.CauHoiTuLuan;
using Data.EF;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Cell.Response;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Users.Request;

namespace Application.CauHoiTrinhTuThaoTac
{
    public class CauHoiTrinhTuThaoTacService : ICauHoiTrinhTuThaoTacService
    {
        private readonly TracNghiemCEVDbContext _context;
        public CauHoiTrinhTuThaoTacService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> ChangeOrder(List<CauHoiTrinhTuThaoTacChangeOrderRequest> request)
        {
            try
            {
                var questions = _context.cauHoiTrinhTuThaoTacs.AsQueryable();
                foreach (var item in request)
                {
                    var cauhoi = await questions.Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                    if (cauhoi != null)
                    {
                        cauhoi.ThuTu = item.ThuTu;
                        _context.cauHoiTrinhTuThaoTacs.Update(cauhoi);
                    }
                }
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("An error occurred while updating the order.");
            }
        }

        public async Task<ApiResult<int>> Count(int id)
        {
            int total = await _context.cauHoiTrinhTuThaoTacs.Where(x => x.CauHoiTuLuanId == id).CountAsync();
            return new ApiSuccessResult<int> { ResultObj = total };
        }

        public async Task<ApiResult<bool>> Create(CauHoiTrinhTuThaoTacCreateRequest request)
        {
            var checkDuplicateQuestion = await _context.cauHoiTrinhTuThaoTacs.Where(x => (x.Text == request.Text && x.CauHoiTuLuanId == request.CauHoiTuLuanId)).FirstOrDefaultAsync();
            if (checkDuplicateQuestion != null)
                return new ApiErrorResult<bool> { Message = "Câu hỏi đã bị trùng" };
            int num = _context.cauHoiTrinhTuThaoTacs.Where(x => x.CauHoiTuLuanId == request.CauHoiTuLuanId).Any() ? _context.cauHoiTrinhTuThaoTacs.Where(x => x.CauHoiTuLuanId == request.CauHoiTuLuanId).Max(x => x.ThuTu) + 1 : 1;

            var newQuestion = new Data.Entities.CauHoiTrinhTuThaoTac()
            {
                CauHoiTuLuanId = request.CauHoiTuLuanId,
                Text = request.Text,
                ThuTu = num,
                Score = request.Score
            };
            _context.cauHoiTrinhTuThaoTacs.Add(newQuestion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(Guid id, CauHoiTrinhTuThaoTacDeleteRequest request)
        {
            var question = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };

            var diemchuys = await _context.DiemChuYs.Where(x => x.CauhoitrinhtuthaotacId == id).ToListAsync();
            foreach(var dcy in diemchuys)
            {
                _context.Remove(dcy);
            }
            var loitaicongdoans = await _context.LoiTaiCongDoans.Where(x => x.CauhoitrinhtuthaotacId == id).ToListAsync();
            foreach (var ltcd in loitaicongdoans)
            {
                var doisachs = await _context.LoiTaiCongDoanDoiSaches.Where(x => x.LoiTaiCongDoanId == ltcd.Id).ToListAsync();
                foreach(var ds in doisachs)
                {
                    _context.Remove(ds);
                }
                _context.Remove(ltcd);
            }
            _context.cauHoiTrinhTuThaoTacs.Remove(question);
            await _context.SaveChangesAsync();
            var remainingQuestions = await _context.cauHoiTrinhTuThaoTacs
               .Where(x => x.CauHoiTuLuanId == request.cauhoituluanId)
               .OrderBy(q => q.ThuTu)
               .ToListAsync();
            int count = 0;
            for (int i = 0; i < remainingQuestions.Count; i++)
            {
                count++;
                remainingQuestions[i].ThuTu = count;
                _context.cauHoiTrinhTuThaoTacs.Update(remainingQuestions[i]);
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = "Đã xoá thành công!" };
        }

        public async Task<ApiResult<List<CauHoiTrinhTuThaoTacVm>>> GetAllByCauHoiTuLuan(int id)
        {
            // Selecting data with includes
            var query = _context.cauHoiTrinhTuThaoTacs
                .Include(x => x.TTTTDiemChuYs)
                .Include(x => x.TTTTLoiTaiCongDoans)
                .ThenInclude(x => x.LoiTaiCongDoanDoiSachs)
                .Where(x => x.CauHoiTuLuanId == id);

            // Projecting the data into the desired ViewModel
            var listquestions = await query.Select(y => new CauHoiTrinhTuThaoTacVm
            {
                Id = y.Id,
                Text = y.Text,
                ThuTu = y.ThuTu,
                Score = y.Score != null ? y.Score : 0,
                CauHoiTuLuanId = y.CauHoiTuLuanId,
                diemChuYs = y.TTTTDiemChuYs != null
                                ? y.TTTTDiemChuYs.Select(z => new DiemChuYVm
                                {
                                    Text = z.Text,
                                    CauhoitrinhtuthaotacId = z.CauhoitrinhtuthaotacId,
                                    Id = z.Id
                                }).ToList()
                                : new List<DiemChuYVm>(),

                loiTaiCongDoans = y.TTTTLoiTaiCongDoans != null
                                ? y.TTTTLoiTaiCongDoans.Select(t => new LoiTaiCongDoanVm
                                {
                                    Text = t.Text,
                                    CauhoitrinhtuthaotacId = t.CauhoitrinhtuthaotacId,
                                    Id = t.Id,
                                    doiSaches = t.LoiTaiCongDoanDoiSachs != null
                                                    ? t.LoiTaiCongDoanDoiSachs.Select(d => new DoiSachVm
                                                    {
                                                        Text = d.Text,
                                                        LoiTaiCongDoanId = d.LoiTaiCongDoanId,
                                                        Id = d.Id
                                                    }).ToList()
                                                    : new List<DoiSachVm>()
                                }).ToList()
                                : new List<LoiTaiCongDoanVm>()
            }).ToListAsync();

            return new ApiSuccessResult<List<CauHoiTrinhTuThaoTacVm>>(listquestions);
        }

        public async Task<ApiResult<CauHoiTrinhTuThaoTacVm>> GetById(Guid id)
        {
            var result = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (result == null)
                return new ApiErrorResult<CauHoiTrinhTuThaoTacVm> { Message = "Không tìm thấy câu hỏi" };
            var question = new CauHoiTrinhTuThaoTacVm
            {
                Id = result.Id,
                Text = result.Text,
                ThuTu = result.ThuTu,
                Score = result.Score,
                CauHoiTuLuanId = result.CauHoiTuLuanId
            };
            return new ApiSuccessResult<CauHoiTrinhTuThaoTacVm>(question);
        }

        public async Task<ApiResult<bool>> UpdateText(Guid id, CauHoiTrinhTuThaoTacUpdateTextRequest request)
        {
            var cauhoi = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (cauhoi == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            cauhoi.Text = request.Text;
            _context.cauHoiTrinhTuThaoTacs.Update(cauhoi);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        private string GetCellValue(ExcelWorksheet worksheet, int row, int column)
        {
            var cellValue = worksheet.Cells[row, column].Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                throw new Exception($"Missing value data at row {row}, column {column}");
            }
            return cellValue;
        }

        public async Task<List<CauHoiTrinhThuThaoTacImportExcelRequest>> ReadExcelFile(Stream fileStream)
        {
            try
            {
                using (var package = new ExcelPackage(fileStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var danhsachthututhaotac = new List<CauHoiTrinhThuThaoTacImportExcelRequest>();
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string? text = null;
                        if (worksheet.Cells[row, 1].Value != null)
                        {
                            text = worksheet.Cells[row, 1].Value.ToString();
                        }
                        float? score = 0;
                        if (worksheet.Cells[row, 2].Value != null)
                        {
                            score = Convert.ToInt32((worksheet.Cells[row, 2].Value.ToString()));
                        }
                        var newThututhaotac = new CauHoiTrinhThuThaoTacImportExcelRequest
                        {
                            Text = text,
                            Score = score
                        };
                        danhsachthututhaotac.Add(newThututhaotac);
                    }
                    return danhsachthututhaotac;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing Excel file: " + ex.Message);
            }
        }

        public async Task<ApiResult<ImportExcelResult>> ImportExcelFile(List<CauHoiTrinhThuThaoTacImportExcelRequest> request, int cauhoituluanId)
        {
            try
            {
                int lineupdate = 0;
                int linetrung = 0;
                foreach (var item in request)
                {
                    var checkDuplicate = await _context.cauHoiTrinhTuThaoTacs.Where(x => (x.CauHoiTuLuanId == cauhoituluanId && x.Text == item.Text)).FirstOrDefaultAsync();
                    if (checkDuplicate != null)
                    {
                        linetrung++;
                    }
                    else
                    {
                        var requestTTTT = new CauHoiTrinhTuThaoTacCreateRequest()
                        {
                            CauHoiTuLuanId = cauhoituluanId,
                            Text = item.Text,
                            Score = item.Score
                        };
                        var result = await Create(requestTTTT);
                        if (result.IsSuccessed)
                        {
                            lineupdate++;
                        }
                    }
                }
                var ketqua = new ImportExcelResult()
                {
                    sodongtrung = linetrung,
                    sodongupdate = lineupdate
                };
                return new ApiSuccessResult<ImportExcelResult>(ketqua);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing insert to SQL: " + ex.Message);
            }
        }

        public async Task<ApiResult<bool>> UpdateScore(Guid id, CauHoiTrinhTuThaoTacUpdateScoreRequest request)
        {
            var question = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            question.Score = request.Score;
            _context.cauHoiTrinhTuThaoTacs.Update(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
