﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Identity.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string Base64Data { get; set; }
    }
}