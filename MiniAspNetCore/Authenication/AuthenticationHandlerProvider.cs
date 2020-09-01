using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationHandlerProvider : IAuthenticationHandlerProvider
    {
        public AuthenticationHandlerProvider(IAuthenticationSchemeProvider schemes)
        {
            Schemes = schemes;
        }
        public IAuthenticationSchemeProvider Schemes { get; }

        private readonly Dictionary<string, IAuthenticationHandler> _handlerMap = new Dictionary<string, IAuthenticationHandler>(StringComparer.Ordinal);

        public async Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme)
        {
            if (_handlerMap.TryGetValue(authenticationScheme, out var value))
            {
                return value;
            }

            var scheme = await Schemes.GetSchemeAsync(authenticationScheme);
            if(scheme == null)
            {
                return null;
            }

            var handler = (context.RequestServices.GetService(scheme.HandlerType) ??
                ActivatorUtilities.CreateInstance(context.RequestServices, scheme.HandlerType)) 
                as IAuthenticationHandler;
            if(handler!=null)
            {
                await handler.InitializeAsync(scheme,context);
                _handlerMap[authenticationScheme] = handler;
            }

            return handler;
        }
    }
}