using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationService : IAuthenticationService
    {
        public HashSet<ClaimsPrincipal> _transformCache;

        public AuthenticationService(IAuthenticationSchemeProvider schemes, IAuthenticationHandlerProvider handlers, IClaimsTransformation transform, IOptions<AuthenticationOptions> options)
        {
            Schemes = schemes;
            Handlers = handlers;
            Transform = transform;
            Options = options.Value;
        }

        public IAuthenticationSchemeProvider Schemes { get; }

        public IAuthenticationHandlerProvider Handlers { get; }

        public IClaimsTransformation Transform { get; }

        public AuthenticationOptions Options { get; }

        public virtual async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            if (scheme == null)
            {
                var defaultschem = await Schemes.GetDefaultAuthenticateSchemeAsync();
                scheme = defaultschem?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            var result = (await handler.AuthenticateAsync()) ?? AuthenticateResult.NoResult();

            if (result.Succeeded)
            {
                var principal = result.Principal!;
                var dotransform = true;
                _transformCache ??= new HashSet<ClaimsPrincipal>();
                if (_transformCache.Contains(principal))
                {
                    dotransform = false;
                }

                if (dotransform)
                {
                    principal = await Transform.TransformAsync(principal);
                    _transformCache.Add(principal);
                }
                return AuthenticateResult.Success(new AuthenticationTicket(principal, result.Properties, result.Ticket!.AuthenticationScheme));
            }
            return result;
        }

        private async Task<Exception> CreateMissingHandlerException(string scheme)
        {
            var schemes = string.Join(", ", (await Schemes.GetAllSchemeAsync()).Select(sch => sch.Name));

            var footer = $" Did you forget to call AddAuthentication().Add[SomeAuthHandler](\"{scheme}\",...)?";

            if (string.IsNullOrEmpty(schemes))
            {
                return new InvalidOperationException(
                    $"No authentication handlers are registered." + footer);
            }

            return new InvalidOperationException(
                $"No authentication handler is registered for the scheme '{scheme}'. The registered schemes are: {schemes}." + footer);
        }

        public virtual async Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            if (scheme == null)
            {
                var defaultChallengeScheme = await Schemes.GetDefaultChallengeSchemeAsync();
                scheme = defaultChallengeScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultChallengeScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            await handler.ChallengeAsync(properties);
        }

        public async Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            if (scheme == null)
            {
                var defaultForbidScheme = await Schemes.GetDefaultForbidSchemeAsync();
                scheme = defaultForbidScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultForbidScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            await handler.ForbidAsync(properties);
        }

        public virtual async Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            if (Options.RequireAuthenticatedSignIn)
            {
                if (principal.Identity == null)
                {
                    throw new InvalidOperationException("SignInAsync when principal.Identity == null is not allowed when AuthenticationOptions.RequireAuthenticatedSignIn is true.");
                }
                if (!principal.Identity.IsAuthenticated)
                {
                    throw new InvalidOperationException("SignInAsync when principal.Identity.IsAuthenticated is false is not allowed when AuthenticationOptions.RequireAuthenticatedSignIn is true.");
                }
            }

            if (scheme == null)
            {
                var defaultschem = await Schemes.GetDefaultSignInSchemeAsync();
                scheme = defaultschem?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultSignInScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingSignInHandlerException(scheme);
            }

            var signInHandler = handler as IAuthenticationSignInHandler;
            if (signInHandler == null)
            {
                throw await CreateMismatchedSignInHandlerException(scheme, handler);
            }

            await signInHandler.SignInAsync(principal, properties);
        }

        public async Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            if (scheme == null)
            {
                var defaultScheme = await Schemes.GetDefaultSignOutSchemeAsync();
                scheme = defaultScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultSignOutScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingSignOutHandlerException(scheme);
            }

            var signOutHandler = handler as IAuthenticationSignOutHandler;
            if (signOutHandler == null)
            {
                throw await CreateMismatchedSignOutHandlerException(scheme, handler);
            }

            await signOutHandler.SignOutAsync(properties);
        }
        private async Task<Exception> CreateMismatchedSignInHandlerException(string scheme, IAuthenticationHandler handler)
        {
            var schemes = await GetAllSignInSchemeNames();

            var mismatchError = $"The authentication handler registered for scheme '{scheme}' is '{handler.GetType().Name}' which cannot be used for SignInAsync. ";

            if (string.IsNullOrEmpty(schemes))
            {
                // CookieAuth is the only implementation of sign-in.
                return new InvalidOperationException(mismatchError
                    + $"Did you forget to call AddAuthentication().AddCookie(\"Cookies\") and SignInAsync(\"Cookies\",...)?");
            }

            return new InvalidOperationException(mismatchError + $"The registered sign-in schemes are: {schemes}.");
        }
        private async Task<Exception> CreateMissingSignInHandlerException(string scheme)
        {
            var schemes = await GetAllSignInSchemeNames();

            // CookieAuth is the only implementation of sign-in.
            var footer = $" Did you forget to call AddAuthentication().AddCookie(\"{scheme}\",...)?";

            if (string.IsNullOrEmpty(schemes))
            {
                return new InvalidOperationException(
                    $"No sign-in authentication handlers are registered." + footer);
            }

            return new InvalidOperationException(
                $"No sign-in authentication handler is registered for the scheme '{scheme}'. The registered sign-in schemes are: {schemes}." + footer);
        }
        private async Task<string> GetAllSignInSchemeNames()
        {
            return string.Join(", ", (await Schemes.GetAllSchemeAsync())
                .Where(sch => typeof(IAuthenticationSignInHandler).IsAssignableFrom(sch.HandlerType))
                .Select(sch => sch.Name));
        }
        private async Task<string> GetAllSignOutSchemeNames()
        {
            return string.Join(", ", (await Schemes.GetAllSchemeAsync())
                .Where(sch => typeof(IAuthenticationSignOutHandler).IsAssignableFrom(sch.HandlerType))
                .Select(sch => sch.Name));
        }

        private async Task<Exception> CreateMissingSignOutHandlerException(string scheme)
        {
            var schemes = await GetAllSignOutSchemeNames();

            var footer = $" Did you forget to call AddAuthentication().AddCookie(\"{scheme}\",...)?";

            if (string.IsNullOrEmpty(schemes))
            {
                // CookieAuth is the most common implementation of sign-out, but OpenIdConnect and WsFederation also support it.
                return new InvalidOperationException($"No sign-out authentication handlers are registered." + footer);
            }

            return new InvalidOperationException(
                $"No sign-out authentication handler is registered for the scheme '{scheme}'. The registered sign-out schemes are: {schemes}." + footer);
        }
        private async Task<Exception> CreateMismatchedSignOutHandlerException(string scheme, IAuthenticationHandler handler)
        {
            var schemes = await GetAllSignOutSchemeNames();

            var mismatchError = $"The authentication handler registered for scheme '{scheme}' is '{handler.GetType().Name}' which cannot be used for {nameof(SignOutAsync)}. ";

            if (string.IsNullOrEmpty(schemes))
            {
                // CookieAuth is the most common implementation of sign-out, but OpenIdConnect and WsFederation also support it.
                return new InvalidOperationException(mismatchError
                    + $"Did you forget to call AddAuthentication().AddCookie(\"Cookies\") and {nameof(SignOutAsync)}(\"Cookies\",...)?");
            }

            return new InvalidOperationException(mismatchError + $"The registered sign-out schemes are: {schemes}.");
        }
    }
}