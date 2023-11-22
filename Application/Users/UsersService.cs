using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Users.Request;
using ViewModels.Users.Response;

namespace Application.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UsersService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Sai mật khẩu" );
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role, string.Join(";",roles))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],// Issuer: Người phát hành token
                                        _config["Tokens:Issuer"],// Audience: Đối tượng chấp nhận token
                                        claims,// Danh sách các claims (thông tin) bạn muốn đưa vào token
                                        expires: DateTime.Now.AddHours(1),// Thời gian hết hạn của token, ở đây là 1 giờ sau khi tạo
                                        signingCredentials: creds);// Thông tin về việc ký (sign) token

            string resultToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new ApiSuccessResult<string> { LoginToken = resultToken };
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsetPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.Name.Contains(request.Keyword) || x.Dept.Name.Contains(request.Keyword));
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.PasswordHash,
                    Name = x.Name,
                    Dept = x.Dept.Name
                }).ToListAsync();
            //4. Select and projection
            var pagedResult = new PagedResult<UserVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Name = request.Name,
                DeptId = request.DeptId
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if(result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                // Lấy danh sách các thông báo lỗi từ IdentityResult.Errors
                var errorMessages = result.Errors.Select(error => error.Description).ToList();
                return new ApiErrorResult<bool> (string.Join(", ", errorMessages) );
            }
        }
    }
}
