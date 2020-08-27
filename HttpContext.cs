using System;

namespace MiniAspNetCore
{
    public class HttpContext
    {
        public HttpContext(IFeatureCollection features)
        {
            Request = new HttpRequest(features);
            Response = new HttpResponse(features);
        }


        public HttpRequest Request { get; set; }

        public HttpResponse Response { get; set; }

        public IServiceProvider RequestServices { get; set; }
    }
}