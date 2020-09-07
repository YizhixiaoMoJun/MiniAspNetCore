using System;

namespace MiniAspNetCore
{
    public interface IApplicationBuilder
    {
        IApplicationBuilder Use(Func<HttpRequestDelegate,HttpRequestDelegate> middleware);

        HttpRequestDelegate Build();
    }
}