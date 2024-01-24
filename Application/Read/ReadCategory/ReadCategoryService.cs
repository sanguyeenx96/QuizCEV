using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostCategory.Response;
using ViewModels.Read.ReadCategory;

namespace Application.Read.ReadCategory
{
    public class ReadCategoryService : IReadCategoryService
    {
        private readonly TracNghiemCEVDbContext _context;
        public ReadCategoryService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> Create(ReadCategoryCreateRequest request)
        {
            var checkDuplicateName = await _context.readCategories.Where(x => x.Title == request.Title).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<int>("Chủ đề này đã tồn tại");

            var newCategory = new Data.Entities.ReadCategory()
            {
                Title = request.Title,
            };
            _context.readCategories.Add(newCategory);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newCategory.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var category = await _context.readCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Category có id {id}");
            _context.readCategories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {category.Title}" };
        }

        public async Task<ApiResult<List<ReadCategoryVm>>> GetAll()
        {
            var result = await _context.readCategories.Select(x => new ReadCategoryVm()
            {
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();
            return new ApiSuccessResult<List<ReadCategoryVm>>(result);
        }

        public async Task<ApiResult<ReadCategoryVm>> GetById(int id)
        {
            var category = await _context.readCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<ReadCategoryVm>($"Không tìm thấy Category có id {id}");
            var result = new ReadCategoryVm()
            {
                Id = category.Id,
                Title = category.Title
            };
            return new ApiSuccessResult<ReadCategoryVm>(result);

        }

        public async Task<ApiResult<bool>> Update(int id, ReadCategoryUpdateRequest request)
        {
            var category = await _context.readCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            category.Title = request.Title;

            _context.readCategories.Update(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
