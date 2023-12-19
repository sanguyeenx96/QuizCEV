using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach
{
    public class ChangeAnswerDoiSachRequest
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int CauHoiTuLuanId { get; set; }
    }
}
