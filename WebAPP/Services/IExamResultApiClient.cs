using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;

namespace WebAPP.Services
{
    public interface IExamResultApiClient
    {
        Task<ApiResult<List<ExamResultVm>>> Getall();

        Task<ApiResult<List<ExamResultVm>>> Search(ExamResultSearchRequest request);

        Task<ApiResult<int>> Create(ExamResultCreateRequest request);

        Task<ApiResult<bool>> CheckRetest(ExamResultCheckRetestRequest request);

    }
}
