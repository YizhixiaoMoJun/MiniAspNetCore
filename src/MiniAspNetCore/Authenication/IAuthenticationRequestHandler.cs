using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IAuthenticationRequestHandler :IAuthenticationHandler
    {
        Task<bool> HandleRequestAsync();       
    }
}