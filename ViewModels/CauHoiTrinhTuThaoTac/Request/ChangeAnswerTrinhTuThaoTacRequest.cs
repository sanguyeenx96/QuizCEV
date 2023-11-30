using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.Request
{
    public class ChangeAnswerTrinhTuThaoTacRequest
    {
        public Guid Id { get; set; }
        public int ThuTu { get; set; }
        public int CauHoiTuLuanId { get; set; }

    }
}
