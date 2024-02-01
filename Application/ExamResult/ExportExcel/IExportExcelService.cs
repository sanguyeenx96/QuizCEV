using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ExamResult.Request;
using ViewModels.ExamResult.Response;
using ViewModels.ExportExcel;

namespace Application.ExamResult.ExportExcel
{
    public interface IExportExcelService
    {
        Task<ApiResult<List<ExamResultVm>>> Searchdata(ExamResultSearchRequest request);
        Task<ApiResult<bool>> ExportExcelFile(Stream fileStream, List<ExportExcelBaoCaoKetQuaDaoTaoCreateRequest> request);
    }
}
