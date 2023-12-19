using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.LogExamDiemChuY.Request
{
    public class LogExamDiemChuYCreateRequest
    {
        public string Text { get; set; }
        public int Answer { get; set; }
        public int CorrectAnswer { get; set; }
        public int LogExamId { get; set; }
    }
}
