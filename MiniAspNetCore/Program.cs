using System;
using System.Text;
using System.Threading.Tasks;
using MiniAspNetCore.Authenication;

namespace MiniAspNetCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var webHostBuilder = new WebHostBuilder()
            .UseHttpListenerServer("http://localhost:5000/")
            .Configure(appBuilder =>
            {
                appBuilder.Use(m1);
                appBuilder.Use(m2);
                appBuilder.Use(Auth);
            });
            var webHost = webHostBuilder.Build();
            await webHost.StartAsync();
        }

        private static HttpRequestDelegate m1(HttpRequestDelegate next) =>
          async context =>
              {
                  await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Hello=>"));
                  await next(context);
              };

        private static HttpRequestDelegate m2(HttpRequestDelegate next) =>
            async context =>
                {

                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("word"));
                    await next(context);
                };

        private static HttpRequestDelegate Auth(HttpRequestDelegate next) =>
            async context =>
                {
                    var schemeProvider = (IAuthenticationSchemeProvider)context.RequestServices.GetService(typeof(IAuthenticationSchemeProvider));
                    var defaultAuthSchem = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
                    var authenticationServices = (IAuthenticationService)context.RequestServices.GetService(typeof(IAuthenticationService));
                    var authResult = await authenticationServices.AuthenicateAsync(context, defaultAuthSchem.Name);
                    if(authResult.Succeeded)
                    {
                        context.User = authResult.Principal;
                    }

                    await next(context);
                };


    }

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseHttpListenerServer(this IWebHostBuilder webHostBuilder, string listenerUrl)
        {
            var httpListenerServer = new HttpListenerServer(listenerUrl);
            webHostBuilder.UseServer(httpListenerServer);
            return webHostBuilder;
        }
    }
}
