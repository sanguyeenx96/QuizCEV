using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Category.Response;
using ViewModels.Common;
using ViewModels.PostCategory.Request;
using ViewModels.PostCategory.Response;

namespace Application.Post.PostCategory
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly TracNghiemCEVDbContext _context;
        public PostCategoryService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> Create(PostCategoryCreateRequest request)
        {
            var checkDuplicateName = await _context.postCategories.Where(x => x.Title == request.Title).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<int>("Chủ đề này đã tồn tại");

            int num = _context.postCategories.Any() ? _context.postCategories.Max(x => x.SortOrder) + 1 : 1;

            var newCategory = new Data.Entities.PostCategory()
            {
                Title  = request.Title,
                SortOrder = num
            };
            _context.postCategories.Add(newCategory);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int> { Id = newCategory.Id };
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var category = await _context.postCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Category có id {id}");
            _context.postCategories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {category.Title}" };
        }

        public async Task<ApiResult<List<PostCategoryVm>>> GetAll()
        {
            var result = await _context.postCategories.Select(x => new PostCategoryVm()
            {
                Id = x.Id,
                Title = x.Title,
                SortOrder = x.SortOrder
            }).ToListAsync();
            return new ApiSuccessResult<List<PostCategoryVm>>(result);
        }

        public async Task<ApiResult<PostCategoryVm>> GetById(int id)
        {
            var category = await _context.postCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<PostCategoryVm>($"Không tìm thấy Category có id  {id}");
            var result = new PostCategoryVm()
            {
                Id = category.Id,
                Title = category.Title,
                SortOrder = category.SortOrder
            };
            return new ApiSuccessResult<PostCategoryVm>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, PostCategoryUpdateRequest request)
        {
            var category = await _context.postCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<bool> { Message = "Không tìm thấy câu hỏi" };
            category.Title = request.Title;

            _context.postCategories.Update(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
