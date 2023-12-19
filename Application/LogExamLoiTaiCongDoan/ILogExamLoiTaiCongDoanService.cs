using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExamLoiTaiCongDoan.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace Application.LogExamLoiTaiCongDoan
{
    public interface ILogExamLoiTaiCongDoanService
    {
        Task<ApiResult<bool>> CreateList(List<LogExamLoiTaiCongDoanCreateRequest> request);
    }
}
