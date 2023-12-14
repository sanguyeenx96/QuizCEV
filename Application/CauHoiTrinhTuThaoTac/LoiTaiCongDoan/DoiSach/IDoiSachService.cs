using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.Common;

namespace Application.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach
{
    public interface IDoiSachService
    {
        Task<ApiResult<List<DoiSachVm>>> GetAllByLoiTaiCongDoanId(int id);
        Task<ApiResult<bool>> Create(int loitaicongdoanId, DoiSachCreateRequest request);
        Task<ApiResult<bool>> Update(int id, DoiSachUpdateRequest request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
