using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;

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

        public async  Task<ApiResult<bool>> Create(CauHoiTrinhTuThaoTacCreateRequest request)
        {
            var checkDuplicateQuestion = await _context.cauHoiTrinhTuThaoTacs.Where(x => (x.Text == request.Text && x.CauHoiTuLuanId == request.CauHoiTuLuanId)).FirstOrDefaultAsync();
            if (checkDuplicateQuestion != null)
                return new ApiErrorResult<bool> { Message = "Câu hỏi đã bị trùng" };
            int num = _context.cauHoiTrinhTuThaoTacs.Where(x => x.CauHoiTuLuanId == request.CauHoiTuLuanId).Any() ? _context.cauHoiTrinhTuThaoTacs.Where(x=>x.CauHoiTuLuanId == request.CauHoiTuLuanId).Max(x => x.ThuTu) + 1 : 1;

            var newQuestion = new Data.Entities.CauHoiTrinhTuThaoTac()
            {
                CauHoiTuLuanId = request.CauHoiTuLuanId,
                Text = request.Text,
                ThuTu = num
            };
            _context.cauHoiTrinhTuThaoTacs.Add(newQuestion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id, CauHoiTrinhTuThaoTacDeleteRequest request)
        {
            var question = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (question == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
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
            var listquestions = await _context.cauHoiTrinhTuThaoTacs.Where(x => x.CauHoiTuLuanId == id).Select(x => new CauHoiTrinhTuThaoTacVm
            {
                Id = x.Id,
                Text = x.Text,
                ThuTu = x.ThuTu,
                CauHoiTuLuanId = x.CauHoiTuLuanId
            }).ToListAsync();
            return new ApiSuccessResult<List<CauHoiTrinhTuThaoTacVm>>(listquestions);
        }

        public async Task<ApiResult<CauHoiTrinhTuThaoTacVm>> GetById(int id)
        {
            var result = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (result == null)
                return new ApiErrorResult<CauHoiTrinhTuThaoTacVm> { Message = "Không tìm thấy câu hỏi" };
            var question = new CauHoiTrinhTuThaoTacVm
            {
                Id = result.Id,
                Text = result.Text,
                ThuTu = result.ThuTu,
                CauHoiTuLuanId = result.CauHoiTuLuanId
            };
            return new ApiSuccessResult<CauHoiTrinhTuThaoTacVm>(question);
        }

        public async Task<ApiResult<bool>> UpdateText(int id, CauHoiTrinhTuThaoTacUpdateTextRequest request)
        {
            var cauhoi = await _context.cauHoiTrinhTuThaoTacs.FindAsync(id);
            if (cauhoi == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            cauhoi.Text = request.Text;
            _context.cauHoiTrinhTuThaoTacs.Update(cauhoi);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
