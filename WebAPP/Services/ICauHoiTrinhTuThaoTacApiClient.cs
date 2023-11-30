using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.Common;

namespace WebAPP.Services
{
    public interface ICauHoiTrinhTuThaoTacApiClient
    {
        Task<ApiResult<List<CauHoiTrinhTuThaoTacVm>>> GetAllByCauHoiTuLuan(int id);
        Task<ApiResult<CauHoiTrinhTuThaoTacVm>> GetById(int id);
        Task<ApiResult<bool>> Create(CauHoiTrinhTuThaoTacCreateRequest request);
        Task<ApiResult<bool>> Delete(int id, CauHoiTrinhTuThaoTacDeleteRequest request);
        Task<ApiResult<int>> Count(int id);
        Task<ApiResult<bool>> ChangeOrder(List<CauHoiTrinhTuThaoTacChangeOrderRequest> request);
        Task<ApiResult<bool>> UpdateText(int id, CauHoiTrinhTuThaoTacUpdateTextRequest request);

    }
}
