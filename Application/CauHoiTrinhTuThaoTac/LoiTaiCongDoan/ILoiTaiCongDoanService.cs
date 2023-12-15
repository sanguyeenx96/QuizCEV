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
    public interface ILoiTaiCongDoanService
    {
        Task<ApiResult<List<LoiTaiCongDoanVm>>> GetAllByCauHoiTrinhTuThaoTacId(Guid id);
        Task<ApiResult<LoiTaiCongDoanVm>> GetById(int id);

        Task<ApiResult<bool>> Create(Guid cauhoitrinhtuthaotacId, LoiTaiCongDoanCreateRequest request);
        Task<ApiResult<bool>> Update(int id, LoiTaiCongDoanUpdateRequest request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
