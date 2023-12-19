using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.DiemChuY
{
    public class ChangeAnswerDiemChuYRequest
    {
        public int Id { get; set; }
        public int Answer { get; set; }
        public int CauHoiTuLuanId { get; set; }
    }
}
