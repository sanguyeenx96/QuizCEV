using ViewModels.Common;
using ViewModels.Users.Request;

namespace WebAPP.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
    }
}
