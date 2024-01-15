using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostCategory.Request;
using ViewModels.PostCategory.Response;
using ViewModels.PostPosts.Request;
using ViewModels.PostPosts.Response;

namespace Application.Post.PostPosts
{
    public interface IPostPostService
    {
        Task<ApiResult<List<PostPostsVm>>> GetAll();
        Task<ApiResult<List<PostPostsVm>>> GetAllByCategory(int id);
        Task<ApiResult<PostPostsVm>> GetById(int id);
        Task<ApiResult<int>> Create(PostPostsCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, PostPostsUpdateRequest request);
    }
}
