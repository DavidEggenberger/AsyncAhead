﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.Features.CQRS.Command;
using Shared.Features.CQRS.Query;
using Shared.Kernel.BuildingBlocks;
using Shared.Kernel.BuildingBlocks.ModelValidation;
using SharedKernel.Interfaces;

namespace Shared.Features.Server
{
    public class BaseController : ControllerBase
    {
        protected readonly ICommandDispatcher commandDispatcher;
        protected readonly IQueryDispatcher queryDispatcher;
        protected readonly IExecutionContextAccessor executionContextAccessor;
        protected readonly IWebContextAccessor webContextAccessor;
        protected readonly IValidationService validationService;

        public BaseController(IServiceProvider serviceProvider)
        {
            commandDispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
            queryDispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();
            executionContextAccessor = serviceProvider.GetRequiredService<IExecutionContextAccessor>();
            webContextAccessor = serviceProvider.GetService<IWebContextAccessor>();
            validationService = serviceProvider.GetRequiredService<IValidationService>();
        }

        public async Task ActionResult()
        {
            
        }
    }
}
