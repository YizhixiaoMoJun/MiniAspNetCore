using System;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationSchemeBuilder
    {
        public AuthenticationSchemeBuilder(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string DisplayName { get; set;}

        public Type HandlerType { get; set;}
        public AuthenticationScheme Build()
        {
            if (HandlerType is null)
            {
                throw new InvalidOperationException($"{nameof(HandlerType)} must be configured to build an {nameof(AuthenticationScheme)}.");
            }

            return new AuthenticationScheme(Name, DisplayName, HandlerType);
        }
    }
}