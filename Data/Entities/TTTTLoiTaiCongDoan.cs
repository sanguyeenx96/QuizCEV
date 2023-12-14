using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TTTTLoiTaiCongDoan
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Guid CauhoitrinhtuthaotacId { get; set; }
        public CauHoiTrinhTuThaoTac CauHoiTrinhTuThaoTac { get; set; }
        public List<LoiTaiCongDoanDoiSach>? LoiTaiCongDoanDoiSachs { get; set; }
    }
}
