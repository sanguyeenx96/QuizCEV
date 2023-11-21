using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ExamResult
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<LogExam> LogExams { get; set; }
        public AppUser AppUser { get; set; }
    }
}
