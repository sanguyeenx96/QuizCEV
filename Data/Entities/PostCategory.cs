using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class PostCategory
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public ICollection<PostPost>? PostPosts { get; set; } // Thuộc tính navigation

    }
}
