using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.Common;

namespace WebAPP.Services
{
    public interface IDiemChuYApiClient
    {
        Task<ApiResult<List<DiemChuYVm>>> GetAllByCauHoiTrinhTuThaoTacId(Guid id);
        Task<ApiResult<bool>> Create(Guid cauhoitrinhtuthaotacId, DiemChuYCreateRequest request);
        Task<ApiResult<bool>> Update(int id, DiemChuYUpdateRequest request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
