using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationSchemeProvider : IAuthenticationSchemeProvider
    {
        private readonly AuthenticationOptions _options;
        private readonly Dictionary<string, AuthenticationScheme> _schemes;
        private readonly List<AuthenticationScheme> _requestHandlers;

        private IEnumerable<AuthenticationScheme> _schemesCopy = Array.Empty<AuthenticationScheme>();

        private IEnumerable<AuthenticationScheme> _requestHandlersCopy = Array.Empty<AuthenticationScheme>();

        private readonly object _lock = new object();
        public AuthenticationSchemeProvider(IOptions<AuthenticationOptions> options)
            : this(options, new Dictionary<string, AuthenticationScheme>(StringComparer.Ordinal))
        {

        }

        public AuthenticationSchemeProvider(IOptions<AuthenticationOptions> options, Dictionary<string, AuthenticationScheme> schemes)
        {
            _options = options.Value;
            _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _requestHandlers = new List<AuthenticationScheme>();
            foreach (var builder in _options.Schemes)
            {
                var schem = builder.Build();
                AddScheme(schem);
            }
        }

        public virtual void AddScheme(AuthenticationScheme scheme)
        {
            if (_schemes.ContainsKey(scheme.Name))
            {
                throw new InvalidOperationException("Scheme already exists: " + scheme.Name);
            }
            lock (_lock)
            {
                if (!TryAddScheme(scheme))
                {
                    throw new InvalidOperationException("Scheme already exists: " + scheme.Name);
                }
            }
        }

        public virtual bool TryAddScheme(AuthenticationScheme scheme)
        {
            if (_schemes.ContainsKey(scheme.Name))
            {
                return false;
            }
            lock (_lock)
            {
                if (_schemes.ContainsKey(scheme.Name))
                {
                    return false;
                }
                if (typeof(IAuthenticationRequestHandler).IsAssignableFrom(scheme.HandlerType))
                {
                    _requestHandlers.Add(scheme);
                    _requestHandlersCopy = _requestHandlers.ToArray();
                }
                _schemes[scheme.Name] = scheme;
                _schemesCopy = _schemes.Values.ToArray();
                return true;
            }
        }

        public virtual Task<IEnumerable<AuthenticationScheme>> GetAllSchemeAsync()
            => Task.FromResult(_schemesCopy);

        public virtual Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync()
            => _options.DefaultAuthenticateScheme != null
            ? GetSchemeAsync(_options.DefaultAuthenticateScheme)
            : GetDefaultSchemeAsync();

        public virtual Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync()
            => _options.DefaultChallengeScheme != null
            ? GetSchemeAsync(_options.DefaultChallengeScheme)
            : GetDefaultSchemeAsync();

        public virtual Task<AuthenticationScheme> GetDefaultForbidSchemeAsync()
            => _options.DefaultForbidScheme != null
            ? GetSchemeAsync(_options.DefaultForbidScheme)
            : GetDefaultChallengeSchemeAsync();

        public virtual Task<AuthenticationScheme> GetDefaultSignInSchemeAsync()
            => _options.DefaultSignInScheme != null
            ? GetSchemeAsync(_options.DefaultSignInScheme)
            : GetDefaultSchemeAsync();

        private Task<AuthenticationScheme> GetDefaultSchemeAsync()
            => _options.DefaultScheme != null
            ? GetSchemeAsync(_options.DefaultScheme)
            : Task.FromResult<AuthenticationScheme>(null);

        public virtual Task<AuthenticationScheme> GetDefaultSignOutSchemeAsync()
            => _options.DefaultSignOutScheme != null
            ? GetSchemeAsync(_options.DefaultSignOutScheme)
            : GetDefaultSignInSchemeAsync();

        public virtual Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync()
            => Task.FromResult(_requestHandlersCopy);

        public virtual Task<AuthenticationScheme> GetSchemeAsync(string name)
            => Task.FromResult(_schemes.ContainsKey(name) ? _schemes[name] : null);

        public virtual void RemoveScheme(string name)
        {
            if (!_schemes.ContainsKey(name))
            {
                return;
            }
            lock (_lock)
            {
                if (_schemes.ContainsKey(name))
                {
                    var scheme = _schemes[name];
                    if (_requestHandlers.Remove(scheme))
                    {
                        _requestHandlersCopy = _requestHandlers.ToArray();
                    }
                    _schemes.Remove(name);
                    _schemesCopy = _schemes.Values.ToArray();
                }
            }
        }
    }
}