using System;
using System.Collections.Generic;
using Stripe;

namespace EMS.Club_Service_Services.API
{
    public class StripeService
    {
        public string SignUserUpToSubscription(string paymentMethodId, CurrentUser currentUser, string priceId)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = currentUser.StripeCustomerId,
            };
            var service = new PaymentMethodService();
            var paymentMethod = service.Attach(paymentMethodId, options);

            // Update customer's default invoice payment method
            var customerOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethod.Id,
                },
            };
            var customerService = new CustomerService();
            customerService.Update(currentUser.StripeCustomerId, customerOptions);

            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = currentUser.StripeCustomerId,
                Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Price = Environment.GetEnvironmentVariable(priceId),
            },
        },
            };
            subscriptionOptions.AddExpand("latest_invoice.payment_intent");
            var subscriptionService = new SubscriptionService();
            try
            {
                Subscription subscription = subscriptionService.Create(subscriptionOptions);
                return "Went good";
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Failed to create subscription.{e}");
                return "Went bad";
                // return BadRequest();
            }
        }
    }
}