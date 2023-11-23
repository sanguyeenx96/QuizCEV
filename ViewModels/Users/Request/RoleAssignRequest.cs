using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;

namespace ViewModels.Users.Request
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
