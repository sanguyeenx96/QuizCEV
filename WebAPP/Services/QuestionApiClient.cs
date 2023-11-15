using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Text;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Question.Response;
using System.Net.Http.Headers;


namespace WebAPP.Services
{
    public class QuestionApiClient : IQuestionApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public QuestionApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ApiResult<int>> Create(QuestionCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/question/", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.DeleteAsync($"/api/question/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<QuestionVm>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync("/api/question/");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<QuestionVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<QuestionVm>>>(result);
        }

        public async Task<ApiResult<List<QuestionVm>>> GetAllByCategory(int categoryId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync($"/api/question/GetAllByCategory/{categoryId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<QuestionVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<QuestionVm>>>(result);
        }

        public async Task<ApiResult<QuestionVm>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync($"/api/question/GetById/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<QuestionVm>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<QuestionVm>>(result);
        }

        public async Task<ApiResult<ImportExcelResult>> ImportExcel(Stream file, int categoryId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var requestContent = new MultipartFormDataContent();
            // Tạo ByteArrayContent từ Stream
            var fileBytes = new byte[file.Length];
            await file.ReadAsync(fileBytes, 0, (int)file.Length);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "excelfile.xlsx" 
            };
            requestContent.Add(fileContent);
         
            var response = await client.PostAsync($"/api/question/importexcel/{categoryId}", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<ImportExcelResult>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<ImportExcelResult>>(result);
        }

        public async Task<ApiResult<int>> Update(int id, QuestionUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/question/GetById/{id}",httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }
    }
}
