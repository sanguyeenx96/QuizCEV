using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using ViewModels.Common;
using ViewModels.User.Request;
using ViewModels.User.Response;
using static System.Net.Mime.MediaTypeNames;

namespace Application.User
{
    public class UserService : IUserService
    {
        private readonly TracNghiemCEVDbContext _context;
        public UserService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Create(UserCreateRequest request)
        {
            var checkDuplicateUsername = await _context.Users.Where(x=>x.Username == request.Username).FirstOrDefaultAsync();
            if (checkDuplicateUsername != null)
                return new ApiErrorResult<int>("Tên đăng nhập đã tồn tại");
            var newUser = new Data.Entities.User()
            {
                Username = request.Username,
                Password = request.Password,
                Bophan = request.Bophan,
                Hoten = request.Hoten,
                MaNV = request.MaNV
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newUser.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return new ApiErrorResult<bool>($"Không tìm thấy User có id {id}");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công tài khoản {user.Username}" };
        }

        public async Task<ApiResult<List<UserVm>>> GetAll()
        {
            var result = await _context.Users.Select(x => new UserVm()
            {
                Id = x.Id,
                Username = x.Username,
                Password = x.Password,
                Role = x.Role,
                MaNV = x.MaNV,
                Hoten = x.Hoten,
                Bophan = x.Bophan
            }).ToListAsync();

            return new ApiSuccessResult<List<UserVm>>(result);
        }

        public async Task<ApiResult<UserVm>> GetById(int id)
        {
            var result = await _context.Users.Where(x => x.Id == id).Select(x => new UserVm()
            {
                Id = x.Id,
                Username = x.Username,
                Password = x.Password,
                Role = x.Role,
                MaNV = x.MaNV,
                Hoten = x.Hoten,
                Bophan = x.Bophan
            }).FirstOrDefaultAsync();
            if (result == null)
                return new ApiErrorResult<UserVm>($"Không tìm thấy User có id {id}");
            return new ApiSuccessResult<UserVm>(result);
        }

        public async Task<ApiResult<int>> Update(int id, UserUpdateRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return new ApiErrorResult<int>("Không tìm thấy dữ liệu User");
            user.Username = request.Username;
            user.Password = request.Password;
            user.Role = request.Role;
            user.MaNV = request.MaNV;
            user.Hoten = request.Hoten;
            user.Bophan = request.Bophan;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id=user.Id };
        }
    }
}
