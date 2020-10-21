

using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using EMS.Club_Service_Services.API;
using EMS.TemplateWebHost.Customization;

namespace EMS.Subscription_Services.API.GraphQlQueries
{
    public class SubscriptionMutations : BaseMutations
    {
        protected readonly SubscriptionContext _context;
        protected readonly IMapper _mapper;
        protected readonly IEventService _eventService;
        protected readonly StripeService _stripeService;

        public SubscriptionMutations(SubscriptionContext context, IEventService eventService, IMapper mapper, IAuthorizationService authorizationService, StripeService stripeService) : base(authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _eventService = eventService;
            _stripeService = stripeService;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> CreateClubSubscriptionAsync(CreateClubSubscriptionRequest request)
        {
            await IsAdminIn(request.ClubId);
            var subscription = _mapper.Map<ClubSubscription>(request);
            

            var productOptions = new ProductCreateOptions
            {
                Name = request.ClubId + request.Name,
            };
            var productService = new ProductService();
            var product = productService.Create(productOptions);

            var options = new PriceCreateOptions
            {
                UnitAmount = request.Price*100,
                Currency = "dkk",
                Recurring = new PriceRecurringOptions
                {
                    Interval = "month",
                },
                Product = product.Id,
            };
            var service = new PriceService();
            var price = service.Create(options);

            subscription.StribePriceId = price.Id;
            subscription.StribeProductId = product.Id;

            _context.ClubSubscriptions.Add(subscription);
            var @event = _mapper.Map<ClubSubscriptionCreatedEvent>(subscription);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return subscription;
        }

        public async Task<string> PaySubscription(CreateSubscriptionRequest req, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var clubSub = await _context.ClubSubscriptions.FindOrThrowAsync(req.ClubSubscriptonId);
            return _stripeService.SignUserUpToSubscription(req.PaymentMethodId, currentUser, clubSub.StribePriceId);
        }

        private static string SignUserUpToSubscription(CreateSubscriptionRequest req, CurrentUser currentUser, ClubSubscription clubSub)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = currentUser.StripeCustomerId,
            };
            var service = new PaymentMethodService();
            var paymentMethod = service.Attach(req.PaymentMethodId, options);

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
                Price = Environment.GetEnvironmentVariable(clubSub.StribePriceId),
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

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubSubscription> UpdateClubSubscriptionAsync(Guid id, UpdateClubSubscriptionRequest request)
        {
            var subscription = await _context.ClubSubscriptions.FindOrThrowAsync(id);

            await IsAdminIn(subscription.ClubId);
            _mapper.Map(request, subscription);
            _context.ClubSubscriptions.Update(subscription);

            var @event = _mapper.Map<ClubSubscriptionUpdatedEvent>(subscription);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return subscription;
        }
    }
}
