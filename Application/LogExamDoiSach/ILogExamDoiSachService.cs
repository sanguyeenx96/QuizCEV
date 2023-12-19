using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamDiemChuY.Request;
using ViewModels.LogExamDoiSach.Request;

namespace Application.LogExamDoiSach
{
    public interface ILogExamDoiSachService
    {
        Task<ApiResult<bool>> CreateList(List<LogExamDoiSachCreateRequest> request);

    }
}
