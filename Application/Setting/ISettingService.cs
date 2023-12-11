using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Setting.Response;

namespace Application.Setting
{
    public interface ISettingService
    {
        Task<ApiResult<List<SettingVm>>> GetAll();
        Task<ApiResult<bool>> ChangeSetting(int id);
    }
}
