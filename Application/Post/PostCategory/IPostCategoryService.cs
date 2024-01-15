using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Category.Request;
using ViewModels.Category.Response;
using ViewModels.Common;
using ViewModels.PostCategory.Request;
using ViewModels.PostCategory.Response;

namespace Application.Post.PostCategory
{
    public interface IPostCategoryService
    {
        Task<ApiResult<List<PostCategoryVm>>> GetAll();
        Task<ApiResult<PostCategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(PostCategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, PostCategoryUpdateRequest request);
    }
}
