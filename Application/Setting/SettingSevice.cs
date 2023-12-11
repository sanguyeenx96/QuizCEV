using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Setting.Response;

namespace Application.Setting
{
    public class SettingSevice : ISettingService
    {
        private readonly TracNghiemCEVDbContext _context;
        public SettingSevice(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> ChangeSetting(int id)
        {
            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
                return new ApiErrorResult<bool>("Không tìm thấy nội dung cài đặt");
            setting.Status = !setting.Status;
            _context.Settings.Update(setting);

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = "Thành công!" };
        }

        public async Task<ApiResult<List<SettingVm>>> GetAll()
        {
            var result = await _context.Settings.Select(x => new SettingVm()
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status
            }).ToListAsync();
            return new ApiSuccessResult<List<SettingVm>>(result);
        }
    }
}
