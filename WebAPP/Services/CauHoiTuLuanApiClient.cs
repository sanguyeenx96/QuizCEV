using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.CauHoiTuLuan.Request;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;
using ViewModels.Question.Response;

namespace WebAPP.Services
{
    public class CauHoiTuLuanApiClient : ICauHoiTuLuanApiClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CauHoiTuLuanApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<int>> Count(int categoryId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/cauhoituluan/count/{categoryId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public async Task<ApiResult<int>> Create(CauHoiTuLuanCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);


            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/cauhoituluan/", httpContent);
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


            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.DeleteAsync($"/api/cauhoituluan/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<CauHoiTuLuanVm>>> GetAllByCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);


            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/cauhoituluan/GetAllByCategory/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<CauHoiTuLuanVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<CauHoiTuLuanVm>>>(result);
        }

        public async Task<ApiResult<CauHoiTuLuanVm>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[ "BaseAdress"]);


            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/cauhoituluan/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<CauHoiTuLuanVm>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<CauHoiTuLuanVm>>(result);
        }

        public async Task<ApiResult<bool>> UpdateScore(int id, CauHoiTuLuanUpdateScoreRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);


            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/cauhoituluan/updatescore/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateText(int id, CauHoiTuLuanUpdateTextRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);


            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/cauhoituluan/updatetext/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
