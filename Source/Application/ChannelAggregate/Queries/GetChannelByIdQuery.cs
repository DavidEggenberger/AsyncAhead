﻿using Domain.Aggregates.ChannelAggregate;
using Infrastructure.CQRS.Query;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChannelAggregate.Queries
{
    public class GetChannelByIdQuery : IQuery<Channel> 
    {
        public Guid Id { get; set; }
    }
    public class GetChannelQueryHandler : IQueryHandler<GetChannelByIdQuery, Channel>
    {
        private readonly ApplicationDbContext applicationDbContext;
        public GetChannelQueryHandler(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Channel> HandleAsync(GetChannelByIdQuery query, CancellationToken cancellation)
        {
            return applicationDbContext.Channels.Single(c => c.Id == query.Id);
        }
    }
}