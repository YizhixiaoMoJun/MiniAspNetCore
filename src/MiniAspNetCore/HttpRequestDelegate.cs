using System.Threading.Tasks;

namespace MiniAspNetCore
{
    public delegate Task HttpRequestDelegate (HttpContext context);
}