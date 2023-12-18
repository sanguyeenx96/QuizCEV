using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CauHoiTrinhTuThaoTac.Response
{
    public class QuestionAndAnswerDoiSachVm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int LoiTaiCongDoanId { get; set; }
        public int Answer { get; set; }
        public int CorrectAnswer { get; set; }

    }

}
