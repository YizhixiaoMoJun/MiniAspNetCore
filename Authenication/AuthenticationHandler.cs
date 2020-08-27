using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

namespace MiniAspNetCore.Authenication
{
    public abstract class AuthenticationHandler<TOptions> : IAuthenticationHandler where TOptions : AuthenticationSchemeOptions
    {
        private Task<AuthenticateResult> _authenticateTask;

        public AuthenticationScheme Scheme { get; private set; } = default!;
        public TOptions Options { get; private set; } = default!;
        protected HttpContext Context { get; private set; } = default!;

        protected HttpRequest Request
        {
            get => Context.Request;
        }

        protected HttpResponse Response
        {
            get => Context.Response;
        }
        protected virtual object Events { get; set; }

        protected IOptionsMonitor<TOptions> OptionsMonitor { get; }
        protected ILogger Logger { get; }

        protected UrlEncoder UrlEncoder { get; }

        protected ISystemClock Clock { get; }

        protected virtual string ClaimsIssuer => Options.ClaimsIssuer ?? Scheme.Name;
        protected AuthenticationHandler(IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        {
            Logger = logger.CreateLogger(this.GetType().FullName);
            UrlEncoder = encoder;
            Clock = clock;
            OptionsMonitor = options;
        }
        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}