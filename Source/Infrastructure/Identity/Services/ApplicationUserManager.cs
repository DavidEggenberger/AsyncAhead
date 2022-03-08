﻿using Infrastructure.Identity;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Types;
using Infrastructure.Identity.Types.Constants;
using Infrastructure.Identity.Types.Enums;
using Infrastructure.Identity.Types.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private IdentificationDbContext identificationDbContext;
        private SubscriptionPlanManager subscriptionPlanManager;
        public ApplicationUserManager(SubscriptionPlanManager subscriptionPlanManager, IdentificationDbContext identificationDbContext, IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<ApplicationUserManager> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.identificationDbContext = identificationDbContext;
            this.subscriptionPlanManager = subscriptionPlanManager;
        }

        public async Task<ApplicationUser> FindUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            ApplicationUser user = await base.GetUserAsync(claimsPrincipal);
            if(user == null)
            {
                throw new IdentityOperationException();
            }
            await LoadApplicationUserAsync(user);
            return user;
        }
        public Task<ApplicationUser> FindByIdAsync(Guid id)
        {
            return FindByIdAsync(id.ToString());
        }
        public async Task<ApplicationUser> FindUserByStripeCustomerId(string stripeCustomerId)
        {
            ApplicationUser applicationUser;
            try
            {
                applicationUser = await identificationDbContext.Users.SingleAsync(u => u.StripeCustomerId == stripeCustomerId);
                await LoadApplicationUserAsync(applicationUser);
                return applicationUser;
            }
            catch(Exception ex)
            {
                throw new IdentityOperationException();
            }
        }
        public async Task<Guid> GetSelectedTeamId(ApplicationUser applicationUser)
        {
            try
            {
                return applicationUser.Memberships.Single(x => x.SelectionStatus == UserSelectionStatus.Selected).Team.Id;
            }
            catch (Exception ex)
            {
                applicationUser.Memberships.Add(new ApplicationUserTeam
                {
                    Role = TeamRole.Admin,
                    SelectionStatus = UserSelectionStatus.Selected,
                    Team = new Team
                    {
                        Name = "Your Team",
                        Subscription = new Subscription
                        {
                            SubscriptionPlan = await subscriptionPlanManager.FindByPlanType(SubscriptionPlanType.Free),
                            Status = SubscriptionStatus.Active
                        }
                    }
                });
                await identificationDbContext.SaveChangesAsync();
                return await GetSelectedTeamId(applicationUser);
            }
            throw new IdentityOperationException();
        }
        public async Task SelectTeamForUser(ApplicationUser applicationUser, Team team)
        {
            applicationUser.Memberships.ForEach(x => x.SelectionStatus = UserSelectionStatus.NotSelected);
            applicationUser.Memberships.Single(m => m.TeamId == team.Id).SelectionStatus = UserSelectionStatus.Selected;
            await identificationDbContext.SaveChangesAsync();
        }
        public List<ApplicationUserTeam> GetAllTeamMemberships(ApplicationUser applicationUser)
        {
            return applicationUser.Memberships.Where(x => x.UserId == applicationUser.Id).ToList();
        }
        public async Task<List<Claim>> GetMembershipClaimsForApplicationUser(ApplicationUser applicationUser)
        {
            ApplicationUserTeam applicationUserTeam;
            try
            {
                applicationUserTeam = applicationUser.Memberships.Single(x => x.SelectionStatus == UserSelectionStatus.Selected);
            }
            catch (Exception ex)
            {
                throw new IdentityOperationException();
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim("TeamSubscriptionPlanType", applicationUserTeam.Team.Subscription.SubscriptionPlan.PlanType.ToString()),
                new Claim("TeamName", applicationUserTeam.Team.Name),
                new Claim(IdentityStringConstants.IdentityTeamIdClaimType, applicationUserTeam.TeamId.ToString()),
                new Claim(IdentityStringConstants.IdentityTeamRoleClaimType, applicationUserTeam.Role.ToString())
            };
            return claims;
        }
        private async Task LoadApplicationUserAsync(ApplicationUser applicationUser)
        {
            await identificationDbContext.Entry(applicationUser).Collection(u => u.Memberships).Query()
                .Include(x => x.Team)
                .ThenInclude(x => x.Subscription)
                .ThenInclude(x => x.SubscriptionPlan).LoadAsync();
        }
    }
}
