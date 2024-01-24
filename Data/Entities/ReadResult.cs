using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ReadResult
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public int ReadPostId { get; set; }
        public Guid UserId { get; set; }

        public AppUser AppUser { get; set; }
        public ReadPost ReadPost { get; set; }


    }
}
