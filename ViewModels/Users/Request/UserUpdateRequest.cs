using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Users.Request
{
    public class UserUpdateRequest
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public int CellId { get; set; }
    }
}
