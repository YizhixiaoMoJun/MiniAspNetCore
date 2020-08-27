using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
namespace MiniAspNetCore.Authenication
{
    public class AuthenticateResult
    {
        protected AuthenticateResult() { }

        //[MemberNotNullWhen(true, nameof(Ticket))]
        public bool Succeeded => Ticket != null;

        public AuthenticationTicket Ticket { get; protected set; }

        public ClaimsPrincipal Principal => Ticket?.Principal;

        public AuthenticationProperties Properties { get; protected set; }

        public Exception Failure { get; protected set; }

        public bool None { get; protected set; }

        public AuthenticateResult Clone()
        {
            if (None)
            {
                return NoResult();
            }
            if (Failure != null)
            {
                return Fail(Failure, Properties?.Clone());
            }
            if (Succeeded)
            {
                return Success(Ticket!.Clone());
            }
            // This shouldn't happen
            throw new NotImplementedException();
        }

        public static AuthenticateResult Success(AuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }
            return new AuthenticateResult() { Ticket = ticket, Properties = ticket.Properties };
        }

        public static AuthenticateResult NoResult()
            => new AuthenticateResult() { None = true };

        public static AuthenticateResult Fail(Exception failure)
            => new AuthenticateResult() { Failure = failure };

        public static AuthenticateResult Fail(Exception failure, AuthenticationProperties properties)
            =>new AuthenticateResult() {Failure = failure,Properties = properties};

        public static AuthenticateResult Fail(string failureMessage)
            => Fail(new Exception(failureMessage));
        
        public static AuthenticateResult Fail(string failureMessage, AuthenticationProperties properties)
            => Fail(new Exception(failureMessage), properties);

    }
}