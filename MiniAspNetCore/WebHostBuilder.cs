using System;
using System.Collections.Generic;

namespace MiniAspNetCore
{
    public class WebHostBuilder : IWebHostBuilder
    {
        private IServer server;

        private List<Action<IApplicationBuilder>> configures = new List<Action<IApplicationBuilder>>();

        public IWebHost Build()
        {
            var applicationBuilder = new ApplicationBuilder();
            foreach (var configure in configures)
            {
                configure(applicationBuilder);
            }
            return new WebHost(server, applicationBuilder.Build());
        }

        public IWebHostBuilder Configure(Action<IApplicationBuilder> configure)
        {
            configures.Add(configure);
            return this;
        }

        public IWebHostBuilder UseServer(IServer server)
        {
            this.server = server;
            return this;
        }
    }
}