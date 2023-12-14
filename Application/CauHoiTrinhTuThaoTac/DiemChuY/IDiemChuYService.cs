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
    public interface IDiemChuYService
    {
        Task<ApiResult<List<DiemChuYVm>>> GetAllByCauHoiTrinhTuThaoTacId(Guid id);
        Task<ApiResult<bool>> Create(Guid cauhoitrinhtuthaotacId, DiemChuYCreateRequest request);
        Task<ApiResult<bool>> Update(int id, DiemChuYUpdateRequest request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
