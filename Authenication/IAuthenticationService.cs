using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResult> AuthenicateAsync(HttpContext context,string scheme);

        Task ChallengeAsync(HttpContext context,string scheme,AuthenticationProperties properties);

        Task ForbidAsync(HttpContext context,string scheme,AuthenticationProperties properties);

        Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties);

        Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties);
    }
}