using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ExportExcel
{
    public class ExportExcelBaoCaoKetQuaDaoTaoCreateRequest
    {
        //TOP INFO
        public string? benyeucaudaotao { get; set; }
        public string? benthuchiendaotao { get; set; }
        public string? noidungdaotao { get; set; }
        public string? diadiemdaotao { get; set; }
        public string? thoigian { get; set; }
        public int? thangdiem { get; set; }
        public int? sodiemdat { get; set; }

        //MIDDLE INFO
        public int? stt { get; set; }
        public string? manv { get; set; }
        public string? hoten { get; set; }
        public string? bophan { get; set; }
        public int? diem { get; set; }
        public string? danhgia { get; set; }
    }
}
