using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.Response
{
    public class QuestionAndAnswerDiemChuYVm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Guid CauhoitrinhtuthaotacId { get; set; }
        public int Answer { get; set; }
        public int CorrectAnswer { get; set; }
        public int CauHoiTuLuanId { get; set; }

    }

}
