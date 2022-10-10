﻿using Infrastructure.Identity;
using Infrastructure.StripeIntegration.Configuration;

namespace Infrastructure.StripeIntegration.Services.Interfaces
{
    public interface IStripeSessionService
    {
        Task<Stripe.Checkout.Session> GetStripeCheckoutSessionAsync(string id);
        Task<Stripe.BillingPortal.Session> CreateBillingPortalSessionAsync(string redirectBaseUrl, string stripeCustomerId);
        Task<Stripe.Checkout.Session> CreateCheckoutSessionAsync(string redirectBaseUrl, ApplicationUser user, Guid tenantId, StripeSubscriptionType stripeSubscription);
    }
}