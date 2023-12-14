using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.Common;

namespace WebAPP.Services
{
    public interface ILoiTaiCongDoanApiClient
    {
        Task<ApiResult<List<LoiTaiCongDoanVm>>> GetAllByCauHoiTrinhTuThaoTacId(Guid id);
        Task<ApiResult<bool>> Create(Guid cauhoitrinhtuthaotacId, LoiTaiCongDoanCreateRequest request);
        Task<ApiResult<bool>> Update(int id, LoiTaiCongDoanUpdateRequest request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
