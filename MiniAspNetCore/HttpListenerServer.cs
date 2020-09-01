using System.Net;
using System.Threading.Tasks;

namespace MiniAspNetCore
{
    public class HttpListenerServer : IServer
    {
        private readonly HttpListener listener;
        private readonly string listenerUrl;
        public HttpListenerServer(string listenerUrl)
        {
            this.listenerUrl = listenerUrl;
            listener = new HttpListener();

        }

        public async Task StartAsync(HttpRequestDelegate handler)
        {
            listener.Prefixes.Add(listenerUrl);
            listener.Start();
            while (true)
            {
                var listenerContext = await listener.GetContextAsync();
                var listerneFeature = new HttpListenerFeature(listenerContext);
                var featureCollection = new FeatureCollection();
                featureCollection.SetService<IRequestFeature>(listerneFeature);
                featureCollection.SetService<IResponseFeature>(listerneFeature);
                var httpContext = new HttpContext(featureCollection);
                await handler(httpContext);
                listenerContext.Response.Close();
            }
        }
    }
}