using System;
using System.Security.Principal;
using System.Threading;

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

        public IPrincipal User {get;set;}

        public CancellationToken RequestAborted { get; set; }
    }
}