using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.Common;

namespace Application.CauHoiTrinhTuThaoTac.DiemChuY
{
    public class DiemChuYService : IDiemChuYService
    {
        private readonly TracNghiemCEVDbContext _context;
        public DiemChuYService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(Guid cauhoitrinhtuthaotacId,DiemChuYCreateRequest request)
        {
            var checkDuplicate = await _context.DiemChuYs.Where(x => (x.CauhoitrinhtuthaotacId == cauhoitrinhtuthaotacId && x.Text == request.Text)).FirstOrDefaultAsync();
            if (checkDuplicate != null)
                return new ApiErrorResult<bool> { Message = "Điểm chú ý đã bị trùng" };

            var newDiemChuY = new Data.Entities.TTTTDiemChuY()
            {
                Text = request.Text,
                CauhoitrinhtuthaotacId = cauhoitrinhtuthaotacId
            };
            _context.DiemChuYs.Add(newDiemChuY);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var diemchuy = await _context.DiemChuYs.FindAsync(id);
            if (diemchuy == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
            _context.Remove(diemchuy);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<DiemChuYVm>>> GetAllByCauHoiTrinhTuThaoTacId(Guid id)
        {
            var result = await _context.DiemChuYs.Where(x => x.CauhoitrinhtuthaotacId == id).Select(x => new DiemChuYVm
            {
                Id = x.Id,
                Text = x.Text,
                CauhoitrinhtuthaotacId = x.CauhoitrinhtuthaotacId
            }).ToListAsync();
            if (result == null)
                return new ApiErrorResult<List<DiemChuYVm>> { Message = "Không tìm thấy dữ liệu" };
            return new ApiSuccessResult<List<DiemChuYVm>>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, DiemChuYUpdateRequest request)
        {
            var diemchuy = await _context.DiemChuYs.FindAsync(id);
            if (diemchuy == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy dữ liệu" };
            diemchuy.Text = request.Text;
            _context.DiemChuYs.Update(diemchuy);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
