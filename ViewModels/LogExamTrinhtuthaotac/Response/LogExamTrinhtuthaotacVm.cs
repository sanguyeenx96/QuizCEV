﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.LogExamTrinhtuthaotac.Response
{
    public class LogExamTrinhtuthaotacVm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ThuTu { get; set; }
        public int Answer { get; set; }
        public int LogExamId { get; set; }
        public float Score { get; set; }
        public float FinalScore { get; set; }

    }
}
