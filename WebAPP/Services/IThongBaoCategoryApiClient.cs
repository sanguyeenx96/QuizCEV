using ViewModels.Common;
using ViewModels.ThongBaoCategory.Request;
using ViewModels.ThongBaoCategory.Response;

namespace WebAPP.Services
{
    public interface IThongBaoCategoryApiClient
    {
        Task<ApiResult<List<ThongBaoCategoryVm>>> GetAll();
        Task<ApiResult<ThongBaoCategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(ThongBaoCategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ThongBaoCategoryUpdateRequest request);
    }
}
