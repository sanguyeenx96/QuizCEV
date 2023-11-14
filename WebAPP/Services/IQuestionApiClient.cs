﻿using ViewModels.Category.Request;
using ViewModels.Category.Response;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Question.Response;

namespace WebAPP.Services
{
    public interface IQuestionApiClient
    {
        Task<ApiResult<List<QuestionVm>>> GetAll();

        Task<ApiResult<List<QuestionVm>>> GetAllByCategory(int categoryId);

        Task<ApiResult<QuestionVm>> GetById(int id);

        Task<ApiResult<int>> Create(QuestionCreateRequest request);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<int>> Update(int id, QuestionUpdateRequest request);
    }
}