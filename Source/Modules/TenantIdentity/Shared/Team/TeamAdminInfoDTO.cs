﻿using WebShared.Identity.Team.AdminManagement;

namespace Modules.IdentityModule.Shared
{
    public class TeamAdminInfoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public TeamMetricsDTO Metrics { get; set; }
        public List<MemberDTO> Members { get; set; }
        public List<AdminNotificationDTO> AdminNotifications { get; set; }
    }
}
