using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult()
        {
            IsSuccessed = false;
        }
        public ApiErrorResult(string mes)
        {
            IsSuccessed = false;
            Message=mes;
        }

    }
}