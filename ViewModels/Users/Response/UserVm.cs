using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Users.Response
{
    public class UserVm
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Dept { get; set; }
        public string? Model { get; set; }
        public string? Cell { get; set; }

        public IList<string> Roles { get; set; }
    }
}
