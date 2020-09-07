using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System;

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
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var target = ResolveTarget(Options.ForwardAuthenticate);
            if (target != null)
            {
                return await Context.AuthenticateAsync(target);
            }

            // Calling Authenticate more than once should always return the original value.
            var result = await HandleAuthenticateOnceAsync() ?? AuthenticateResult.NoResult();
            if (result.Failure == null)
            {
                var ticket = result.Ticket;
                if (ticket?.Principal != null)
                {
                    // Logger.AuthenticationSchemeAuthenticated(Scheme.Name);
                }
                else
                {
                    //  Logger.AuthenticationSchemeNotAuthenticated(Scheme.Name);
                }
            }
            else
            {
                // Logger.AuthenticationSchemeNotAuthenticatedWithFailure(Scheme.Name, result.Failure.Message);
            }
            return result;
        }
        protected virtual string ResolveTarget(string scheme)
        {
            var target = scheme ?? Options.ForwardDefaultSelector?.Invoke(Context) ?? Options.ForwardDefault;

            // Prevent self targetting
            return string.Equals(target, Scheme.Name, StringComparison.Ordinal)
                ? null
                : target;
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

        protected Task<AuthenticateResult> HandleAuthenticateOnceAsync()
        {
            if (_authenticateTask == null)
            {
                _authenticateTask = HandleAuthenticateAsync();
            }

            return _authenticateTask;
        }
        protected virtual Task<object> CreateEventsAsync() => Task.FromResult(new object());

        protected abstract Task<AuthenticateResult> HandleAuthenticateAsync();

        protected async Task<AuthenticateResult> HandleAuthenticateOnceSafeAsync()
        {
            try
            {
                return await HandleAuthenticateOnceAsync();
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
        protected virtual Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            return Task.CompletedTask;
        }
        protected virtual Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            return Task.CompletedTask;
        }


    }

}