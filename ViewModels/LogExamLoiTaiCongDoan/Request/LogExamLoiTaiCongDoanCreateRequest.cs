using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.LogExamLoiTaiCongDoan.Request
{
    public class LogExamLoiTaiCongDoanCreateRequest
    {
        public string Text { get; set; }
        public int Answer { get; set; }
        public int CorrectAnswer { get; set; }
        public int LogExamId { get; set; }
    }
}
