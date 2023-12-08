using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Cell
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public Model Model { get; set; }
        public List<AppUser> AppUsers { get; set; }

    }
}
