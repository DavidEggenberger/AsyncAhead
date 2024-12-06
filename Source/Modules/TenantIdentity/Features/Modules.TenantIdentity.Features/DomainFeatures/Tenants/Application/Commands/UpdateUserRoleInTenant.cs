﻿using Shared.Features.Messaging.Command;
using Shared.Features.Server;
using Shared.Kernel.BuildingBlocks.Auth;
using Shared.Kernel.DomainKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modules.TenantIdentity.Features.DomainFeatures.Tenants.Application.Commands
{
    public class UpdateTenantMembership : Command
    {
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public TenantRole Role { get; set; }
    }
    public class UpdateTenantMembershipCommandHandler : ServerExecutionBase<TenantIdentityModule>, ICommandHandler<UpdateTenantMembership>
    {
        public UpdateTenantMembershipCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public async Task HandleAsync(UpdateTenantMembership command, CancellationToken cancellationToken)
        {
            var tenant = await module.TenantIdentityDbContext.GetTenantExtendedByIdAsync(command.TenantId);

            tenant.ChangeRoleOfTenantMember(command.UserId, command.Role);

            await module.TenantIdentityDbContext.SaveChangesAsync();
        }
    }
}
