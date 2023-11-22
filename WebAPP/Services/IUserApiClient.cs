using ViewModels.Common;
using ViewModels.Users.Request;
using ViewModels.Users.Response;

namespace WebAPP.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetuserPaging(GetUserPagingRequest request);

    }
}
