﻿using Microsoft.EntityFrameworkCore;
using Modules.TenantIdentity.Shared.DTOs.Tenant;
using Shared.Features.Messaging.Query;
using Shared.Features.Server;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modules.TenantIdentity.Features.DomainFeatures.Tenants.Application.Queries
{
    public class GetTenantDetailsByID : Query<TenantDetailDTO>
    {
        public Guid TenantId { get; set; }
    }
    public class GetTenantDetailsByIDQueryHandler : ServerExecutionBase<TenantIdentityModule>, IQueryHandler<GetTenantDetailsByID, TenantDetailDTO>
    {
        public GetTenantDetailsByIDQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public async Task<TenantDetailDTO> HandleAsync(GetTenantDetailsByID query, CancellationToken cancellation)
        {
            var tenantDetail = await module.TenantIdentityDbContext.Tenants.Where(t => t.TenantId == query.TenantId).SingleAsync();
            return tenantDetail.ToDetailDTO();
        }
    }
}
