﻿using Modules.TenantIdentity.Features.Infrastructure.EFCore;
using Shared.Features.Messaging.Query;
using Shared.Features.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modules.TenantIdentity.Features.DomainFeatures.Users.Application.Queries
{
    public class GetUserById : Query<ApplicationUser>
    {
        public Guid UserId { get; set; }
    }
    public class GetUserByIdHandler : ServerExecutionBase<TenantIdentityModule>, IQueryHandler<GetUserById, ApplicationUser>
    {
        public GetUserByIdHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApplicationUser> HandleAsync(GetUserById query, CancellationToken cancellation)
        {
            return await module.TenantIdentityDbContext.GetUserByIdAsync(query.UserId);
        }
    }
}
