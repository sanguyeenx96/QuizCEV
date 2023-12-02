using ViewModels.Common;
using ViewModels.LogExam.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace WebAPP.Services
{
    public interface ILogExamTrinhtuthaotacApiClient
    {
        Task<ApiResult<bool>> CreateList(List<LogExamTrinhtuthaotacCreateRequest> request);

    }
}
