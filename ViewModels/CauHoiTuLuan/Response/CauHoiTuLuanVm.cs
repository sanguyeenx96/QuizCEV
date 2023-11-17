using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTuLuan.Response
{
    public class CauHoiTuLuanVm
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public float? Score { get; set; }
        public int CategoryId { get; set; }
    }
}
