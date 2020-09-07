using System;
using System.Security.Claims;

namespace MiniAspNetCore.Authenication
{
    public abstract class ResultContext<TOptions> : BaseContext<TOptions> where TOptions : AuthenticationSchemeOptions
    {
        private AuthenticationProperties _properties;
        protected ResultContext(HttpContext context, AuthenticationScheme scheme, TOptions options)
            : base(context, scheme, options) { }
        public ClaimsPrincipal Principal { get; set; }
        public AuthenticationProperties Properties
        {
            get
            {
                _properties ??= new AuthenticationProperties();
                return _properties;
            }
            set => _properties = value;
        }
        public AuthenticateResult Result { get; private set; } = default!;

        /// <summary>
        /// Calls success creating a ticket with the <see cref="Principal"/> and <see cref="Properties"/>.
        /// </summary>
       // public void Success() => Result = HandleRequestResult.Success(new AuthenticationTicket(Principal!, Properties, Scheme.Name));

        /// <summary>
        /// Indicates that there was no information returned for this authentication scheme.
        /// </summary>
        public void NoResult() => Result = AuthenticateResult.NoResult();

        /// <summary>
        /// Indicates that there was a failure during authentication.
        /// </summary>
        /// <param name="failure"></param>
        public void Fail(Exception failure) => Result = AuthenticateResult.Fail(failure);

        /// <summary>
        /// Indicates that there was a failure during authentication.
        /// </summary>
        /// <param name="failureMessage"></param>
        public void Fail(string failureMessage) => Result = AuthenticateResult.Fail(failureMessage);

    }
}