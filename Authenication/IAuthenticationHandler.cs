using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationHandler
    {
         Task InitializeAsync(AuthenticationScheme scheme, HttpContext context);

         Task<AuthenticateResult> AuthenticateAsync();

         Task ChallengeAsync(AuthenticationProperties properties);

         Task ForbidAsync(AuthenticationProperties properties);
    }
}