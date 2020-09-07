using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniAspNetCore.Authenication
{
    public interface IClaimsTransformation
    {
         Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal);
    }
}