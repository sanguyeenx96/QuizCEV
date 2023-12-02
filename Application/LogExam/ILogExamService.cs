using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.LogExam.Request;
using ViewModels.LogExam.Response;

namespace Application.LogExam
{
    public interface ILogExamService
    {
        Task<ApiResult<LogExamVm>> GetById(int id);

        Task<ApiResult<List<LogExamVm>>> GetByExamId(int id);

        Task<ApiResult<int>> CreateSingle(LogExamCreateRequest request);

        Task<ApiResult<bool>> CreateList(List<LogExamCreateRequest> request);

    }
}
