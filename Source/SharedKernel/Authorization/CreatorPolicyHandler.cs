﻿using Microsoft.AspNetCore.Authorization;
using Common.Kernel;
using Common.Exstensions;

namespace WebServer.Authorization
{
    public class CreatorPolicyHandler : AuthorizationHandler<CreatorPolicyRequirement, IAuditable>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatorPolicyRequirement requirement, IAuditable resource)
        {
            if(context.User.GetUserId<Guid>() == resource.CreatedByUserId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
    public class CreatorPolicyRequirement : IAuthorizationRequirement
    {

    }
}