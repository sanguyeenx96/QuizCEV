using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ReadCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<ReadPost>? readPosts { get; set; } // Thuộc tính navigation
    }
}
