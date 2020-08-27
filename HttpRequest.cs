using System;
using System.Collections.Specialized;
using System.IO;

namespace MiniAspNetCore
{
    public class HttpRequest
    {
        private readonly IRequestFeature feature;

        public HttpRequest(IFeatureCollection features)
        {
            feature = features.GetService<IRequestFeature>();
        }

        public Uri Url { get { return feature.Url; } }

        public NameValueCollection Headers { get { return feature.Headers; } }

        public Stream Body { get { return feature.Body; } }


    }
}