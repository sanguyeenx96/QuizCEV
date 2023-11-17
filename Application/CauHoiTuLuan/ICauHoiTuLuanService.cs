using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Question.Response;

namespace Application.CauHoiTuLuan
{
    public interface ICauHoiTuLuanService
    {
        Task<ApiResult<List<CauHoiTuLuanVm>>> GetAllByCategory(int id);
        Task<ApiResult<CauHoiTuLuanVm>> GetById(int id);
        Task<ApiResult<int>> Create(CauHoiTuLuanCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<bool>> UpdateScore(int id,CauHoiTuLuanUpdateScoreRequest request);

    }
}
