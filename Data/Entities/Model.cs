using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string Name { get; set; }
        public Dept Dept { get; set; }
        public List<Cell> Cells { get; set; }
    }
}
