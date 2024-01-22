using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostPosts.Response;
using ViewModels.ThongBaoPost.Request;
using ViewModels.ThongBaoPost.Response;

namespace Application.ThongBao.ThongBaoPost
{
    public class ThongBaoPostService : IThongBaoPostService
    {
        private readonly TracNghiemCEVDbContext _context;
        public ThongBaoPostService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(ThongBaoPostCreateRequest request)
        {
            var checkDuplicateName = await _context.thongBaoPosts.Where(x => (x.Title == request.Title && x.ThongBaoCategoryId == request.ThongBaoCategoryId)).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<bool>("Tiêu đề này đã tồn tại trong chủ đề");
            var newPost = new Data.Entities.ThongBaoPost()
            {
                Title = request.Title,
                Content = request.Content,
                FilePath = request.FilePath,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ViewCount = 0,
                ThongBaoCategoryId = request.ThongBaoCategoryId
            };
            _context.thongBaoPosts.Add(newPost);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var post = await _context.thongBaoPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            _context.thongBaoPosts.Remove(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {post.Title}" };
        }

        public async Task<ApiResult<List<ThongBaoPostVm>>> Get6()
        {
            var result = await _context.thongBaoPosts
           .Select(x => new ThongBaoPostVm()
           {
               Id = x.Id,
               Title = x.Title,
               FilePath = x.FilePath,
               DateCreated = x.DateCreated,
               DateUpdated = x.DateUpdated,
               ViewCount = x.ViewCount,
               ThongBaoCategoryId = x.ThongBaoCategoryId,
               catName = _context.thongBaoCategories
                           .Where(pc => pc.Id == x.ThongBaoCategoryId)
                           .Select(pc => pc.Title)
                           .FirstOrDefault()
           })
           .Take(6)
           .ToListAsync();
            return new ApiSuccessResult<List<ThongBaoPostVm>>(result);
        }

        public async Task<ApiResult<List<ThongBaoPostVm>>> GetAll()
        {
            var result = await _context.thongBaoPosts.Select(x => new ThongBaoPostVm()
            {
                Id = x.Id,
                Title = x.Title,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                FilePath = x.FilePath,
                ViewCount = x.ViewCount,
                ThongBaoCategoryId = x.ThongBaoCategoryId,
                catName = _context.thongBaoCategories
                           .Where(pc => pc.Id == x.ThongBaoCategoryId)
                           .Select(pc => pc.Title)
                           .FirstOrDefault()
            }).ToListAsync();
            return new ApiSuccessResult<List<ThongBaoPostVm>>(result);
        }

        public async Task<ApiResult<List<ThongBaoPostVm>>> GetAllByCategory(int id)
        {
            var category = await _context.thongBaoCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<List<ThongBaoPostVm>>($"Không tìm thấy Category có id {id}");
            var result = _context.thongBaoPosts.Where(x => x.ThongBaoCategoryId == id);
            // Projecting the data into the desired ViewModel
            var listPost = await result.Select(x => new ThongBaoPostVm
            {
                Id = x.Id,
                Title = x.Title,
                FilePath = x.FilePath,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                ViewCount = x.ViewCount,
                ThongBaoCategoryId = x.ThongBaoCategoryId,
                catName = _context.thongBaoCategories
                           .Where(pc => pc.Id == x.ThongBaoCategoryId)
                           .Select(pc => pc.Title)
                           .FirstOrDefault()
            }).ToListAsync();
            return new ApiSuccessResult<List<ThongBaoPostVm>>(listPost);
        }

        public async Task<ApiResult<ThongBaoPostVm>> GetById(int id)
        {
            var post = await _context.thongBaoPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<ThongBaoPostVm>($"Không tìm thấy post có id {id}");
            var result = new ThongBaoPostVm
            {
                Id = post.Id,
                Title = post.Title,
                ThumbImage = post.ThumbImage,
                FilePath = post.FilePath,

                Content = post.Content,
                DateCreated = post.DateCreated,
                DateUpdated = post.DateUpdated,
                ViewCount = post.ViewCount,
                ThongBaoCategoryId = post.ThongBaoCategoryId,
                catName = _context.thongBaoCategories
                            .Where(pc => pc.Id == post.ThongBaoCategoryId)
                            .Select(pc => pc.Title)
                            .FirstOrDefault()
            };
            if (result.catName == null)
            {
                // Xử lý trường hợp không tìm thấy catName
                // Có thể làm thêm log hoặc xử lý khác tùy vào yêu cầu của bạn
                return new ApiErrorResult<ThongBaoPostVm>($"Không tìm thấy catName cho PostCategoryId {post.ThongBaoCategoryId}");
            }

            return new ApiSuccessResult<ThongBaoPostVm>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, ThongBaoPostUpdateRequest request)
        {
            var post = await _context.thongBaoPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            post.Title = request.Title;
            post.Content = request.Content;
            post.DateUpdated = DateTime.Now;
            post.ThongBaoCategoryId = request.ThongBaoCategoryId;
            _context.thongBaoPosts.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
