﻿using WebShared.Identity.UserTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShared.Identity.Team
{
    public class MemberDTO
    {

        public MembershipStatusDTO Status { get; set; }
        public TeamRoleDTO Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
    }
}