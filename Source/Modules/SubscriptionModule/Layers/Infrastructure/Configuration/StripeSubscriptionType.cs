﻿using SharedKernel.DomainKernel.Tenant;

namespace Infrastructure.StripeIntegration.Configuration
{
    public class StripeSubscriptionType
    {
        public string StripePriceId { get; set; }
        public SubscriptionPlanType Type { get; set; }
        public int TrialPeriodDays { get; set; }
    }
}
