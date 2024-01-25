using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.PostPosts.Response;
using ViewModels.Read.ReadPost;

namespace Application.Read.ReadPost
{
    public class ReadPostService : IReadPostService
    {
        private readonly TracNghiemCEVDbContext _context;
        public ReadPostService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            var post = await _context.readPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            post.Status = !post.Status;
            _context.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Create(ReadPostCreateRequest request)
        {
            var checkDuplicateName = await _context.readPosts.Where(x => (x.Title == request.Title && x.ReadCategoryId == request.ReadCategoryId)).FirstOrDefaultAsync();
            if (checkDuplicateName != null)
                return new ApiErrorResult<bool>("Tiêu đề này đã tồn tại trong chủ đề");

            var newPost = new Data.Entities.ReadPost()
            {
                Title = request.Title,
                Description = request.Description,
                ThumbImage = request.ThumbImage,
                Content = request.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ViewCount = 0,
                Time = request.Time,
                Status = true,
                ReadCategoryId = request.ReadCategoryId
            };
            _context.readPosts.Add(newPost);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var post = await _context.readPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            _context.readPosts.Remove(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool> { Message = $"Đã xoá thành công {post.Title}" };
        }

        public async Task<ApiResult<List<ReadPostVm>>> GetAll()
        {
            var result = await _context.readPosts
          .Select(x => new ReadPostVm()
          {
              Id = x.Id,
              Title = x.Title,
              Description = x.Description,
              ThumbImage = x.ThumbImage,
              DateCreated = x.DateCreated,
              DateUpdated = x.DateUpdated,
              ViewCount = x.ViewCount,
              ReadCategoryId = x.ReadCategoryId,
              Status = x.Status,
              Time = x.Time,
              catName = _context.readCategories
                          .Where(pc => pc.Id == x.ReadCategoryId)
                          .Select(pc => pc.Title)
                          .FirstOrDefault()
          })
          .ToListAsync();
            return new ApiSuccessResult<List<ReadPostVm>>(result);
        }

        public async Task<ApiResult<List<ReadPostVm>>> GetAllByCategory(int id)
        {
            var category = await _context.readCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<List<ReadPostVm>>($"Không tìm thấy Category có id {id}");
            var result = _context.readPosts.Where(x => x.ReadCategoryId == id);
            // Projecting the data into the desired ViewModel
            var listPost = await result.Select(x => new ReadPostVm
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ThumbImage = x.ThumbImage,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                ViewCount = x.ViewCount,
                ReadCategoryId = x.ReadCategoryId,
                Status = x.Status,
                Time = x.Time,
                catName = _context.readCategories
                            .Where(pc => pc.Id == x.ReadCategoryId)
                            .Select(pc => pc.Title)
                            .FirstOrDefault()
            }).ToListAsync();
            return new ApiSuccessResult<List<ReadPostVm>>(listPost);
        }

        public async Task<ApiResult<ReadPostVm>> GetById(int id)
        {
            var post = await _context.readPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<ReadPostVm>($"Không tìm thấy post có id {id}");
            var result = new ReadPostVm
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                ThumbImage = post.ThumbImage,
                Content = post.Content,
                DateCreated = post.DateCreated,
                DateUpdated = post.DateUpdated,
                ViewCount = post.ViewCount,
                ReadCategoryId = post.ReadCategoryId,
                Status = post.Status,
                Time = post.Time,
                catName = _context.readCategories
                            .Where(pc => pc.Id == post.ReadCategoryId)
                            .Select(pc => pc.Title)
                            .FirstOrDefault()
            };
            if (result.catName == null)
            {
                // Xử lý trường hợp không tìm thấy catName
                // Có thể làm thêm log hoặc xử lý khác tùy vào yêu cầu của bạn
                return new ApiErrorResult<ReadPostVm>($"Không tìm thấy catName cho PostCategoryId {post.ReadCategoryId}");
            }
            return new ApiSuccessResult<ReadPostVm>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, ReadPostUpdateRequest request)
        {
            var post = await _context.readPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");

            if (request.Title != null)
                post.Title = request.Title;

            if (request.Description != null)
                post.Description = request.Description;

            if (request.Content != null)
                post.Content = request.Content;

            if (request.ReadCategoryId != 0) // Assuming 0 is not a valid ReadCategoryId
                post.ReadCategoryId = request.ReadCategoryId;

            if (request.Time.HasValue && request.Time.Value != null)
                post.Time = request.Time.Value;

            if (request.Status.HasValue && request.Status.Value != null)
                post.Status = request.Status.Value;

            post.DateUpdated = DateTime.Now;

            _context.readPosts.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateThumbImage(int id, ReadPostThumbImageUpdate request)
        {
            var post = await _context.readPosts.FindAsync(id);
            if (post == null)
                return new ApiErrorResult<bool>($"Không tìm thấy post có id {id}");
            post.ThumbImage = request.ThumbImage;
            _context.readPosts.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();

        }

        public async Task<ApiResult<List<ReadPostVm>>> UserGetAllByCategory(int id, ReadPostUserGetAllByCategory request)
        {
            var category = await _context.readCategories.FindAsync(id);
            if (category == null)
                return new ApiErrorResult<List<ReadPostVm>>($"Không tìm thấy Category có id {id}");

            var result = _context.readPosts.Include(x => x.readResults).Where(x => x.ReadCategoryId == id);

            var listPost = await result.ToListAsync(); // Thực hiện truy vấn đồng bộ ở đây

            var listPostVm = new List<ReadPostVm>();
            foreach (var x in listPost)
            {
                var isReaded = await _context.readResults
                    .AnyAsync(rr => rr.ReadPostId == x.Id && rr.UserId == request.userId);

                var catName = await _context.readCategories
                    .Where(pc => pc.Id == x.ReadCategoryId)
                    .Select(pc => pc.Title)
                    .FirstOrDefaultAsync();

                listPostVm.Add(new ReadPostVm
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ThumbImage = x.ThumbImage,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ViewCount = x.ViewCount,
                    ReadCategoryId = x.ReadCategoryId,
                    Status = x.Status,
                    Time = x.Time,
                    catName = catName,
                    isReaded = isReaded
                });
            }
            return new ApiSuccessResult<List<ReadPostVm>>(listPostVm);
        }

    }
}
