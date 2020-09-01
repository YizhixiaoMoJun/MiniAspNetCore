using System;

namespace MiniAspNetCore.Authenication
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}