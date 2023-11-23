using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Dept.Response;

namespace WebAPP.Services
{
    public interface IDeptApiClient
    {
        Task<ApiResult<List<DeptVm>>> GetAll();
        Task<ApiResult<bool>> Create(DeptCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, DeptUpdateRequest request);
    }
}
