using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Status { get; set; }
        public int Time { get; set; }
        public List<Question> Questions { get; set; }
        public List<CauHoiTuLuan> cauHoiTuLuans { get; set; }
        public List<ExamResult> ExamResults { get; set; }
    }
}
