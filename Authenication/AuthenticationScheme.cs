using System;

namespace MiniAspNetCore.Authenication
{
    public class AuthenticationScheme
    {
        public AuthenticationScheme(string name, string displayName, Type handlerType)
        {
            Name = name;
            DisplayName = displayName;
            HandlerType = handlerType;
        }

        public string Name{get;}

        public string DisplayName{get;}
       
        public Type HandlerType{get;}
    }
}