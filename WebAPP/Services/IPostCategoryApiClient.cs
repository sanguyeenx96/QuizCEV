using ViewModels.Common;
using ViewModels.PostCategory.Request;
using ViewModels.PostCategory.Response;

namespace WebAPP.Services
{
    public interface IPostCategoryApiClient
    {
        Task<ApiResult<List<PostCategoryVm>>> GetAll();
        Task<ApiResult<PostCategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(PostCategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, PostCategoryUpdateRequest request);
    }
}
