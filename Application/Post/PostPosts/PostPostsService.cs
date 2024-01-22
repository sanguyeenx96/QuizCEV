using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.DiemChuY;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan;
using ViewModels.CauHoiTrinhTuThaoTac.Response;
using ViewModels.Common;
using ViewModels.PostCategory.Response;
using ViewModels.PostPosts.Request;
using ViewModels.PostPosts.Response;
using Data.Entities;
using System.Reflection.Metadata;

namespace Application.Post.PostPosts
{
    public class PostPostsService : IPostPostService
    {
        private readonly TracNghiemCEVDbContext _context;
        public PostPostsService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(PostPostsCreateRequest request)
        {
            var checkDuplicateName = await _context.postPosts.Where(x => (x.Title == request.Title && x.PostCategoryId == request.PostCategoryId)).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<bool>("Tiêu đề này đã tồn tại trong chủ đề");

            var newPost = new Data.Entities.PostPost()
            {
                Title = request.Title,
                Description = request.Description,
                ThumbImage = request.ThumbImage,
                Content = request.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ViewCount = 0,
                PostCategoryId = request.PostCategoryId
            };
            _context.postPosts.Add(newPost);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var post = await _context.postPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            _context.postPosts.Remove(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {post.Title}" };
        }

        public async Task<ApiResult<List<PostPostsVm>>> Get6()
        {
            var result = await _context.postPosts
           .Select(x => new PostPostsVm()
           {
               Id = x.Id,
               Title = x.Title,
               Description = x.Description,
               ThumbImage = x.ThumbImage,
               DateCreated = x.DateCreated,
               DateUpdated = x.DateUpdated,
               ViewCount = x.ViewCount,
               PostCategoryId = x.PostCategoryId,
               catName = _context.postCategories
                           .Where(pc => pc.Id == x.PostCategoryId)
                           .Select(pc => pc.Title)
                           .FirstOrDefault()
           })
           .Take(6)
           .ToListAsync();
            return new ApiSuccessResult<List<PostPostsVm>>(result);
        }

        public async Task<ApiResult<List<PostPostsVm>>> GetAll()
        {
            var result = await _context.postPosts.Select(x => new PostPostsVm()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ThumbImage = x.ThumbImage,
                Content = x.Content,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                ViewCount = x.ViewCount,
                PostCategoryId = x.PostCategoryId,
                catName = _context.postCategories
                       .Where(pc => pc.Id == x.PostCategoryId)
                       .Select(pc => pc.Title)
                       .FirstOrDefault()
            }).ToListAsync();
            return new ApiSuccessResult<List<PostPostsVm>>(result);
        }

        public async Task<ApiResult<List<PostWithOutContentVm>>> GetAllByCategory(int id)
        {
            var category = await _context.postCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<List<PostWithOutContentVm>>($"Không tìm thấy Category có id {id}");
            var result = _context.postPosts.Where(x => x.PostCategoryId == id);
            // Projecting the data into the desired ViewModel
            var listPost = await result.Select(x => new PostWithOutContentVm
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ThumbImage = x.ThumbImage,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                ViewCount = x.ViewCount,
                PostCategoryId = x.PostCategoryId,
                catName = _context.postCategories
                            .Where(pc => pc.Id == x.PostCategoryId)
                            .Select(pc => pc.Title)
                            .FirstOrDefault()
            }).ToListAsync();
            return new ApiSuccessResult<List<PostWithOutContentVm>>(listPost);
        }

        public async Task<ApiResult<PostPostsVm>> GetById(int id)
        {
            var post = await _context.postPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<PostPostsVm>($"Không tìm thấy post có id {id}");
            var result = new PostPostsVm
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                ThumbImage = post.ThumbImage,
                Content = post.Content,
                DateCreated = post.DateCreated,
                DateUpdated = post.DateUpdated,
                ViewCount = post.ViewCount,
                PostCategoryId = post.PostCategoryId,
                catName = _context.postCategories
                            .Where(pc => pc.Id == post.PostCategoryId)
                            .Select(pc => pc.Title)
                            .FirstOrDefault()
            };
            if (result.catName == null)
            {
                // Xử lý trường hợp không tìm thấy catName
                // Có thể làm thêm log hoặc xử lý khác tùy vào yêu cầu của bạn
                return new ApiErrorResult<PostPostsVm>($"Không tìm thấy catName cho PostCategoryId {post.PostCategoryId}");
            }

            return new ApiSuccessResult<PostPostsVm>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, PostPostsUpdateRequest request)
        {
            var post = await _context.postPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            post.Title = request.Title;
            post.Description = request.Description;
            post.Content = request.Content;
            post.DateUpdated = DateTime.Now;
            post.PostCategoryId = request.PostCategoryId;
            _context.postPosts.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateThumbImage(int id, PostPostThumbImageUpdate request)
        {
            var post = await _context.postPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            post.ThumbImage = request.ThumbImage;
            _context.postPosts.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
