﻿using Microsoft.AspNetCore.SignalR;
using Shared.SharedKernel.Exstensions;

namespace Shared.Modules.Layers.Infrastructure.SignalR
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User.GetUserId<string>();
        }
    }
}