using System.Threading.Tasks;

namespace MiniAspNetCore
{
    public interface IWebHost
    {
        Task StartAsync();
    }
}