﻿using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebServer.Services
{
    public class TeamResolver : ITeamResolver
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TeamManager teamManager;
        public TeamResolver(IHttpContextAccessor httpContextAccessor, TeamManager teamManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.teamManager = teamManager;
        }

        public Task<Team> ResolveTeamAsync()
        {
            return teamManager.FindByClaimsPrincipalAsync(httpContextAccessor.HttpContext.User);
        }

        public Guid ResolveTeamId()
        {
            try
            {
                return new Guid(httpContextAccessor.HttpContext.User.FindFirst("TeamId").Value);
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }
    }
}