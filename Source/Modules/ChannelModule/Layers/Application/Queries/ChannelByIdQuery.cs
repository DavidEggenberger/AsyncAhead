﻿using Modules.ChannelModule.Domain;
using Shared.Modules.Layers.Application.CQRS.Query;
using Shared.Modules.Layers.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Domain;
using Shared.Modules.Layers.Infrastructure.CQRS.Query;
using Modules.ChannelModule.Infrastructure.EFCore;

namespace Modules.ChannelModule.Layers.Application.Queries
{
    public class ChannelByIdQuery : IQuery<Channel> 
    {
        public Guid Id { get; set; }
    }
    public class GetChannelQueryHandler : BaseQueryHandler<ChannelDbContext, Channel>, IQueryHandler<ChannelByIdQuery, Channel>
    {
        public GetChannelQueryHandler(ChannelDbContext applicationDbContext) : base(applicationDbContext) { }
        public Task<Channel> HandleAsync(ChannelByIdQuery query, CancellationToken cancellation)
        {
            return dbSet.SingleAsync(c => c.Id == query.Id);
        }
    }
}
