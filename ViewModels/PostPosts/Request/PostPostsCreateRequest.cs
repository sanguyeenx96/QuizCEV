using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PostPosts.Request
{
    public class PostPostsCreateRequest
    {
        public int PostCategoryId { get; set; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
    }
}
