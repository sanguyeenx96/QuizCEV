﻿using System;
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
        public string Answer { get; set; }
        public string CorrectAnswer { get; set; }
        public int CauHoiTuLuanId { get; set; }

    }

}
