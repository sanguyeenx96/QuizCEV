using ViewModels.Common;
using ViewModels.Role.Response;

namespace WebAPP.Services
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
