using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using ViewModels.Category.Response;
using ViewModels.Common;
using ViewModels.Question.Request;
using ViewModels.Users;
using ViewModels.Users.Request;
using ViewModels.Users.Response;

namespace Application.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly TracNghiemCEVDbContext _context;


        public UsersService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration,TracNghiemCEVDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _context = context;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.Users
                        .Include(u => u.Cell) // Sử dụng Include để kèm theo thông tin từ bảng có khóa ngoại
                        .ThenInclude(u => u.Model)
                        .ThenInclude(u => u.Dept)
                        .FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user == null)
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Sai mật khẩu");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Country,user.Cell.Model.Dept.Name)
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

        public async Task<ApiResult<bool>> Create(RegisterRequest request)
        {
            var checkDuplicate = await _userManager.FindByNameAsync(request.UserName);
            if (checkDuplicate != null)
            {
                return new ApiErrorResult<bool>("'Tên đăng nhập (Mã nhân viên) đã bị trùng");
            }
            var user = new AppUser()
            {
                UserName = request.UserName,
                Name = request.Name,
                CellId = request.CellId
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                // Lấy danh sách các thông báo lỗi từ IdentityResult.Errors
                var errorMessages = result.Errors.Select(error => error.Description).ToList();
                return new ApiErrorResult<bool>(string.Join(", ", errorMessages));
            }
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request) //Dùng cái này
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Name = request.Name,
                CellId = request.CellId
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var addRole = await _userManager.AddToRoleAsync(user, request.Role);
                return new ApiSuccessResult<bool>();
            }
            else
            {
                // Lấy danh sách các thông báo lỗi từ IdentityResult.Errors
                var errorMessages = result.Errors.Select(error => error.Description).ToList();
                return new ApiErrorResult<bool>(string.Join(", ", errorMessages));
            }
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            var user = await _userManager.Users
                        .Include(u => u.Cell) // Sử dụng Include để kèm theo thông tin từ bảng có khóa ngoại
                        .ThenInclude(u => u.Model)
                        .ThenInclude(u => u.Dept)
                        .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return new ApiErrorResult<UserVm>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserVm()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Password = user.PasswordHash,
                Dept = user.Cell.Model.Dept.Name,
                Cell = user.Cell.Name,
                Model = user.Cell.Model.Name,
                Roles = roles
            };
            return new ApiSuccessResult<UserVm>(userVm);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            //Xoá hết role không được chọn
            var removeRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            await _userManager.RemoveFromRolesAsync(user, removeRoles);
            //Gán role mới
            var addedRoles = request.Roles.Where(x => x.Selected == true).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<UserVm>>> GetAllByDeptId(int id)
        {
            var query = _userManager.Users;
            if (id != 0)
            {
                query = query.Where(x => x.Cell.Model.Dept.Id == id);
            }
            var data = await query
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.PasswordHash,
                    Name = x.Name,
                    Dept = x.Cell.Model.Dept.Name,
                    Cell = x.Cell.Name,
                    Model = x.Cell.Model.Name,
                }).ToListAsync();
            foreach (var item in data)
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                item.Roles = roles;
            }

            return new ApiSuccessResult<List<UserVm>>(data);
        }

        public async Task<ApiResult<bool>> Phanquyen(Guid id, UserPhanquyenRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            // Xoá hết các vai trò hiện tại của người dùng
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            // Gán vai trò mới
            await _userManager.AddToRoleAsync(user, request.roleName);
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == request.UserName && x.Id != id))
            {
                return new ApiErrorResult<bool>("Tên đăng nhập đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            user.Name = request.Name;
            user.UserName = request.UserName;
            user.CellId = request.CellId;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<ApiResult<bool>> ResetPassword(Guid id, UserResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật mật khẩu không thành công");
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Xoá không thành công");
        }

        public async Task<ApiResult<int>> Count(int id)
        {
            var result = await _userManager.Users.Where(x => x.CellId == id).CountAsync();
            return new ApiSuccessResult<int>(result);
        }

        public async Task<ApiResult<bool>> CheckPassWord(Guid id, UserCheckPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            var result = await _userManager.CheckPasswordAsync(user, request.pwd);
            if (result)
            {
                return new ApiSuccessResult<bool>(true);
            }
            else
            {
                return new ApiErrorResult<bool>("Mật khẩu không đúng");
            }
        }

        private string GetCellValue(ExcelWorksheet worksheet, int row, int column)
        {
            var cellValue = worksheet.Cells[row, column].Value?.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                throw new Exception($"Missing value data at row {row}, column {column}");
            }
            return cellValue;
        }

        public async Task<List<UserImportExcelRequest>> ReadExcelFile(Stream fileStream)
        {
            try
            {
                using (var package = new ExcelPackage(fileStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var danhsachUser = new List<UserImportExcelRequest>();
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var newUser = new UserImportExcelRequest
                        {
                            Name = GetCellValue(worksheet, row, 1),
                            UserName = GetCellValue(worksheet, row, 2),
                            Password = GetCellValue(worksheet, row, 3).ToUpper(),
                        };
                        danhsachUser.Add(newUser);
                    }
                    return danhsachUser;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing Excel file: " + ex.Message);
            }
        }

        public async Task<ApiResult<ImportExcelResult>> ImportExcelFile(List<UserImportExcelRequest> request, string role, int cellId)
        {
            try
            {
                int lineupdate = 0;
                int linetrung = 0;
                foreach (var item in request)
                {
                    var checkDuplicate = await _userManager.FindByNameAsync(item.UserName);
                    if (checkDuplicate != null)
                    {
                        linetrung++;
                    }
                    else
                    {
                        var user = new AppUser()
                        {
                            UserName = item.UserName,
                            Name = item.Name,
                            CellId = cellId
                        };
                        var result = await _userManager.CreateAsync(user, item.Password);
                        if (result.Succeeded)
                        {
                            var addRole = await _userManager.AddToRoleAsync(user, role);
                            lineupdate++;
                        }
                    }               
                }
                var ketqua = new ImportExcelResult()
                {
                    sodongtrung = linetrung,
                    sodongupdate = lineupdate
                };
                return new ApiSuccessResult<ImportExcelResult>(ketqua);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing insert to SQL: " + ex.Message);
            }
        }

    }
}
