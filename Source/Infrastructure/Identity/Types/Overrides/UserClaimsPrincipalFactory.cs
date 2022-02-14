﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Types.Overrides
{
    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>
    {
        public async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            return new ClaimsPrincipal();
        }
    }
}