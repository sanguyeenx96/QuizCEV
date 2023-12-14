using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.Common;

namespace WebAPP.Services
{
    public interface IDoiSachApiClient
    {
        Task<ApiResult<List<DoiSachVm>>> GetAllByLoiTaiCongDoanId(int id);
        Task<ApiResult<bool>> Create(int loitaicongdoanId, DoiSachCreateRequest request);
        Task<ApiResult<bool>> Update(int id, DoiSachUpdateRequest request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
