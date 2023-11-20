using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CauHoiTuLuan
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public float? Score { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<CauHoiTrinhTuThaoTac> cauHoiTrinhTuThaoTacs { get; set; }

    }
}
