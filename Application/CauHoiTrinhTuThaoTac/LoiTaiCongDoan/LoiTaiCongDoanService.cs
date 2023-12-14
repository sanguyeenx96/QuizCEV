using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.Common;

namespace Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan
{
    public class LoiTaiCongDoanService : ILoiTaiCongDoanService
    {
        private readonly TracNghiemCEVDbContext _context;
        public LoiTaiCongDoanService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(Guid cauhoitrinhtuthaotacId, LoiTaiCongDoanCreateRequest request)
        {
            var checkDuplicate = await _context.LoiTaiCongDoans.Where(x => (x.CauhoitrinhtuthaotacId == cauhoitrinhtuthaotacId && x.Text == request.Text)).FirstOrDefaultAsync();
            if (checkDuplicate != null)
                return new ApiErrorResult<bool> { Message = "Nội dung lỗi tại công đoạn này đã bị trùng" };

            var newLoiTaiCongDoan = new Data.Entities.TTTTLoiTaiCongDoan()
            {
                Text = request.Text,
                CauhoitrinhtuthaotacId = cauhoitrinhtuthaotacId
            };
            _context.LoiTaiCongDoans.Add(newLoiTaiCongDoan);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var loitaicongdoan = await _context.LoiTaiCongDoans.FindAsync(id);
            if (loitaicongdoan == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
            _context.Remove(loitaicongdoan);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<LoiTaiCongDoanVm>>> GetAllByCauHoiTrinhTuThaoTacId(Guid id)
        {
            var result = await _context.LoiTaiCongDoans.Where(x => x.CauhoitrinhtuthaotacId == id).Select(x => new LoiTaiCongDoanVm
            {
                Id = x.Id,
                Text = x.Text,
                CauhoitrinhtuthaotacId = x.CauhoitrinhtuthaotacId
            }).ToListAsync();
            if (result == null)
                return new ApiErrorResult<List<LoiTaiCongDoanVm>> { Message = "Không tìm thấy dữ liệu" };
            return new ApiSuccessResult<List<LoiTaiCongDoanVm>>(result);

        }

        public async Task<ApiResult<bool>> Update(int id, LoiTaiCongDoanUpdateRequest request)
        {
            var loitaicongdoan = await _context.LoiTaiCongDoans.FindAsync(id);
            if (loitaicongdoan == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
            loitaicongdoan.Text = request.Text;
            _context.LoiTaiCongDoans.Update(loitaicongdoan);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();

        }
    }
}
