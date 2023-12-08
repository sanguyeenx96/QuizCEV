using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Users.Response;

namespace ViewModels.Cell.Response
{
    public class CellVm
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public int? Soluongtaikhoan { get; set; }
        public List<UserVm>? Users { get; set; }

    }
}
