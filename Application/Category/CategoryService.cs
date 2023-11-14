using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Category.Request;
using ViewModels.Category.Response;
using ViewModels.Common;
using ViewModels.User.Request;
using ViewModels.User.Response;

namespace Application.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly TracNghiemCEVDbContext _context;

        public CategoryService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> Create(CategoryCreateRequest request)
        {
            var checkDuplicateName = await _context.Categories.Where(x => x.Name == request.Name).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<int>("Chủ đề này đã tồn tại");
            var newCategory = new Data.Entities.Category()
            {
                Name = request.Name,
                DateCreate = DateTime.Now,
                LastUpdate = DateTime.Now

            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newCategory.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Category có id {id}");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {category.Name}" };
        }

        public async Task<ApiResult<List<CategoryVm>>> GetAll()
        {
            var result = await _context.Categories.Select(x => new CategoryVm()
            {
                Id = x.Id,
                Name = x.Name,
                DateCreate = x.DateCreate,
                LastUpdate = x.LastUpdate,
                Status = x.Status
            }).ToListAsync();
            return new ApiSuccessResult<List<CategoryVm>>(result);
        }

        public async Task<ApiResult<CategoryVm>> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<CategoryVm>($"Cannot find an category with id {id}");
            var result = new CategoryVm()
            {
                Id = category.Id,
                Name = category.Name,
                DateCreate = category.DateCreate,
                LastUpdate = category.LastUpdate,
                Status = category.Status
            };
            return new ApiSuccessResult<CategoryVm>(result);
        }

        public async Task<ApiResult<int>> UpdateName(int id, CategoryUpdateNameRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<int>("Không tìm thấy dữ liệu Category");
            category.Name = request.Name;
            category.LastUpdate = DateTime.Now;
            _context.Update(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = category.Id };
        }

        public async Task<ApiResult<bool>> UpdateStatus(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu Category");
            category.Status = !category.Status;
            _context.Update(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}





