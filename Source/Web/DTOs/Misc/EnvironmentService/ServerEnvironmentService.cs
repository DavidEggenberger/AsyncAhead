﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EnvironmentService
{
    public class ServerEnvironmentService : IEnvironmentService
    {
        public HostingEnvironment GetEnvironment()
        {
            return HostingEnvironment.Server;
        }
    }
}