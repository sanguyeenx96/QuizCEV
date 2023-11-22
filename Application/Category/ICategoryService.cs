using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Category.Request;
using ViewModels.Category.Response;
using ViewModels.Common;


namespace Application.Category
{
    public interface ICategoryService
    {
        Task<ApiResult<List<CategoryVm>>> GetAll();
        Task<ApiResult<CategoryVm>> GetById(int id);
        Task<ApiResult<int>> Create(CategoryCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<int>> UpdateName(int id, CategoryUpdateNameRequest request);
        Task<ApiResult<bool>> UpdateStatus(int id);
    }
}
