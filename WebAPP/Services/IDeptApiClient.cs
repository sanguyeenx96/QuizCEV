using ViewModels.Cell.Request;
using ViewModels.Cell.Response;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Dept.Response;
using ViewModels.Model.Request;
using ViewModels.Model.Response;

namespace WebAPP.Services
{
    public interface IDeptApiClient
    {
        //Dept
        Task<ApiResult<List<DeptVm>>> GetAll();
        Task<ApiResult<bool>> Create(DeptCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> Update(int id, DeptUpdateRequest request);

        //Model
        Task<ApiResult<List<ModelVm>>> GetAllByDept(int id);

        Task<ApiResult<bool>> CreateModel(int DeptId, ModelCreateRequest request);
        Task<ApiResult<bool>> UpdateModel(int id, ModelUpdateRequest request);
        Task<ApiResult<bool>> DeleteModel(int id);

        //Cell
        Task<ApiResult<List<CellVm>>> GetAllByModel(int id);

        Task<ApiResult<bool>> CreateCell(int ModelId, CellCreateRequest request);
        Task<ApiResult<bool>> UpdateCell(int id, CellUpdateRequest request);
        Task<ApiResult<bool>> DeleteCell(int id);
    }
}
