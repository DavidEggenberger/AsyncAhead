﻿using Modules.TenantIdentity.Features.Infrastructure.EFCore;
using Shared.Features.Messaging.Command;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modules.TenantIdentity.Features.DomainFeatures.Users.Application.Commands
{
    public class SetSelectedTenantForUser : ICommand
    {
        public Guid SelectedTenantId { get; set; }
        public Guid UserId { get; set; }
    }
    public class SetSelectedTenantForUserHandler : ICommandHandler<SetSelectedTenantForUser>
    {
        private readonly TenantIdentityDbContext tenantIdentityDbContext;
        public SetSelectedTenantForUserHandler(TenantIdentityDbContext tenantIdentityDbContext)
        {
            this.tenantIdentityDbContext = tenantIdentityDbContext;
        }

        public async Task HandleAsync(SetSelectedTenantForUser command, CancellationToken cancellationToken)
        {
            var user = await tenantIdentityDbContext.GetUserByIdAsync(command.UserId);

            if (user.SelectedTenantId == command.SelectedTenantId)
            {
                return;
            }

            user.SelectedTenantId = command.SelectedTenantId;

            await tenantIdentityDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
