using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ThongBaoPost.Response
{
    public class ThongBaoPostVm
    {
        public int Id { get; set; }
        public string Title { set; get; }
        public string? ThumbImage { set; get; }
        public string? Description { set; get; }
        public string? FilePath { set; get; }
        public string? Content { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateUpdated { set; get; }
        public int ViewCount { get; set; }
        public int ThongBaoCategoryId { get; set; }

        public string? catName { get; set; }
    }
}
