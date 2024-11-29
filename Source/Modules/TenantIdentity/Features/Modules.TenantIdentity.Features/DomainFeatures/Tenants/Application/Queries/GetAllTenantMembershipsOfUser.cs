﻿using Microsoft.EntityFrameworkCore;
using Modules.TenantIdentity.Shared.DTOs.Tenant;
using Shared.Features.Messaging.Query;
using Shared.Features.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modules.TenantIdentity.Features.DomainFeatures.Tenants.Application.Queries
{
    public class GetAllTenantMembershipsOfUser : Query<List<TenantMembershipDTO>>
    {
        public Guid UserId { get; set; }
    }
    public class GetAllTenantMembershipsOfUserQueryHandler : ServerExecutionBase<TenantIdentityModule>, IQueryHandler<GetAllTenantMembershipsOfUser, List<TenantMembershipDTO>>
    {
        public GetAllTenantMembershipsOfUserQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider) { } 

        public async Task<List<TenantMembershipDTO>> HandleAsync(GetAllTenantMembershipsOfUser query, CancellationToken cancellation)
        {
            var tenantMemberships = await module.TenantIdentityDbContext.TenantMeberships.Where(tm => tm.UserId == query.UserId).ToListAsync();
            return tenantMemberships.Select(tm => tm.ToDTO()).ToList();
        }
    }
}
