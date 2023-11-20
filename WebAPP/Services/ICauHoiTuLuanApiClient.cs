using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;

namespace WebAPP.Services
{
    public interface ICauHoiTuLuanApiClient
    {
        Task<ApiResult<List<CauHoiTuLuanVm>>> GetAllByCategory(int id);
        Task<ApiResult<CauHoiTuLuanVm>> GetById(int id);
        Task<ApiResult<int>> Create(CauHoiTuLuanCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> UpdateScore(int id,CauHoiTuLuanUpdateScoreRequest request);
        Task<ApiResult<int>> Count(int categoryId);
        Task<ApiResult<bool>> UpdateText(int id,CauHoiTuLuanUpdateTextRequest request);

    }
}
