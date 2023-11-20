using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ViewModels.CauHoiTrinhTuThaoTac.Request;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.CauHoiTuLuan.Response;
using ViewModels.Common;

namespace WebAPP.Services
{
    public class CauHoiTrinhTuThaoTacApiClient : ICauHoiTrinhTuThaoTacApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public CauHoiTrinhTuThaoTacApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ApiResult<bool>> ChangeOrder(List<CauHoiTrinhTuThaoTacChangeOrderRequest> request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/cauhoitrinhtuthaotac/ChangeOrder/", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<int>> Count(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync($"/api/cauhoitrinhtuthaotac/count/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public async Task<ApiResult<int>> Create(CauHoiTrinhTuThaoTacCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/cauhoitrinhtuthaotac/", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id, CauHoiTrinhTuThaoTacDeleteRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/cauhoitrinhtuthaotac/delete/{id}",httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<CauHoiTrinhTuThaoTacVm>>> GetAllByCauHoiTuLuan(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync($"/api/cauhoitrinhtuthaotac/GetAllByCauHoiTuLuan/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<CauHoiTrinhTuThaoTacVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<CauHoiTrinhTuThaoTacVm>>>(result);
        }

        public async Task<ApiResult<CauHoiTrinhTuThaoTacVm>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var response = await client.GetAsync($"/api/cauhoitrinhtuthaotac/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<CauHoiTrinhTuThaoTacVm>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<CauHoiTrinhTuThaoTacVm>>(result);
        }

        public async Task<ApiResult<bool>> UpdateText(int id, CauHoiTrinhTuThaoTacUpdateTextRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/cauhoitrinhtuthaotac/updatetext/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
