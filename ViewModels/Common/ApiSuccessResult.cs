
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
        }

        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }

        public ApiSuccessResult(int id)
        {
            IsSuccessed = true;
            Id = id;
        }

        public ApiSuccessResult(float score)
        {
            IsSuccessed = true;
            Score = score;
        }
    }
}