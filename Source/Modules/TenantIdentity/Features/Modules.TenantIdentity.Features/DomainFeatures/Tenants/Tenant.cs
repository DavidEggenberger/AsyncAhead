﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.TenantIdentity.Features.DomainFeatures.Tenants.Domain.Exceptions;
using Modules.TenantIdentity.Shared.DTOs.Tenant;
using Shared.Features.Domain;
using Shared.Features.Domain.Exceptions;
using Shared.Kernel.BuildingBlocks.Auth;
using Shared.Kernel.Errors;
using System.Collections.Generic;
using System;
using System.Linq;
using Shared.Kernel.DomainKernel;

namespace Modules.TenantIdentity.Features.DomainFeatures.Tenants.Domain
{
    public class Tenant : Entity
    {
        public Tenant() { }

        public string Name { get; set; }
        public SubscriptionPlanType SubscriptionPlanType { get; set; }
        public IReadOnlyCollection<TenantMembership> Memberships => memberships.AsReadOnly();
        private List<TenantMembership> memberships = new List<TenantMembership>();
        public IReadOnlyCollection<TenantInvitation> Invitations => invitations.AsReadOnly();
        private List<TenantInvitation> invitations = new List<TenantInvitation>();

        public static Tenant CreateTenantWithAdmin(string tenantName, Guid adminUserId)
        {
            return new Tenant
            {
                Name = tenantName,
                memberships = new List<TenantMembership>
                {
                    new TenantMembership(adminUserId, TenantRole.Admin)
                }
            };
        }

        public void AddUser(Guid userId, TenantRole role)
        {
            ThrowIfCallerIsNotInRole(TenantRole.Admin);

            TenantMembership tenantMembership;
            if ((tenantMembership = memberships.SingleOrDefault(m => m.UserId == userId)) is not null)
            {
                throw new DomainException("");
            }
            else
            {
                memberships.Add(new TenantMembership(userId, role));
            }
        }

        public void ChangeRoleOfTenantMember(Guid userId, TenantRole newRole)
        {
            ThrowIfCallerIsNotInRole(TenantRole.Admin);

            if (CheckIfMember(userId) is false)
            {
                throw new MemberNotFoundException();
            }

            TenantMembership tenantMembership = memberships.Single(m => m.UserId == userId);
            tenantMembership.Role = newRole;
        }

        public void RemoveUser(Guid userId)
        {
            ThrowIfCallerIsNotInRole(TenantRole.Admin);

            if (CheckIfMember(userId) is false)
            {
                throw new MemberNotFoundException();
            }

            memberships.Remove(memberships.Single(m => m.UserId == userId));
        }

        public void InviteUserToRole(string email, TenantRole role)
        {
            ThrowIfCallerIsNotInRole(TenantRole.Admin);

            if (invitations.Any(invitation => invitation.Email == email))
            {
                throw new DomainException("");
            }

            invitations.Add(new TenantInvitation { Email = email, Role = role });
        }

        public void DeleteTenantMembership(Guid membershipId)
        {
            ThrowIfCallerIsNotInRole(TenantRole.Admin);

            var tenantMembership = Memberships.SingleOrDefault(t => t.Id == membershipId);
            if (tenantMembership == null)
            {
                throw Errors.NotFound(nameof(TenantMembership), membershipId);
            }

            memberships.Remove(tenantMembership);
        }

        public bool CheckIfMember(Guid userId)
        {
            return memberships.Any(membership => membership.UserId == userId);
        }

        public void ThrowIfUserCantDeleteTenant()
        {
            ThrowIfCallerIsNotInRole(TenantRole.Admin);
        }

        public TenantDTO ToDTO() => new TenantDTO();
        public TenantDetailDTO ToDetailDTO() => new TenantDetailDTO();

    }

    public class TenantEFConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenant");
        }
    }
}
