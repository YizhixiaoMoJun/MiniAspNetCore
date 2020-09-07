using System;
using System.Collections.Specialized;
using System.IO;

namespace MiniAspNetCore
{
    public interface IRequestFeature
    {
        Uri Url { get; }

        NameValueCollection Headers { get;  }

        Stream Body { get; }

    }
}