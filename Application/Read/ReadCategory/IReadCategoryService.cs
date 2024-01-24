using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostCategory.Request;
using ViewModels.PostCategory.Response;
using ViewModels.Read.ReadCategory;

namespace Application.Read.ReadCategory
{
    public interface IReadCategoryService
    {
        Task<ApiResult<List<ReadCategoryVm>>> GetAll();
        Task<ApiResult<ReadCategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(ReadCategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, ReadCategoryUpdateRequest request);
    }
}
