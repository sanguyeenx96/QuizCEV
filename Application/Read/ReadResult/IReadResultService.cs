using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.Read.ReadResult;

namespace Application.Read.ReadResult
{
    public interface IReadResultService
    {
        Task<ApiResult<List<ReadResultVm>>> Search(ReadResultSearchRequest request);
        Task<ApiResult<int>> Create(ReadResultCreateRequest request);
    }
}
