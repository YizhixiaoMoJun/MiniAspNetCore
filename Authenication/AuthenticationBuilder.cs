using System;
using Microsoft.Extensions.DependencyInjection;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationBuilder
    {
        public AuthenticationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public virtual IServiceCollection Services { get; }

        private AuthenticationBuilder AddSchemeHelper<TOptions, THandler>(string authenticationScheme, string displayName,Action<TOptions> configureOptions)
            where TOptions : AuthenticationSchemeOptions, new()
            where THandler : class, IAuthenticationHandler
        {
            Services.Configure<AuthenticationOptions>(o =>
            {
                o.AddScheme(authenticationScheme, scheme =>
                {
                    scheme.HandlerType = typeof(THandler);
                    scheme.DisplayName = displayName;
                });
            });
            if(configureOptions!=null)
            {
                Services.Configure(authenticationScheme,configureOptions);
            }
            Services.AddOptions<TOptions>(authenticationScheme).Validate(o=>{
                o.Validate(authenticationScheme);
                return true;
            });
            Services.AddTransient<THandler>();
            return this;
        }
        public virtual AuthenticationBuilder AddScheme<TOptions,THandler>(string authenticationScheme, string displayName, Action<TOptions> configureOptions)
            where TOptions : AuthenticationSchemeOptions, new()
            where THandler : AuthenticationHandler<TOptions>
            => AddSchemeHelper<TOptions, THandler>(authenticationScheme, displayName, configureOptions);
        
        public virtual AuthenticationBuilder AddScheme<TOptions, THandler>(string authenticationScheme, Action<TOptions> configureOptions)
            where TOptions : AuthenticationSchemeOptions, new()
            where THandler : AuthenticationHandler<TOptions>
            => AddScheme<TOptions, THandler>(authenticationScheme, displayName: null, configureOptions: configureOptions);
        
    }
}