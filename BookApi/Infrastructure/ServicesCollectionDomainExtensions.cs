using Microsoft.Extensions.DependencyInjection;
using Shirley.Book.Service.AuthServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Infrastructure
{
    /// <summary>
    /// domain services extensions
    /// </summary>
    public static class ServicesCollectionDomainExtensions
    {
        /// <summary>
        /// register domain services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthServices, AuthServices>();

            services.AddScoped<HttpGlobalExceptionHandler>();
            return services;
        }
    }
}
