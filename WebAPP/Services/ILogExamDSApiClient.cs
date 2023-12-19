using ViewModels.Common;
using ViewModels.LogExamDoiSach.Request;
using ViewModels.LogExamLoiTaiCongDoan.Request;

namespace WebAPP.Services
{
    public interface ILogExamDSApiClient
    {
        Task<ApiResult<bool>> CreateList(List<LogExamDoiSachCreateRequest> request);

    }
}
