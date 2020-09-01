using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model
{
    public class BaseResponse
    {
        public string Message { get; set; }

        public bool Result { get; set; }

        public int StatusCode { get; set; }

        public object Data { get; set; }
    }
}
