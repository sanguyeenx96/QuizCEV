﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;
using ViewModels.Question.Request;

namespace Application.CauHoiTrinhTuThaoTac
{
    public interface ICauHoiTrinhTuThaoTacService
    {
        Task<ApiResult<List<CauHoiTrinhTuThaoTacVm>>> GetAllByCauHoiTuLuan(int id);
        Task<ApiResult<CauHoiTrinhTuThaoTacVm>> GetById(Guid id);
        Task<ApiResult<bool>> Create(CauHoiTrinhTuThaoTacCreateRequest request);
        Task<ApiResult<bool>> Delete(Guid id,CauHoiTrinhTuThaoTacDeleteRequest request);
        Task<ApiResult<int>> Count(int id);
        Task<ApiResult<bool>> ChangeOrder(List<CauHoiTrinhTuThaoTacChangeOrderRequest> request);
        Task<ApiResult<bool>> UpdateText(Guid id, CauHoiTrinhTuThaoTacUpdateTextRequest request);
        Task<ApiResult<bool>> UpdateScore(Guid id, CauHoiTrinhTuThaoTacUpdateScoreRequest request);


        //UploadExcel
        Task<List<CauHoiTrinhThuThaoTacImportExcelRequest>> ReadExcelFile(Stream fileStream);
        Task<ApiResult<ImportExcelResult>> ImportExcelFile(List<CauHoiTrinhThuThaoTacImportExcelRequest> request, int cauhoituluanId);


    }
}
