using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.Cell.Request;
using ViewModels.Cell.Response;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Dept.Response;
using ViewModels.Model.Request;
using ViewModels.Model.Response;

namespace WebAPP.Services
{
    public class DeptApiClient : IDeptApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeptApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<bool>> Create(DeptCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();//Tạo http client
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);//Cấu hình base address

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");//Lấy giá trị token từ phiên làm việc (session) của người dùng 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);// thiết lập header Authorization cho yêu cầu HTTP. Điều này thường được sử dụng để xác thực người dùng.

            var json = JsonConvert.SerializeObject(request);//chuyển đối tượng request thành chuỗi JSON
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");//Đối tượng httpContent sẽ được sử dụng làm nội dung của yêu cầu HTTP.

            var response = await client.PostAsync("/api/dept/", httpContent);//Sử dụng phương thức PostAsync của HttpClient để gửi yêu cầu HTTP POST đến địa chỉ "/api/dept/".
            var result = await response.Content.ReadAsStringAsync();//Đọc nội dung phản hồi từ yêu cầu HTTP. 
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);// giải mã JSON thành một đối tượng ApiSuccessResult<bool>
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> CreateCell(int ModelId, CellCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();//Tạo http client
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);//Cấu hình base address

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");//Lấy giá trị token từ phiên làm việc (session) của người dùng 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);// thiết lập header Authorization cho yêu cầu HTTP. Điều này thường được sử dụng để xác thực người dùng.

            var json = JsonConvert.SerializeObject(request);//chuyển đối tượng request thành chuỗi JSON
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");//Đối tượng httpContent sẽ được sử dụng làm nội dung của yêu cầu HTTP.

            var response = await client.PostAsync($"/api/dept/createcell/{ModelId}", httpContent);//Sử dụng phương thức PostAsync của HttpClient để gửi yêu cầu HTTP POST đến địa chỉ "/api/dept/".
            var result = await response.Content.ReadAsStringAsync();//Đọc nội dung phản hồi từ yêu cầu HTTP. 
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);// giải mã JSON thành một đối tượng ApiSuccessResult<bool>
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> CreateModel(int DeptId, ModelCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();//Tạo http client
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);//Cấu hình base address

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");//Lấy giá trị token từ phiên làm việc (session) của người dùng 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);// thiết lập header Authorization cho yêu cầu HTTP. Điều này thường được sử dụng để xác thực người dùng.

            var json = JsonConvert.SerializeObject(request);//chuyển đối tượng request thành chuỗi JSON
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");//Đối tượng httpContent sẽ được sử dụng làm nội dung của yêu cầu HTTP.

            var response = await client.PostAsync($"/api/dept/createmodel/{DeptId}", httpContent);//Sử dụng phương thức PostAsync của HttpClient để gửi yêu cầu HTTP POST đến địa chỉ "/api/dept/".
            var result = await response.Content.ReadAsStringAsync();//Đọc nội dung phản hồi từ yêu cầu HTTP. 
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);// giải mã JSON thành một đối tượng ApiSuccessResult<bool>
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/dept/{id}" );
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> DeleteCell(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/dept/deletecell/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> DeleteModel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/dept/deletemodel/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<DeptVm>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync("/api/dept");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<DeptVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<DeptVm>>>(result);
        }
        public async Task<ApiResult<List<ModelVm>>> GetAllByDept(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/dept/getallbydept/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<ModelVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<ModelVm>>>(result);
        }
        public async Task<ApiResult<List<CellVm>>> GetAllByModel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/dept/getallbymodel/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<CellVm>>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<CellVm>>>(result);
        }


        public async Task<ApiResult<bool>> Update(int id, DeptUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/dept/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateCell(int id, CellUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/dept/editcell/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateModel(int id, ModelUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAdress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/dept/editmodel/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
