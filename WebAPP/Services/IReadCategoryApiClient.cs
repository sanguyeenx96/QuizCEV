using ViewModels.Common;
using ViewModels.Read.ReadCategory;

namespace WebAPP.Services
{
    public interface IReadCategoryApiClient
    {
        Task<ApiResult<List<ReadCategoryVm>>> GetAll();
        Task<ApiResult<ReadCategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(ReadCategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ReadCategoryUpdateRequest request);

    }
}
