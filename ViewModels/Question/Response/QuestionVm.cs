using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Question.Response
{
    public class QuestionVm
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? QA { get; set; }
        public string? QB { get; set; }
        public string? QC { get; set; }
        public string? QD { get; set; }
        public string? QCorrectAns { get; set; }
        public int? Score { get; set; }
        public int CategoryId { get; set; }
    }
}
