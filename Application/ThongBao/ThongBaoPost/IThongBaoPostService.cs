using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostPosts.Request;
using ViewModels.PostPosts.Response;
using ViewModels.ThongBaoPost.Request;
using ViewModels.ThongBaoPost.Response;

namespace Application.ThongBao.ThongBaoPost
{
    public interface IThongBaoPostService
    {
        Task<ApiResult<List<ThongBaoPostVm>>> GetAll();
        Task<ApiResult<List<ThongBaoPostVm>>> Get6();
        Task<ApiResult<List<ThongBaoPostVm>>> GetAllByCategory(int id);
        Task<ApiResult<ThongBaoPostVm>> GetById(int id);
        Task<ApiResult<bool>> Create(ThongBaoPostCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ThongBaoPostUpdateRequest request);
    }
}

