using System;
using Microsoft.Extensions.DependencyInjection;

namespace MiniAspNetCore.Authenication
{
    public static class AuthenicationServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddAuthentication(this IServiceCollection services)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddAuthenticationCore();
            return new AuthenticationBuilder(services);
        }
    }
}