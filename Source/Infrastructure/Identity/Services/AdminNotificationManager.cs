﻿using Identity.Interfaces;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class AdminNotificationManager
    {
        private readonly IdentificationDbContext identificationDbContext;
        private readonly IIdentityUINotifierService identityUINotifierService;
        public AdminNotificationManager(IdentificationDbContext identificationDbContext, IIdentityUINotifierService identityUINotifierService)
        {
            this.identityUINotifierService = identityUINotifierService;
            this.identificationDbContext = identificationDbContext;
        }

        public async Task CreateNotification(Team team, AdminNotificationType notificationType, ApplicationUser creator, string message)
        {
            AdminNotification notification = new AdminNotification()
            {
                Team = team,
                Creator = creator,
                Type = notificationType,
                Message = message,
                CreatedAt = DateTime.Now
            };
            identificationDbContext.AdminNotifications.Add(notification);
            await identityUINotifierService.NotifyAdminMembersAboutNewNotification(team.Id);
        }
    }
}