using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Role.Response;

namespace Application.Role
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
