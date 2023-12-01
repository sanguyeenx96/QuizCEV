using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category.Response
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Status { get; set; }
        public int Time { get; set; }



    }
}
