﻿namespace Shared.Kernel
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
        Guid TenantId { get; set; }
    }
}
