﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Users.Request
{
    public class UserGetListRequest
    {
        public int? deptId { get; set; }
        public int? modelId { get; set; }
        public int? cellId { get; set; }

    }
}