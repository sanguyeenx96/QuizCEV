using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.LogExam.Response;

namespace ViewModels.ExamResult.Response
{
    public class ExamResultVm
    {
        public int Id { get; set; }
        public bool? Result { get; set; }
        public int ThoiGianLamBai { get; set; }
        public int ThoiGianChoPhepLamBai { get; set; }
        public string? CategoryName { get; set; }
        public DateTime Date { get; set; }
        public float Score { get; set; }
        public Guid UserId { get; set; }
        public string? Hoten { get; set; }
        public string? Username { get; set; }

        public string? Bophan { get; set; }
        public string? Model { get; set; }
        public string? Cell { get; set; }


        public int CategoryId { get; set; }
        public List<LogExamVm>? LogExams { get; set; }
    }
}
