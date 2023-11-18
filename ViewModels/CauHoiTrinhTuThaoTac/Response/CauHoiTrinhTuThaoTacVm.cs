using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.Response
{
    public class CauHoiTrinhTuThaoTacVm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? ThuTu { get; set; }
        public float? Score { get; set; }
        public int CauHoiTuLuanId { get; set; }

    }
}
