using ViewModels.Common;
using ViewModels.Read.ReadPost;

namespace WebAPP.Services
{
    public interface IReadPostApiClient
    {
        Task<ApiResult<List<ReadPostVm>>> GetAll();
        Task<ApiResult<List<ReadPostVm>>> GetAllByCategory(int id);
        Task<ApiResult<List<ReadPostVm>>> UserGetAllByCategory(int id, ReadPostUserGetAllByCategory request);

        Task<ApiResult<ReadPostVm>> GetById(int id);
        Task<ApiResult<bool>> Create(ReadPostCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ReadPostUpdateRequest request);
        Task<ApiResult<bool>> UpdateThumbImage(int id, ReadPostThumbImageUpdate request);
        Task<ApiResult<bool>> ChangeStatus(int id);

    }
}
