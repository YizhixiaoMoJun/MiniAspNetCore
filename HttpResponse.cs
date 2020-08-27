using System.Collections.Specialized;
using System.IO;

namespace MiniAspNetCore
{
    public class HttpResponse
    {

        private readonly IResponseFeature feature;
        public HttpResponse(IFeatureCollection features)
        {
            feature = features.GetService<IResponseFeature>();
        }
        public NameValueCollection Headers => feature.Headers;
        public Stream Body => feature.Body;
        public int StatusCode
        {
            get => feature.StatusCode;
            set => feature.StatusCode = value;
        }
    }
}