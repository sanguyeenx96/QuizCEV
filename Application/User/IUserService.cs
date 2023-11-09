using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.User.Request;
using ViewModels.User.Response;

namespace Application.User
{
    public interface IUserService 
    {
        Task<ApiResult<List<UserVm>>> GetAll();
        Task<ApiResult<UserVm>> GetById(int id);
        Task<ApiResult<int>> Create(UserCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<int>> Update(int id,UserUpdateRequest request);
    }
}
