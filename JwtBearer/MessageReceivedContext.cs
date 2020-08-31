using MiniAspNetCore.Authenication;

namespace MiniAspNetCore.JwtBearer
{
    public class MessageReceivedContext : ResultContext<JwtBearerOptions>
    {
          public MessageReceivedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            JwtBearerOptions options)
            : base(context, scheme, options) { }

        /// <summary>
        /// Bearer Token. This will give the application an opportunity to retrieve a token from an alternative location.
        /// </summary>
        public string Token { get; set; }
    }
}