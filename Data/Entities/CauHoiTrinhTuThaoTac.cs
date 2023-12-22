
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CauHoiTrinhTuThaoTac
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; }
        public int ThuTu { get; set; }
        public float? Score { get; set; }
        public int CauHoiTuLuanId { get; set; }
        public CauHoiTuLuan CauHoiTuLuan { get; set; }
        public List<TTTTDiemChuY>? TTTTDiemChuYs { get; set; }
        public List<TTTTLoiTaiCongDoan>? TTTTLoiTaiCongDoans { get; set; }

    }
}
