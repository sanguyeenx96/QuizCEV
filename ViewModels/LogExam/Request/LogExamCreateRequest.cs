using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.LogExam.Request
{
    public class LogExamCreateRequest
    {
        public int ExamResultId { get; set; }
        public string? LoaiCauHoi { get; set; } //tn hay tl
        public string? Cauhoi { get; set; }
        public string? QA { get; set; }
        public string? QB { get; set; }
        public string? QC { get; set; }
        public string? QD { get; set; }
        public string? Cautraloi { get; set; }
        public string? Dapandung { get; set; }
        public float Score { get; set; }
        public float FinalScore { get; set; }
    }
}
