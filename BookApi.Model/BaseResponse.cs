using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Model
{
    /// <summary>
    /// base response
    /// </summary>
    public class BaseResponse
    {
        public string Message { get; set; }

        public bool Result { get; set; }

        public int StatusCode { get; set; }

    }

    /// <summary>
    /// base response with data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }


}
