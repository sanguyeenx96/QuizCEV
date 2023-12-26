using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Question.Request
{
    public class QuestionImportExcelRequest
    {
        public string? Text { get; set; }
        public string? QA { get; set; }
        public string? QB { get; set; }
        public string? QC { get; set; }
        public string? QD { get; set; }
        public string? QCorrectAns { get; set; }
        public float? Score { get; set; }
    }
}
