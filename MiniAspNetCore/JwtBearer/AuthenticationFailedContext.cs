using System;
using MiniAspNetCore.Authenication;
namespace MiniAspNetCore.JwtBearer
{
    public class AuthenticationFailedContext: ResultContext<JwtBearerOptions>
    {
        public AuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            JwtBearerOptions options)
            : base(context, scheme, options) { }

        public Exception Exception { get; set; }
    }
}