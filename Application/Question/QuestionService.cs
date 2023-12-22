using Data.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Question.Response;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Question
{
    public class QuestionService : IQuestionService
    {
        private readonly TracNghiemCEVDbContext _context;
        private readonly IConfiguration _configuration;

        public QuestionService(TracNghiemCEVDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiResult<int>> Create(QuestionCreateRequest request)
        {
            var checkDuplicateQuestion = await _context.Questions.Where(x => (x.Text == request.Text && x.CategoryId == request.CategoryId)).FirstOrDefaultAsync();
            if (checkDuplicateQuestion != null)
                return new ApiErrorResult<int> { Message = "Câu hỏi đã bị trùng" };
            var newQuestion = new Data.Entities.Question()
            {
                CategoryId = request.CategoryId,
                Text = request.Text,
                QA = request.QA,
                QB = request.QB,
                QC = request.QC,
                QD = request.QD,
                QCorrectAns = request.QCorrectAns.ToUpper()
            };
            _context.Questions.Add(newQuestion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newQuestion.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = "Đã xoá thành công!" };
        }

        public async Task<ApiResult<List<QuestionVm>>> GetAll()
        {
            var result = await _context.Questions.Select(x => new QuestionVm
            {
                Id = x.Id,
                Text = x.Text,
                QA = x.QA,
                QB = x.QB,
                QC = x.QC,
                QD = x.QD,
                QCorrectAns = x.QCorrectAns,
                Score = x.Score,
                CategoryId = x.CategoryId
            }).ToListAsync();
            return new ApiSuccessResult<List<QuestionVm>>(result);
        }

        public async Task<ApiResult<List<QuestionVm>>> GetAllByCategory(int categoryId)
        {
            var listquestions = await _context.Questions.Where(x => x.CategoryId == categoryId).Select(x => new QuestionVm
            {
                Id = x.Id,
                Text = x.Text,
                QA = x.QA,
                QB = x.QB,
                QC = x.QC,
                QD = x.QD,
                QCorrectAns = x.QCorrectAns,
                Score=x.Score,
                CategoryId = x.CategoryId
            }).ToListAsync();
            return new ApiSuccessResult<List<QuestionVm>>(listquestions);
        }

        public async Task<ApiResult<QuestionVm>> GetById(int id)
        {
            var result = await _context.Questions.FindAsync(id);
            if (result == null)
                return new ApiErrorResult<QuestionVm> { Message = "Không tìm thấy câu hỏi" };
            var question = new QuestionVm
            {
                Id = result.Id,
                Text = result.Text,
                QA = result.QA,
                QB = result.QB,
                QC = result.QC,
                QD = result.QD,
                QCorrectAns = result.QCorrectAns,
                Score = result.Score,
                CategoryId = result.CategoryId
            };
            return new ApiSuccessResult<QuestionVm>(question);
        }

        private bool ChecktrungTextQuestion(int categoryId, string text)
        {
            var result = _context.Questions.Where(x => (x.CategoryId == categoryId && x.Text == text)).Count();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<List<QuestionImportExcelRequest>> ReadExcelFile(Stream fileStream)
        {
            try
            {
                using (var package = new ExcelPackage(fileStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var danhsachcauhois = new List<QuestionImportExcelRequest>();
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string? cauhoi = null;
                        if (worksheet.Cells[row, 1].Value != null)
                        {
                            cauhoi = worksheet.Cells[row, 1].Value.ToString();
                        }
                        string? qa = null;
                        if (worksheet.Cells[row, 2].Value != null)
                        {
                            qa = worksheet.Cells[row, 2].Value.ToString();
                        }
                        string? qb = null;
                        if (worksheet.Cells[row, 3].Value != null)
                        {
                            qb = worksheet.Cells[row, 3].Value.ToString();
                        }
                        string? qc = null;
                        if (worksheet.Cells[row, 4].Value != null)
                        {
                            qc = worksheet.Cells[row, 4].Value.ToString();
                        }
                        string? qd = null;
                        if (worksheet.Cells[row, 5].Value != null)
                        {
                            qd = worksheet.Cells[row, 5].Value.ToString();
                        }
                        string? qcans = "A";
                        if (worksheet.Cells[row, 6].Value != null)
                        {
                            qcans = worksheet.Cells[row, 6].Value.ToString().ToUpper();
                        }
                        var newquestion = new QuestionImportExcelRequest
                        {
                            Text = cauhoi,
                            QA = qa,
                            QB = qb,
                            QC = qc,
                            QD = qd,
                            QCorrectAns = qcans                            
                        };
                        danhsachcauhois.Add(newquestion);
                    }
                    return danhsachcauhois;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing Excel file: " + ex.Message);
            }
        }

        public async Task<ApiResult<ImportExcelResult>> ImportExcelFile(List<QuestionImportExcelRequest> request, int categoryId)
        {
            try
            {
                int lineupdate = 0;
                int linetrung = 0;
                string connectionString = _configuration.GetConnectionString(SystemConstants.MainConnectionString);
                // Create a DataTable to hold your data
                DataTable dataTable = new DataTable("Danhsachcauhoi");
                dataTable.Columns.Add("Text", typeof(string));
                dataTable.Columns.Add("QA", typeof(string));
                dataTable.Columns.Add("QB", typeof(string));
                dataTable.Columns.Add("QC", typeof(string));
                dataTable.Columns.Add("QD", typeof(string));
                dataTable.Columns.Add("QCorrectAns", typeof(string));
                dataTable.Columns.Add("CategoryId", typeof(int));
                foreach (var cauhoi in request)
                {
                    bool isTextExists = ChecktrungTextQuestion(categoryId, cauhoi.Text);
                    if (!isTextExists)
                    {
                        lineupdate++;
                        dataTable.Rows.Add(cauhoi.Text, cauhoi.QA, cauhoi.QB, cauhoi.QC, cauhoi.QD, cauhoi.QCorrectAns, categoryId);
                    }
                    else
                    {
                        linetrung++;
                    }
                }
                using (var bulkCopy = new SqlBulkCopy(connectionString))
                {
                    bulkCopy.DestinationTableName = "Questions";
                    // Đối với các cột không khớp trực tiếp với tên cột trong SQL, bạn có thể ánh xạ chúng.
                    bulkCopy.ColumnMappings.Add("Text", "Text");
                    bulkCopy.ColumnMappings.Add("QA", "QA");
                    bulkCopy.ColumnMappings.Add("QB", "QB");
                    bulkCopy.ColumnMappings.Add("QC", "QC");
                    bulkCopy.ColumnMappings.Add("QD", "QD");
                    bulkCopy.ColumnMappings.Add("QCorrectAns", "QCorrectAns");
                    bulkCopy.ColumnMappings.Add("CategoryId", "CategoryId");
                    // Thiết lập kích thước lô nếu cần
                    bulkCopy.BatchSize = 1000; // Điều chỉnh kích thước lô theo nhu cầu
                    // Thực hiện sao chép dữ liệu vào SQL
                    await bulkCopy.WriteToServerAsync(dataTable);
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

        public async Task<ApiResult<int>> Update(int id, QuestionUpdateRequest request)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<int> { Message = "Không tìm thấy câu hỏi" };
            question.Text = request.Text;
            question.QA = request.QA;
            question.QB = request.QB;
            question.QC = request.QC;
            question.QD = request.QD;
            question.QCorrectAns = request.QCorrectAns.ToUpper();
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = question.Id };
        }

        public async Task<ApiResult<int>> Count(int categoryId)
        {
            int total = await _context.Questions.Where(x => x.CategoryId == categoryId).CountAsync();
            return new ApiSuccessResult<int> { ResultObj = total};
        }

        public async Task<ApiResult<int>> UpdateScore(int id, QuestionUpdateScoreRequest request)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<int> { Message = "Không tìm thấy câu hỏi" };

            question.Score = request.Score;
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = question.Id };
        }

        public async Task<ApiResult<float>> GetTotalScore(int categoryId)
        {
            var result1 = await _context.Questions.Where(x => x.CategoryId == categoryId).ToListAsync();
            //var result2 = await _context.CauHoiTuLuans.Where(x => x.CategoryId == categoryId).ToListAsync();
            var result2 = await _context.cauHoiTrinhTuThaoTacs.Include(x=>x.CauHoiTuLuan).Where(x => x.CauHoiTuLuan.CategoryId == categoryId).ToListAsync();
            float totalScore = 0;
            foreach(var item in result1)
            {
                float score = item.Score ?? 0;
                totalScore += score;
            }
            foreach (var item in result2)
            {
                float score = item.Score ?? 0;
                totalScore += score;
            }
            return new ApiSuccessResult<float> { Score = totalScore };
        }
    }
}
