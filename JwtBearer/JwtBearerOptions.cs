using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using MiniAspNetCore.Authenication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace MiniAspNetCore.JwtBearer
{
    public class JwtBearerOptions : AuthenticationSchemeOptions
    {
        private JwtSecurityTokenHandler _defaultHandler = new JwtSecurityTokenHandler();
        public JwtBearerOptions()
        {
            SecurityTokenValidators = new List<ISecurityTokenValidator> { _defaultHandler };
        }
        public bool RequireHttpsMetadata { get; set; } = true;
        public string MetadataAddress { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string Challenge { get; set; } = JwtBearerDefaults.AuthenticationScheme;
        public new JwtBearerEvents Events
        {
            get { return (JwtBearerEvents)base.Events; }
            set { base.Events = value; }
        }

        /// <summary>
        /// Gets or sets the timeout when using the backchannel to make an http call.
        /// </summary>
        public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromMinutes(1);

         public IConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager { get; set; }

        public bool RefreshOnIssuerKeyNotFound { get; set; } = true;

        /// <summary>
        /// Gets the ordered list of <see cref="ISecurityTokenValidator"/> used to validate access tokens.
        /// </summary>
        public IList<ISecurityTokenValidator> SecurityTokenValidators { get; private set; }

        /// <summary>
        /// Gets or sets the parameters used to validate identity tokens.
        /// </summary>
        /// <remarks>Contains the types and definitions required for validating a token.</remarks>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

        /// <summary>
        /// Defines whether the bearer token should be stored in the
        /// <see cref="AuthenticationProperties"/> after a successful authorization.
        /// </summary>
        public bool SaveToken { get; set; } = true;

        /// <summary>
        /// Defines whether the token validation errors should be returned to the caller.
        /// Enabled by default, this option can be disabled to prevent the JWT handler
        /// from returning an error and an error_description in the WWW-Authenticate header.
        /// </summary>
        public bool IncludeErrorDetails { get; set; } = true;

        /// <summary>
        /// Gets or sets the <see cref="MapInboundClaims"/> property on the default instance of <see cref="JwtSecurityTokenHandler"/> in SecurityTokenValidators, which is used when determining 
        /// whether or not to map claim types that are extracted when validating a <see cref="JwtSecurityToken"/>. 
        /// <para>If this is set to true, the Claim Type is set to the JSON claim 'name' after translating using this mapping. Otherwise, no mapping occurs.</para>
        /// <para>The default value is true.</para>
        /// </summary>
        public bool MapInboundClaims
        {
            get => _defaultHandler.MapInboundClaims;
            set => _defaultHandler.MapInboundClaims = value;
        }

    }
}