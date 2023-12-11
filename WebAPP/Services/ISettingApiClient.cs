using Microsoft.AspNetCore.Components.Web;
using ViewModels.Common;
using ViewModels.Dept.Response;
using ViewModels.Setting.Response;

namespace WebAPP.Services
{
    public interface ISettingApiClient
    {
        Task<ApiResult<List<SettingVm>>> GetAll();
        Task<ApiResult<bool>> ChangeStatus(int id);
    }
}
