using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostCategory.Request;
using ViewModels.PostCategory.Response;
using ViewModels.ThongBaoCategory.Request;
using ViewModels.ThongBaoCategory.Response;

namespace Application.ThongBao.ThongBaoCategory
{
    public interface IThongBaoCategoryService
    {
        Task<ApiResult<List<ThongBaoCategoryVm>>> GetAll();
        Task<ApiResult<ThongBaoCategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(ThongBaoCategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ThongBaoCategoryUpdateRequest request);
    }
}
