using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class PostPost
    {
        public int Id { get; set; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateUpdated { set; get; }
        public int ViewCount { get; set; }  
        public int PostCategoryId { get; set; } // Khóa ngoại đến bảng Category
        public PostCategory? postCategory { get; set; } // Thuộc tính navigation
    }
}
