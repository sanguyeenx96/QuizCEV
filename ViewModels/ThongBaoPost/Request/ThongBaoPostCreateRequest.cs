using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ThongBaoPost.Request
{
    public class ThongBaoPostCreateRequest
    {
        public int ThongBaoCategoryId { get; set; }
        public string Title { set; get; }
        public string Content { set; get; }
        public string? FilePath { set; get; }

    }
}
