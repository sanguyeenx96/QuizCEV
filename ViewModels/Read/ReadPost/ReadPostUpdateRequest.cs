using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Read.ReadPost
{
    public class ReadPostUpdateRequest
    {
        public int ReadCategoryId { get; set; }
        public string? Title { set; get; }
        public string? Description { set; get; }
        public string? Content { set; get; }
        public int? Time { get; set; }
        public bool? Status { get; set; }
    }
}
