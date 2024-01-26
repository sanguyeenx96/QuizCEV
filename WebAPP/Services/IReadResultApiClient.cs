using ViewModels.Common;
using ViewModels.Read.ReadResult;

namespace WebAPP.Services
{
    public interface IReadResultApiClient
    {
        Task<ApiResult<List<ReadResultVm>>> Search(ReadResultSearchRequest request);
        Task<ApiResult<int>> Create(ReadResultCreateRequest request);
        Task<ApiResult<int>> CountPerson(ReadResultCountPersonRequest request);

    }
}
