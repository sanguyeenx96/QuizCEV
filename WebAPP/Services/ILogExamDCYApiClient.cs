using ViewModels.Common;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace WebAPP.Services
{
    public interface ILogExamDCYApiClient
    {
        Task<ApiResult<bool>> CreateList(List<LogExamDiemChuYCreateRequest> request);

    }
}
