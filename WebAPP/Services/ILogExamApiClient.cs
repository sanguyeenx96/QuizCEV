using ViewModels.Common;
using ViewModels.LogExam.Request;
using ViewModels.LogExam.Response;

namespace WebAPP.Services
{
    public interface ILogExamApiClient
    {
        Task<ApiResult<LogExamVm>> GetById(int id);

        Task<ApiResult<List<LogExamVm>>> GetByExamId(int id);

        Task<ApiResult<bool>> CreateList(List<LogExamCreateRequest> request);

        Task<ApiResult<int>> CreateSingle(LogExamCreateRequest request);

    }
}
