using System.Threading.Tasks;

namespace MiniAspNetCore
{
    public class WebHost : IWebHost
    {
        private readonly IServer server;

        private readonly HttpRequestDelegate requestDelegate;

        public WebHost(IServer server, HttpRequestDelegate requestDelegate)
        {
            this.server = server;
            this.requestDelegate = requestDelegate;
        }

        public async Task StartAsync()
        {
             await server.StartAsync(requestDelegate);
        }
    }
}