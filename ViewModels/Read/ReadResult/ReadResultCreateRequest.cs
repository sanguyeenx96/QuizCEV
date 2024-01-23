using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Read.ReadResult
{
    public class ReadResultCreateRequest
    {
        public int ReadPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
