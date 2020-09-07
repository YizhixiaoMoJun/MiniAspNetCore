using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationSignInHandler : IAuthenticationSignOutHandler
    {
        Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties);
    }
}