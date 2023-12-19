using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class LogExamDoiSach
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public string CorrectAnswer { get; set; }
        public int LogExamId { get; set; }
        public LogExam LogExam { get; set; }
    }
}
