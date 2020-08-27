using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationSignOutHandler : IAuthenticationHandler
    {
        Task SignOutAsync(AuthenticationProperties properties);
    }
}