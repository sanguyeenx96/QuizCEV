using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ExamResult.Response
{
    public class ExamResultVm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Score { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
