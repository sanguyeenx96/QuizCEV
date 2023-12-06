using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ExamResult.Request
{
    public class ExamResultSearchRequest
    {
        public Guid? UserId { get; set; }
        public int? examResultId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? Date { get; set; }
        public int? boPhanId { get; set; }
        public string? userName { get; set; }
        public string? name { get; set; }


    }
}
