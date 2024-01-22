using ViewModels.Common;
using ViewModels.ThongBaoPost.Request;
using ViewModels.ThongBaoPost.Response;

namespace WebAPP.Services
{
    public interface IThongBaoPostApiClient
    {
        Task<ApiResult<List<ThongBaoPostVm>>> GetAll();
        Task<ApiResult<List<ThongBaoPostVm>>> Get6();
        Task<ApiResult<List<ThongBaoPostVm>>> GetAllByCategory(int id);
        Task<ApiResult<ThongBaoPostVm>> GetById(int id);
        Task<ApiResult<bool>> Create(ThongBaoPostCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ThongBaoPostUpdateRequest request);
    }
}
