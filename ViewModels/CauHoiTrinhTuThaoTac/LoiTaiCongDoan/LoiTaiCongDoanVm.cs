using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan.DoiSach;

namespace ViewModels.CauHoiTrinhTuThaoTac.LoiTaiCongDoan
{
    public class LoiTaiCongDoanVm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ChooseNumber { get; set; }

        public Guid CauhoitrinhtuthaotacId { get; set; }
        public List<DoiSachVm>? doiSaches { get; set; }

    }
}
