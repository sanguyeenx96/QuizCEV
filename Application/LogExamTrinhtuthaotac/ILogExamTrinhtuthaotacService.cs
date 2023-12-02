using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExam.Request;
using ViewModels.LogExamTrinhtuthaotac.Request;

namespace Application.LogExamTrinhtuthaotac
{
    public interface ILogExamTrinhtuthaotacService
    {
        Task<ApiResult<bool>> CreateList(List<LogExamTrinhtuthaotacCreateRequest> request);
    }
}
