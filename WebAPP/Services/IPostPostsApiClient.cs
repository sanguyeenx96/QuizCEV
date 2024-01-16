using ViewModels.Common;
using ViewModels.PostPosts.Request;
using ViewModels.PostPosts.Response;

namespace WebAPP.Services
{
    public interface IPostPostsApiClient
    {
        Task<ApiResult<List<PostPostsVm>>> GetAll();
        Task<ApiResult<List<PostWithOutContentVm>>> GetAllByCategory(int id);
        Task<ApiResult<PostPostsVm>> GetById(int id);
        Task<ApiResult<bool>> Create(PostPostsCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, PostPostsUpdateRequest request);
        Task<ApiResult<bool>> UpdateThumbImage(int id, PostPostThumbImageUpdate request);

    }
}
