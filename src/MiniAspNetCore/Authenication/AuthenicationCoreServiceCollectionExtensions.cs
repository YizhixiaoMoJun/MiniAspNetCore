using System;
using Microsoft.Extensions.DependencyInjection;
namespace MiniAspNetCore.Authenication
{
    public static class AuthenicationCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
            services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
            return services;
        }
        public static IServiceCollection AddAuthenticationCore(this IServiceCollection services, Action<AuthenticationOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddAuthenticationCore();
            services.Configure(configureOptions);
            return services;
        }
    }
}