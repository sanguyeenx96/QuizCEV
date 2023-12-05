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
        Task<ApiResult<List<UserVm>>> GetAllByDeptId(int id);
        Task<ApiResult<bool>> Phanquyen(Guid id, UserPhanquyenRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> ResetPassword(Guid id, UserResetPasswordRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<int>> Count(int id);
        Task<ApiResult<bool>> CheckPassWord(Guid id, UserCheckPasswordRequest request);


    }
}
