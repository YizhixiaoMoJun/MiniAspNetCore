using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationSchemeProvider
    {
        Task<IEnumerable<AuthenticationScheme>> GetAllSchemeAsync();
        Task<AuthenticationScheme> GetSchemeAsync(string name);
        Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync();
        Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync();
        Task<AuthenticationScheme> GetDefaultForbidSchemeAsync();
        Task<AuthenticationScheme> GetDefaultSignInSchemeAsync();
        Task<AuthenticationScheme> GetDefaultSignOutSchemeAsync();
        void AddScheme(AuthenticationScheme scheme);
        bool TryAddScheme(AuthenticationScheme scheme)
        {
            try
            {
                AddScheme(scheme);
                return true;
            }
            catch
            {
                return false;
            }
        }
        void RemoveScheme(string name);
        Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync();
    }
}