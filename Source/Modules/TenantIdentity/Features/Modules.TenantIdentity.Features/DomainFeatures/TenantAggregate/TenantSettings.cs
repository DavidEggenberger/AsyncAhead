﻿using Shared.Features.Domain;
using Shared.Features.DomainKernel;

namespace Modules.TenantIdentity.Features.DomainFeatures.TenantAggregate
{
    public class TenantSettings : Entity
    {
        public string IconURI { get; set; }
    }
}
