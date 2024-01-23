using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Read.ReadResult
{
    public class ReadResultVm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ReadPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
