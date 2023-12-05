using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ExamResult.Request
{
    public class ExamResultCreateRequest
    {
        public float Score { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ThoiGianLamBai { get; set; }
        public int ThoiGianChoPhepLamBai { get; set; }

    }
}
