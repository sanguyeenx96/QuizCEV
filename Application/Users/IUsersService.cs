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
        Task<ApiResult<bool>> Create(RegisterRequest request);

        Task<ApiResult<UserVm>> GetById(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);

    }
}
