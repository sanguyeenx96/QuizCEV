using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ThongBaoPost.Request
{
    public class ThongBaoPostUpdateRequest
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public int ThongBaoCategoryId { get; set; }
    }
}
