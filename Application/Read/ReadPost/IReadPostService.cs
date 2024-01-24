using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostPosts.Request;
using ViewModels.PostPosts.Response;
using ViewModels.Read.ReadPost;

namespace Application.Read.ReadPost
{
    public interface IReadPostService
    {
        Task<ApiResult<List<ReadPostVm>>> GetAll();
        Task<ApiResult<List<ReadPostVm>>> GetAllByCategory(int id);
        Task<ApiResult<ReadPostVm>> GetById(int id);
        Task<ApiResult<bool>> Create(ReadPostCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ReadPostUpdateRequest request);
        Task<ApiResult<bool>> UpdateThumbImage(int id, ReadPostThumbImageUpdate request);
    }
}
