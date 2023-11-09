using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ExamResult.Request
{
    public class ExamResultSearchRequest
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Score { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
    }
}
