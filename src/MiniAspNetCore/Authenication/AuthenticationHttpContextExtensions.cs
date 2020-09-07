using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MiniAspNetCore.Authenication
{
    public static class AuthenticationHttpContextExtensions
    {
        public static Task<AuthenticateResult> AuthenticateAsync(this HttpContext context) =>
           context.AuthenticateAsync(scheme: null);
        public static Task<AuthenticateResult> AuthenticateAsync(this HttpContext context, string scheme) =>
         context.RequestServices.GetRequiredService<IAuthenticationService>().AuthenticateAsync(context, scheme);
    }
}