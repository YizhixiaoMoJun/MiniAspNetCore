using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniAspNetCore
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly List<Func<HttpRequestDelegate, HttpRequestDelegate>> middlewares = new List<Func<HttpRequestDelegate, HttpRequestDelegate>>();
        public HttpRequestDelegate Build()
        {
            return httpcontext =>{
                HttpRequestDelegate next =
                      _=>{_.Response.StatusCode = 500;return Task.CompletedTask;};
            
            middlewares.Reverse();
            foreach(var middleware in middlewares)
            {
                next =middleware(next);
            }
            return next(httpcontext);
          };
        }

        public IApplicationBuilder Use(Func<HttpRequestDelegate, HttpRequestDelegate> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }
    }
}