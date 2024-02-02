using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.ExportExcel;

namespace WebAPP.Services
{
    public interface IExamResultApiClient
    {
        Task<ApiResult<List<ExamResultVm>>> Getall();

        Task<ApiResult<List<ExamResultVm>>> Search(ExamResultSearchRequest request);

        Task<ApiResult<int>> Create(ExamResultCreateRequest request);

        Task<ApiResult<bool>> CheckRetest(ExamResultCheckRetestRequest request);

        Task<ApiResult<bool>> DanhGia(int id, ExamResultDanhGiaRequest request);

        Task<ApiResult<List<ExamResultVm>>> Searchdata(ExamResultSearchRequest request);

        Task<ApiResult<bool>> ExportExcelFile(Stream file, List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> request);


    }
}
