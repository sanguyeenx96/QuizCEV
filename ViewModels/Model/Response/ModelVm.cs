using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Model.Response
{
    public class ModelVm
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string Name { get; set; }
        public int? Soluongtaikhoan { get; set; }

        public List<ViewModels.Cell.Response.CellVm>? Cells { get; set; }
    }
}
