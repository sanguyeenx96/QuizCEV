using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModels.Users.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Guest.Controllers
{
    [Area("Guest")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserApiClient _userApiClient;
        private readonly IPostPostsApiClient _postPostsApiClient;
        private readonly IPostCategoryApiClient _postCategoryApiClient;
        private readonly IThongBaoPostApiClient _thongBaoPostApiClient;
        private readonly IThongBaoCategoryApiClient _thongBaoCategoryApiClient;
        public HomeController(IUserApiClient userApiClient, IConfiguration configuration, 
            IPostPostsApiClient postPostsApiClient,IPostCategoryApiClient postCategoryApiClient,
            IThongBaoPostApiClient thongBaoPostApiClient, IThongBaoCategoryApiClient thongBaoCategoryApiClient
            )
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _postPostsApiClient = postPostsApiClient;
            _postCategoryApiClient = postCategoryApiClient;
            _thongBaoCategoryApiClient = thongBaoCategoryApiClient;
            _thongBaoPostApiClient = thongBaoPostApiClient;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userApiClient.Authenticate(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            var token = result.LoginToken;
            var userPrincipal = this.ValidateToken(token);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", token);

            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            var roles = userPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
      
            if (roles.Contains("admin"))
            {
                HttpContext.Session.SetString("Role", "admin");
                return Json(new { success = true, message = "admin" });
            }
            if (roles.Contains("user"))
            {
                HttpContext.Session.SetString("Role", "user");
                return Json(new { success = true, message = "user" });
            }
            else
            {
                return Json(new { success = false, message = "Tài khoản chưa được gán quyền" });
            }
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("Role");

            //return RedirectToAction("Login", "Login");
            return Json(new { success = true });
        }

        public IActionResult CheckRole()
        {
            string role = null;
            string hoten = null;
            string checkrole = HttpContext.Session.GetString("Role");
            if (!string.IsNullOrEmpty(checkrole))
            {
                role = checkrole;
                hoten = User.FindFirst(ClaimTypes.Name).Value.ToString();
            }
            return Json(new { role = role, name = hoten });
        }

        public IActionResult Index()
        {
            return View(); 
        }

        public async Task<IActionResult> ListPost()
        {
            var result = await _postCategoryApiClient.GetAll();
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> GetListPostByCategory(int id)
        {
            var result = await _postPostsApiClient.GetAllByCategory(id);
            return PartialView("guest/listpost/_noidungtab", result.ResultObj);
        }

        public async Task<IActionResult> ViewPost(int id)
        {
            var result = await _postPostsApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Get6News()
        {
            var resultAll = await _postPostsApiClient.Get6();
            var result = resultAll.ResultObj.ToList();
            return PartialView("guest/index/_tintuc", result);
        }

        [HttpGet]
        public async Task<IActionResult> Get3NewsCungChuDe(int id)
        {
            var resultAll = await _postPostsApiClient.GetAllByCategory(id);
            var result = resultAll.ResultObj.Take(3).ToList();
            return PartialView("guest/index/_tintuccungchuyenmuc", result);
        }


        //THÔNG BÁO

        [HttpGet]
        public async Task<IActionResult> Get6ThongBao()
        {
            var resultAll = await _thongBaoPostApiClient.Get6();
            var result = resultAll.ResultObj.ToList();
            return PartialView("guest/index/_thongbao", result);
        }

        public async Task<IActionResult> ViewThongBao(int id)
        {
            var result = await _thongBaoPostApiClient.GetById(id);
            return View(result.ResultObj);
        }
        public async Task<IActionResult> ListThongBao()
        {
            var result = await _thongBaoCategoryApiClient.GetAll();
            return View(result.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> GetListThongBaoByCategory(int id)
        {
            var result = await _thongBaoPostApiClient.GetAllByCategory(id);
            return PartialView("guest/listthongbao/_noidungtab", result.ResultObj);
        }

        public IActionResult DownloadFile(string filename)
        {
            // Đường dẫn thực tế đến file trên máy chủ
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Thông báo", filename);

            // Kiểm tra xem file có tồn tại không
            if (System.IO.File.Exists(filePath))
            {
                // Đọc nội dung của file thành một mảng byte
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                // Xác định loại nội dung của file
                string contentType = "application/octet-stream"; // Loại nội dung tổng quát cho các tệp tin nhị phân
                // Tạo một tên file để hiển thị khi tải về
                string downloadFileName = filename; // Bạn có thể điều chỉnh tên file theo ý muốn
                return File(fileBytes, contentType, downloadFileName);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
