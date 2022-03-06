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
                throw new IdentityOperationException("No user is found for the provided HttpContext");
            }
            await identificationDbContext.Entry(user).Collection(u => u.Memberships).LoadAsync();
            return user;
        }
        public async Task<ApplicationUser> FindByStripeCustomerId(string stripeCustomerId)
        {
            ApplicationUser user;
            if((user = identificationDbContext.Users.SingleOrDefault(u => u.StripeCustomerId == stripeCustomerId)) != null)
            {
                return user;
            }
            throw new IdentityOperationException("No user is found for the provided HttpContext");
        }
        public async Task<Guid> GetSelectedTeamId(ApplicationUser applicationUser)
        {
            await identificationDbContext.Entry(applicationUser).Collection(x => x.Memberships).Query().Include(x => x.Team).LoadAsync();
            try
            {
                return applicationUser.Memberships.Single(x => x.Status == UserSelectionStatus.Selected).Team.Id;
            }
            catch (Exception ex)
            {
                applicationUser.Memberships.Add(new ApplicationUserTeam
                {
                    Role = TeamRole.Admin,
                    Status = UserSelectionStatus.Selected,
                    Team = new Team
                    {
                        NameIdentitifer = "Your Team",
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
            throw new IdentityOperationException("");
        }
        public async Task UnSelectAllTeams(ApplicationUser applicationUser)
        {
            await identificationDbContext.Entry(applicationUser).Collection(x => x.Memberships).LoadAsync();
            applicationUser.Memberships?.ToList().ForEach(x => x.Status = UserSelectionStatus.NotSelected);
            await identificationDbContext.SaveChangesAsync();
        }
        public async Task SelectTeamForUser(ApplicationUser applicationUser, Team team)
        {
            applicationUser.Memberships.ToList().ForEach(x => x.Status = UserSelectionStatus.NotSelected);
            applicationUser.Memberships.Where(x => x.TeamId == team.Id).First().Status = UserSelectionStatus.Selected;
            await identificationDbContext.SaveChangesAsync();
        }
        public async Task<List<ApplicationUserTeam>> GetAllTeamMemberships(ApplicationUser applicationUser)
        {
            return identificationDbContext.ApplicationUserTeams.Include(x => x.Team).Where(x => x.UserId == applicationUser.Id).ToList();
        }
        public async Task<List<Team>> GetTeamsWhereApplicationUserIsMember(ApplicationUser applicationUser)
        {
            throw new Exception();
        }
        public async Task<List<Team>> GetTeamsWhereApplicationUserIsAdmin(ApplicationUser applicationUser)
        {
            throw new Exception();
        }
        public async Task<IdentityOperationResult<List<Claim>>> GetMembershipClaimsForApplicationUser(ApplicationUser applicationUser)
        {
            ApplicationUser _applicationUser = await identificationDbContext.Users.Include(x => x.Memberships).FirstAsync(x => x.Id == applicationUser.Id);
            ApplicationUserTeam applicationUserTeam = _applicationUser.Memberships.Where(x => x.Status == UserSelectionStatus.Selected).FirstOrDefault();
            if(applicationUserTeam == null)
            {
                return IdentityOperationResult<List<Claim>>.Fail("");
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(IdentityStringConstants.IdentityTeamIdClaimType, applicationUserTeam.TeamId.ToString()),
                new Claim(IdentityStringConstants.IdentityTeamRoleClaimType, applicationUserTeam.Role.ToString())
            };
            return IdentityOperationResult<List<Claim>>.Success(claims);
        }
    }
}
