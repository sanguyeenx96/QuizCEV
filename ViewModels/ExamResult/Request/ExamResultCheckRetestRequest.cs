using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ExamResult.Request
{
    public class ExamResultCheckRetestRequest
    {
        public Guid UserId { get; set; }
        public string CategoryName { get; set; }
    }
}
