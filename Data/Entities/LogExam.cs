using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class LogExam
    {
        public int Id { get; set; }
        public int ExamResultId { get; set; }
        public string Cauhoi { get; set; }
        public string QA { get; set; }
        public string QB { get; set; }
        public string QC { get; set; }
        public string QD { get; set; }
        public string Cautraloi { get; set; }
        public string Dapandung { get; set; }
        public ExamResult ExamResult { get; set; }
    }
}
