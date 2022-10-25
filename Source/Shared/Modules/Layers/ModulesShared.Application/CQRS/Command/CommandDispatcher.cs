﻿using Microsoft.Extensions.DependencyInjection;
using Shared.Modules.Layers.Infrastructure.CQRS.Command;

namespace Shared.Modules.Layers.Application.CQRS.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellation = default) where TCommand : ICommand
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            return handler.HandleAsync(command, cancellation);
        }
        public Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellation = default) where TCommand : ICommand
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
            return handler.HandleAsync(command, cancellation);
        }
    }
}
