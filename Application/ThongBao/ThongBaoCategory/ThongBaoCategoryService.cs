using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostCategory.Response;
using ViewModels.ThongBaoCategory.Request;
using ViewModels.ThongBaoCategory.Response;

namespace Application.ThongBao.ThongBaoCategory
{
    public class ThongBaoCategoryService : IThongBaoCategoryService
    {
        private readonly TracNghiemCEVDbContext _context;
        public ThongBaoCategoryService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> Create(ThongBaoCategoryCreateRequest request)
        {
            var checkDuplicateName = await _context.thongBaoCategories.Where(x => x.Title == request.Title).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<int>("Chủ đề này đã tồn tại");
            int num = _context.thongBaoCategories.Any() ? _context.thongBaoCategories.Max(x => x.SortOrder) + 1 : 1;
            var newCategory = new Data.Entities.ThongBaoCategory()
            {
                Title = request.Title,
                SortOrder = num
            };
            _context.thongBaoCategories.Add(newCategory);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newCategory.Id };

        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var category = await _context.thongBaoCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Category có id {id}");
            _context.thongBaoCategories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {category.Title}" };

        }

        public async Task<ApiResult<List<ThongBaoCategoryVm>>> GetAll()
        {
            var result = await _context.thongBaoCategories.Select(x => new ThongBaoCategoryVm()
            {
                Id = x.Id,
                Title = x.Title,
                SortOrder = x.SortOrder
            }).ToListAsync();
            return new ApiSuccessResult<List<ThongBaoCategoryVm>>(result);
        }

        public async Task<ApiResult<ThongBaoCategoryVm>> GetById(int id)
        {
            var category = await _context.thongBaoCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<ThongBaoCategoryVm>($"Không tìm thấy Category có id  {id}");
            var result = new ThongBaoCategoryVm()
            {
                Id = category.Id,
                Title = category.Title,
                SortOrder = category.SortOrder
            };
            return new ApiSuccessResult<ThongBaoCategoryVm>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, ThongBaoCategoryUpdateRequest request)
        {
            var category = await _context.thongBaoCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            category.Title = request.Title;

            _context.thongBaoCategories.Update(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();

        }
    }
}
