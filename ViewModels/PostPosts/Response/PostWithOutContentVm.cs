using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PostPosts.Response
{
    public class PostWithOutContentVm
    {
        public int Id { get; set; }
        public string Title { set; get; }
        public string? ThumbImage { set; get; }
        public string Description { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateUpdated { set; get; }
        public int ViewCount { get; set; }
        public int PostCategoryId { get; set; }
        public string? catName { set; get; }
    }
}
