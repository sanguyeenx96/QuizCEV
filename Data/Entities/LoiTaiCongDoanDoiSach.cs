using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class LoiTaiCongDoanDoiSach
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int LoiTaiCongDoanId { get; set; }
        public TTTTLoiTaiCongDoan TTTTLoiTaiCongDoans { get; set; }
    }
}
