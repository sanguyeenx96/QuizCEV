using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan
{
    public class ChangeAnswerLoiTaiCongDoanRequest
    {
        public int Id { get; set; }
        public int Answer { get; set; }
        public int CauHoiTuLuanId { get; set; }
    }
}
