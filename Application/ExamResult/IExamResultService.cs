using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;

namespace Application.ExamResult
{
    public interface IExamResultService 
    {
        Task<ApiResult<List<ExamResultVm>>> Getall();

        //Task<ApiResult<List<ExamResultVm>>> Search(ExamResultSearchRequest request);

        Task<ApiResult<int>> Create(ExamResultCreateRequest request);

        //Task<ApiResult<int>> Update(int id, ExamResultUpdateRequest request);

        //Task<ApiResult<bool>> Delete(int id);

    }
}
