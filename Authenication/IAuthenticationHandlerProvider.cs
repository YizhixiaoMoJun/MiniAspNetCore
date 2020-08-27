using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationHandlerProvider
    {
         Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme);
    }
}