using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Role { get; set; }
        public string MaNV { get; set; }
        public string Hoten { get; set; }
        public string Bophan { get; set; }
        public List<ExamResult> ExamResults { get; set; }
        public List<LogExam> LogExams { get; set; }
    }
}
