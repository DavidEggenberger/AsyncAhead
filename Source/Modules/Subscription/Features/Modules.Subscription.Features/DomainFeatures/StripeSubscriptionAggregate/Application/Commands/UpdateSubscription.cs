﻿using Shared.Features.CQRS.Command;

namespace Modules.Subscriptions.Features.DomainFeatures.StripeSubscriptionAggregate.Application.Commands
{
    public class UpdateSubscription : ICommand
    {
        public Stripe.Subscription Subscription { get; set; }
    }
}
