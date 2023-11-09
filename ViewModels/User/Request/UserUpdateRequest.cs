using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.User.Request
{
    public class UserUpdateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Role { get; set; }
        public string MaNV { get; set; }
        public string Hoten { get; set; }
        public string Bophan { get; set; }
    }
}
