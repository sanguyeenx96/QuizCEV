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
        public HomeController(IUserApiClient userApiClient, IConfiguration configuration, IPostPostsApiClient postPostsApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _postPostsApiClient = postPostsApiClient;
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
        public IActionResult ViewPost()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get6News()
        {
            var resultAll = await _postPostsApiClient.GetAll();
            var result = resultAll.ResultObj.Take(6).ToList();
            return PartialView("guest/index/_tintuc", result);
        }
    }
}
