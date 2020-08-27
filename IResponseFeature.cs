using System.Collections.Specialized;
using System.IO;

namespace MiniAspNetCore
{
    public interface IResponseFeature
    {
        NameValueCollection Headers { get; }

        Stream Body { get; }

        int StatusCode { get; set; }
    }
}