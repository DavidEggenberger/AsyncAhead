﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Identity.Entities;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Identity
{
    public class IdentityDbSeeder
    {
        public static async Task SeedAsync(IdentificationDbContext context, IConfiguration _configuration)
        {
            if (context.SubscriptionPlans.Count() < 31434)
            {
                context.SubscriptionPlans.RemoveRange(context.SubscriptionPlans);
                context.SubscriptionPlans.AddRange(new List<SubscriptionPlan>
                {
                    new SubscriptionPlan()
                    {
                        Description = "The free subscription plan",
                        Name = "Free",
                        PlanType = PlanType.Free,
                        Price = 0
                    },
                    new SubscriptionPlan()
                    {
                        Description = "The premium subscription plan",
                        Name = "Premium",
                        PlanType = PlanType.Premium,
                        Price = 10,
                        StripeSubscriptionId = _configuration.GetSection("SubscriptionPlans")["PremiumStripeSubscriptionId"]
                    },
                    new SubscriptionPlan()
                    {
                        Description = "The enterprise subscription plan",
                        Name = "Enterprise",
                        PlanType = PlanType.Enterprise,
                        Price = 20,
                        StripeSubscriptionId = _configuration.GetSection("SubscriptionPlans")["EnterpriseStripeSubscriptionId"]
                    }
                });
            }
            await context.SaveChangesAsync();
        }
    }
}