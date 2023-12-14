using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.Common;

namespace Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach
{
    public class DoiSachService : IDoiSachService
    {
        private readonly TracNghiemCEVDbContext _context;
        public DoiSachService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(int loitaicongdoanId, DoiSachCreateRequest request)
        {
            var checkDuplicate = await _context.LoiTaiCongDoanDoiSaches.Where(x => (x.LoiTaiCongDoanId == loitaicongdoanId && x.Text == request.Text)).FirstOrDefaultAsync();
            if (checkDuplicate != null)
                return new ApiErrorResult<bool> { Message = "Đối sách đã bị trùng" };

            var newDoiSach = new Data.Entities.LoiTaiCongDoanDoiSach()
            {
                Text = request.Text,
                LoiTaiCongDoanId = loitaicongdoanId
            };
            _context.LoiTaiCongDoanDoiSaches.Add(newDoiSach);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var doisach = await _context.LoiTaiCongDoanDoiSaches.FindAsync(id);
            if (doisach == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
            _context.Remove(doisach);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<DoiSachVm>>> GetAllByLoiTaiCongDoanId(int id)
        {
            var result = await _context.LoiTaiCongDoanDoiSaches.Where(x => x.LoiTaiCongDoanId == id).Select(x => new DoiSachVm
            {
                Id = x.Id,
                Text = x.Text,
                LoiTaiCongDoanId = x.LoiTaiCongDoanId
            }).ToListAsync();
            if (result == null)
                return new ApiErrorResult<List<DoiSachVm>> { Message = "Không tìm thấy dữ liệu" };
            return new ApiSuccessResult<List<DoiSachVm>>(result);

        }

        public async Task<ApiResult<bool>> Update(int id, DoiSachUpdateRequest request)
        {
            var doisach = await _context.LoiTaiCongDoanDoiSaches.FindAsync(id);
            if (doisach == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
            doisach.Text = request.Text;
            _context.LoiTaiCongDoanDoiSaches.Update(doisach);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
