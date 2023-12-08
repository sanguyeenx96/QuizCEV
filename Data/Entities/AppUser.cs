using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public int CellId { get; set; }
        public List<ExamResult> ExamResults { get; set; }
        public Cell Cell { get; set; }
    }
}
