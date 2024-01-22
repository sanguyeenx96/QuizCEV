using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.Common;
using ViewModels.PostCategory.Response;
using ViewModels.ThongBaoCategory.Request;
using ViewModels.ThongBaoCategory.Response;

namespace WebAPP.Services
{
    public class ThongBaoCategoryApiClient : IThongBaoCategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThongBaoCategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<int>> Create(ThongBaoCategoryCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/thongbaocategory/", httpContent);
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
            var response = await client.DeleteAsync($"/api/thongbaocategory/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<ThongBaoCategoryVm>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync("/api/thongbaocategory/");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<ThongBaoCategoryVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<ThongBaoCategoryVm>>>(result);

        }

        public async Task<ApiResult<ThongBaoCategoryVm>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/thongbaocategory/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<ThongBaoCategoryVm>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<ThongBaoCategoryVm>>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, ThongBaoCategoryUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"/api/thongbaocategory/update/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);

        }
    }
}
