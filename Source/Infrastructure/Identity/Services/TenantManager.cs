﻿using Infrastructure.Identity;
using Infrastructure.Identity.Types.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class TenantManager
    {
        private IdentificationDbContext identificationDbContext;
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        public TenantManager(IdentificationDbContext identificationDbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.identificationDbContext = identificationDbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IdentityOperationResult> InviteUserToRoleThroughEmailToTenant(Tenant tenant, TenantRoleType role, string Email)
        {
            throw new Exception();
        }
        public async Task<IdentityOperationResult> InviteUserThroughEmailToTenant(Tenant tenant, string Email)
        {
            throw new Exception();
        }
        public async Task<IdentityOperationResult> CreateNewTenantAsync(string name)
        {

            identificationDbContext.Tenants.Add(new Tenant
            {
                Name = name,
                
            });
            await identificationDbContext.SaveChangesAsync();
            return IdentityOperationResult.Success();
        }
        public async Task<IdentityOperationResult<List<ApplicationUser>>> GetAllMembersAsync(Tenant tenant)
        {
            Tenant _tenant = await identificationDbContext.Tenants.Include(x => x.Members).ThenInclude(x => x.User).FirstAsync(x => x.Id == tenant.Id);
            return IdentityOperationResult<List<ApplicationUser>>.Success(tenant.Members.Select(x => x.User).ToList());
        }
        public async Task<IdentityOperationResult<List<ApplicationUser>>> GetAllMembersByRoleAsync(Tenant tenant, TenantRoleType role)
        {
            Tenant _tenant = await identificationDbContext.Tenants.Include(x => x.Members).ThenInclude(x => x.User).FirstAsync(x => x.Id == tenant.Id);
            return IdentityOperationResult<List<ApplicationUser>>.Success(tenant.Members.Where(x => x.Role == role).Select(x => x.User).ToList());
        }
        public async Task<IdentityOperationResult> UpdateTenantName(Tenant tenant, string newName)
        {
            throw new Exception();
        }
        public async Task<bool> CheckIfNameIsValidForTenant(string name)
        {
            if(!identificationDbContext.Tenants.Any(x => x.Name == name))
            {
                return true;
            }
            return false;
        }
    }
}
