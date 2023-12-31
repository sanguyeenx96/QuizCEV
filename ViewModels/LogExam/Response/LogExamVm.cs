﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.LogExamDiemChuY.Response;
using ViewModels.LogExamDoiSach.Response;
using ViewModels.LogExamLoiTaiCongDoan.Response;
using ViewModels.LogExamTrinhtuthaotac.Response;

namespace ViewModels.LogExam.Response
{
    public class LogExamVm
    {
        public int Id { get; set; }
        public int ExamResultId { get; set; }
        public string LoaiCauHoi { get; set; } //tn hay tl
        public string? Cauhoi { get; set; }
        public string? QA { get; set; }
        public string? QB { get; set; }
        public string? QC { get; set; }
        public string? QD { get; set; }
        public string? Cautraloi { get; set; }
        public string? Dapandung { get; set; }
        public float? Score { get; set; }
        public float? FinalScore { get; set; }
        public List<LogExamTrinhtuthaotacVm>? LogExamTrinhtuthaotacs { get; set; }
        public List<LogExamDiemChuYVm>? logExamDiemChuYs { get; set; }
        public List<LogExamLoiTaiCongDoanVm>? logExamLoiTaiCongDoans { get; set; }
        public List<LogExamDoiSachVm>? logExamDoiSaches { get; set; }


    }
}
