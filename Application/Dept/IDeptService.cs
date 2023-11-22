using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Dept.Response;
using ViewModels.Users.Request;

namespace Application.Dept
{
    public interface IDeptService
    {
        Task<ApiResult<List<DeptVm>>> GetAll();
        Task<ApiResult<bool>> Create(DeptCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(DeptUpdateRequest request);

    }
}
