using ViewModels.Category.Request;
using ViewModels.Category.Response;
using ViewModels.Common;

namespace WebAPP.Services
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryVm>>> GetAll();

        Task<ApiResult<CategoryVm>> GetById(int id);

        Task<ApiResult<int>> Create(CategoryCreateRequest request);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<int>> UpdateName(int id, CategoryUpdateNameRequest request);

        Task<ApiResult<bool>> UpdateStatus(int id);

        Task<ApiResult<bool>> UpdateTime(int id,CategoryUpdateTimeRequest request);


    }
}
