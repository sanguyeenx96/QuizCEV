using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Setting
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public bool Status { get; set; }
    }
}
