using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Read.ReadResult
{
    public class ReadResultSearchRequest
    {
        public Guid? UserId { get; set; }
        public int? readResultId { get; set; }
        public int? readPostId { get; set; }
        public DateTime? Date { get; set; }
        public int? boPhanId { get; set; }
        public int? modelId { get; set; }
        public int? cellId { get; set; }
        public string? userName { get; set; }
        public string? name { get; set; }
    }
}
