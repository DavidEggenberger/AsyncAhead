﻿using Domain.Aggregates.TenantAggregate.Enums;
using Shared.Modules.Layers.Domain;

namespace Modules.TenantIdentityModule.Domain
{
    public class TenantMembership : Entity
    {
        private TenantMembership() { }
        public TenantMembership(Guid userId, Role role)
        {
            UserId = userId;
            Role = role;
        }
        public Guid UserId { get; set; }
        public Tenant Tenant { get; set; }
        public Role Role { get; set; }
    }
}
