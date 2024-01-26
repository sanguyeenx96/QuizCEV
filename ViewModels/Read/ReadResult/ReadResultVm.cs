using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Users;

namespace ViewModels.Read.ReadResult
{
    public class ReadResultVm : UserInfoVm
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public int? ReadPostId { get; set; }
        public string? CategoryTitle { get; set; }
        public string? PostTitle { get; set; }
    }
}
