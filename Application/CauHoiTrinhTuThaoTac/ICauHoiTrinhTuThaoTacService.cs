using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;

namespace Application.CauHoiTrinhTuThaoTac
{
    public interface ICauHoiTrinhTuThaoTacService
    {
        Task<ApiResult<List<CauHoiTrinhTuThaoTacVm>>> GetAllByCauHoiTuLuan(int id);
        Task<ApiResult<CauHoiTrinhTuThaoTacVm>> GetById(int id);
        Task<ApiResult<int>> Create(CauHoiTrinhTuThaoTacCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> UpdateScore(int id, CauHoiTrinhTuThaoTacUpdateScoreRequest request);
        Task<ApiResult<int>> Count(int id);
        Task<ApiResult<bool>> ChangeOrder(List<CauHoiTrinhTuThaoTacChangeOrderRequest> request);

    }
}
