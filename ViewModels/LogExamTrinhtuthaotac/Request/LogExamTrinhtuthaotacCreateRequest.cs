using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.LogExamTrinhtuthaotac.Request
{
    public class LogExamTrinhtuthaotacCreateRequest
    {
        public string Text { get; set; }
        public int ThuTu { get; set; }
        public int Answer { get; set; }
        public int LogExamId { get; set; }
    }
}
