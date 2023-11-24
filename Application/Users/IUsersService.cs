using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Users.Request;
using ViewModels.Users.Response;

namespace Application.Users
{
    public interface IUsersService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetUsetPaging(int id, GetUserPagingRequest request);
        Task<ApiResult<List<UserVm>>> GetAllByDeptId(int id);
        Task<ApiResult<bool>> Create(RegisterRequest request);
        Task<ApiResult<UserVm>> GetById(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<bool>> Phanquyen(Guid id, UserPhanquyenRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> ResetPassword(Guid id, UserResetPasswordRequest request);
        Task<ApiResult<bool>> Delete(Guid id);


    }
}
