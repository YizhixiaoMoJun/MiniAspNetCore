using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace MiniAspNetCore
{
    public class HttpListenerFeature : IRequestFeature, IResponseFeature
    {
        private readonly HttpListenerContext listenerContext;

        public HttpListenerFeature(HttpListenerContext listenerContext)
        {
            this.listenerContext = listenerContext;
        }

        public Uri Url => listenerContext.Request.Url;

        public int StatusCode
        {
            get => listenerContext.Response.StatusCode;
            set => listenerContext.Response.StatusCode = value;
        }

        NameValueCollection IRequestFeature.Headers => listenerContext.Request.Headers;

        NameValueCollection IResponseFeature.Headers => listenerContext.Response.Headers;

        Stream IRequestFeature.Body => listenerContext.Request.InputStream;
        Stream IResponseFeature.Body => listenerContext.Response.OutputStream;
    }
}