using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace Application.LogExamDiemChuY
{
    public interface ILogExamDiemChuYService
    {
        Task<ApiResult<bool>> CreateList(List<LogExamDiemChuYCreateRequest> request);

    }
}
