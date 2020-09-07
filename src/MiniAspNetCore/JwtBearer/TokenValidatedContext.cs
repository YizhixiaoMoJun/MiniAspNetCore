using Microsoft.IdentityModel.Tokens;
using MiniAspNetCore.Authenication;

namespace MiniAspNetCore.JwtBearer
{
    public class TokenValidatedContext : ResultContext<JwtBearerOptions>
    {
        public TokenValidatedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            JwtBearerOptions options)
            : base(context, scheme, options) { }

        public SecurityToken SecurityToken { get; set; }
    }
}