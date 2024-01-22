using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ThongBaoPost
    {
        public int Id { get; set; }
        public string Title { set; get; }
        public string? Description { set; get; }
        public string? ThumbImage { set; get; }
        public string? FilePath { set; get; }
        public string Content { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateUpdated { set; get; }
        public int ViewCount { get; set; }
        public int ThongBaoCategoryId { get; set; } // Khóa ngoại đến bảng Category
        public ThongBaoCategory? thongBaoCategory { get; set; } // Thuộc tính navigation
    }
}
