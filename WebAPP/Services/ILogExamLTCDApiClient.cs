using ViewModels.Common;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamLoiTaiCongDoan.Request;

namespace WebAPP.Services
{
    public interface ILogExamLTCDApiClient
    {
        Task<ApiResult<bool>> CreateList(List<LogExamLoiTaiCongDoanCreateRequest> request);

    }
}
