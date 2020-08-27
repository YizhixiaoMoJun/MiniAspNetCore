using System;
using System.Collections.Generic;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationOptions
    {
        private readonly IList<AuthenticationSchemeBuilder> _schemes = new List<AuthenticationSchemeBuilder>();

        public IEnumerable<AuthenticationSchemeBuilder> Schemes => _schemes;
        public IDictionary<string, AuthenticationSchemeBuilder> SchemeMap { get; } = new Dictionary<string, AuthenticationSchemeBuilder>(StringComparer.Ordinal);
        public void AddScheme(string name, Action<AuthenticationSchemeBuilder> configureBuilder)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (configureBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureBuilder));
            }
            if (SchemeMap.ContainsKey(name))
            {
                throw new InvalidOperationException("Scheme already exists: " + name);
            }

            var builder = new AuthenticationSchemeBuilder(name);
            configureBuilder(builder);
            _schemes.Add(builder);
            SchemeMap[name] = builder;
        }
        public void AddScheme<THandler>(string name, string displayName) where THandler : IAuthenticationHandler
            => AddScheme(name, b =>
            {
                b.DisplayName = displayName;
                b.HandlerType = typeof(THandler);
            });
        public string DefaultScheme { get; set; }

        /// <summary>
        /// Used as the default scheme by <see cref="IAuthenticationService.AuthenticateAsync(HttpContext, string)"/>.
        /// </summary>
        public string DefaultAuthenticateScheme { get; set; }

        /// <summary>
        /// Used as the default scheme by <see cref="IAuthenticationService.SignInAsync(HttpContext, string, System.Security.Claims.ClaimsPrincipal, AuthenticationProperties)"/>.
        /// </summary>
        public string DefaultSignInScheme { get; set; }

        /// <summary>
        /// Used as the default scheme by <see cref="IAuthenticationService.SignOutAsync(HttpContext, string, AuthenticationProperties)"/>.
        /// </summary>
        public string DefaultSignOutScheme { get; set; }

        /// <summary>
        /// Used as the default scheme by <see cref="IAuthenticationService.ChallengeAsync(HttpContext, string, AuthenticationProperties)"/>.
        /// </summary>
        public string DefaultChallengeScheme { get; set; }

        /// <summary>
        /// Used as the default scheme by <see cref="IAuthenticationService.ForbidAsync(HttpContext, string, AuthenticationProperties)"/>.
        /// </summary>
        public string DefaultForbidScheme { get; set; }

        /// <summary>
        /// If true, SignIn should throw if attempted with a ClaimsPrincipal.Identity.IsAuthenticated = false.
        /// </summary>
        public bool RequireAuthenticatedSignIn { get; set; } = true;
    }
}