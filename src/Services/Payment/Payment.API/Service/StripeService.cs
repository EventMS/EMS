using System;
using System.Collections.Generic;
using EMS.Payment_Services.API.Context.Model;
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

        public string SignUpToEvent(float price, Guid UserId, Guid EventId)
        {
            long? longPrice = (long?) (price * 100);
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = longPrice,
                Currency = "dkk",
                Metadata = new Dictionary<string, string>()
                {
                    {"EventId", EventId.ToString() },
                    {"UserId", UserId.ToString() }
                }
            });
            return paymentIntent.ClientSecret;
        }

        public string SignUserUpToSubscription(string paymentMethodId, User user, ClubSubscription clubSubscription)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = user.StripeUserId,
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
            customerService.Update(user.StripeUserId, customerOptions);

            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = user.StripeUserId,
                Items = new List<SubscriptionItemOptions>()
                    {
                        new SubscriptionItemOptions
                        {
                            Price = clubSubscription.StripePriceId,
                        },
                    },
                Metadata = new Dictionary<string, string>()
                {
                    {"UserId", user.UserId.ToString()},
                    {"ClubSubscriptionId", clubSubscription.ClubSubscriptionId.ToString()},
                }
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