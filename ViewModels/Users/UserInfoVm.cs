using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Users
{
    public class UserInfoVm
    {
        public Guid? UserId { get; set; }
        public string? Hoten { get; set; }
        public string? Username { get; set; }
        public string? Bophan { get; set; }
        public string? Model { get; set; }
        public string? Cell { get; set; }
    }
}
