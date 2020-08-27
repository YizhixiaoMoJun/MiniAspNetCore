using System.Threading.Tasks;

namespace MiniAspNetCore
{
    public interface IServer
    {
         Task StartAsync(HttpRequestDelegate handler);
    }
}