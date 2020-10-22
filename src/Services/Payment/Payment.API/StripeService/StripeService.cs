using System;
using System.Collections.Generic;
using Serilog;
using Stripe;

namespace EMS.Payment_Services.API
{
    //These examples are taken straight from stripe documentation with MINOR adjustments. 
    public class StripeService
    {
        public Customer CreateCustomer(string email)
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
            };
            var service = new CustomerService();
            var customer = service.Create(options);
            return customer;
        }


        public Product CreateProduct(Guid clubId, string name)
        {
            var productOptions = new ProductCreateOptions
            {
                Name = clubId + name,
            };
            var productService = new ProductService();
            var product = productService.Create(productOptions);
            return product;
        }

        public Price CreatePrice(long productPrice, Product product)
        {
            var options = new PriceCreateOptions
            {
                UnitAmount = productPrice * 100,
                Currency = "dkk",
                Recurring = new PriceRecurringOptions
                {
                    Interval = "month",
                },
                Product = product.Id,
            };
            var service = new PriceService();
            var price = service.Create(options);
            return price;
        }


        public string SignUserUpToSubscription(string paymentMethodId, string stripeCustomerId, string priceId)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = stripeCustomerId,
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
            customerService.Update(stripeCustomerId, customerOptions);

            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = stripeCustomerId,
                Items = new List<SubscriptionItemOptions>()
                    {
                        new SubscriptionItemOptions
                        {
                            Price = priceId,
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