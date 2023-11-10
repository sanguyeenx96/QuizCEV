using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ViewModels.Category.Request;
using ViewModels.Category.Response;
using ViewModels.Common;

namespace WebAPP.Services
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ApiResult<int>> Create(CategoryCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/category/", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public Task<ApiResult<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<CategoryVm>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync("/api/category/");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<CategoryVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<CategoryVm>>>(result);
        }

        public async Task<ApiResult<CategoryVm>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync($"/api/category/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<CategoryVm>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<CategoryVmS>>(result);
        }

        public Task<ApiResult<int>> UpdateName(int id, CategoryUpdateNameRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<int>> UpdateStatus(int id, CategoryUpdateStatusRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
