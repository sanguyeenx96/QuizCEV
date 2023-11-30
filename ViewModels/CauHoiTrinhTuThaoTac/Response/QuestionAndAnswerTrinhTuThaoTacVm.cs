using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.Response
{
    public class QuestionAndAnswerTrinhTuThaoTacVm
    {
        public Guid Id { get; set; }
        public string CauHoiTuLuanText { get; set; }
        public string Text { get; set; }
        public int? ThuTu { get; set; }
        public int CauHoiTuLuanId { get; set; }
        public int Answer { get; set; }
    }
}
