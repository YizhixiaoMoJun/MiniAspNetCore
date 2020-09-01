using System.Security.Claims;
namespace MiniAspNetCore.Authenication
{
    public class AuthenticationTicket
    {
        public AuthenticationTicket(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
        {
            this.AuthenticationScheme = authenticationScheme;
            this.Principal = principal;
            this.Properties = properties;
        }
        public AuthenticationTicket(ClaimsPrincipal principal, string authenticationScheme)
            : this(principal, properties: null, authenticationScheme: authenticationScheme)
        { }
        public string AuthenticationScheme { get; }

        public ClaimsPrincipal Principal { get; }

        public AuthenticationProperties Properties { get; }

        public AuthenticationTicket Clone()
        {
            var principal = new ClaimsPrincipal();
            foreach (var identity in Principal.Identities)
            {
                principal.AddIdentity(identity.Clone());
            }
            return new AuthenticationTicket(principal, Properties.Clone(), AuthenticationScheme);
        }
    }
}