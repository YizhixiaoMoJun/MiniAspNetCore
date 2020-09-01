using System;

namespace MiniAspNetCore.Authenication
{
    public abstract class BaseContext<TOptions> where TOptions : AuthenticationSchemeOptions
    {
        protected BaseContext(HttpContext context,AuthenticationScheme scheme,TOptions options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (scheme == null)
            {
                throw new ArgumentNullException(nameof(scheme));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            HttpContext = context;
            Scheme = scheme;
            Options = options;
        }
           /// <summary>
        /// The authentication scheme.
        /// </summary>
        public AuthenticationScheme Scheme { get; }

        /// <summary>
        /// Gets the authentication options associated with the scheme.
        /// </summary>
        public TOptions Options { get; }

        /// <summary>
        /// The context.
        /// </summary>
        public HttpContext HttpContext { get; }

        /// <summary>
        /// The request.
        /// </summary>
        public HttpRequest Request => HttpContext.Request;

        /// <summary>
        /// The response.
        /// </summary>
        public HttpResponse Response => HttpContext.Response;
    }
}