using ViewModels.Common;
using ViewModels.Users.Request;
using ViewModels.Users.Response;

namespace WebAPP.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetuserPaging(int id, GetUserPagingRequest request);
        Task<ApiResult<bool>> Create(RegisterRequest request);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<UserVm>> GetById(Guid id);
    }
}
