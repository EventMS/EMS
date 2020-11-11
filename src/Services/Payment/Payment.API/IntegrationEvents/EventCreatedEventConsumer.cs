using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.Context.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Event = EMS.Payment_Services.API.Context.Model.Event;

namespace EMS.Payment_Services.API.Events
{
    public class EventCreatedEventConsumer :
        IConsumer<EventCreatedEvent>
    {
        private readonly PaymentContext _paymentContext;
        private readonly StripeService _stripeService;

        public EventCreatedEventConsumer(PaymentContext paymentContext, StripeService stripeService)
        {
            _paymentContext = paymentContext;
            _stripeService = stripeService;
        }

        public async Task Consume(ConsumeContext<EventCreatedEvent> context)
        {
            var eventsPresent = await _paymentContext.EventPrices.AnyAsync(e => e.EventId == context.Message.EventId);

            if (!eventsPresent)
            {
                var e = new Event()
                {
                    ClubId = context.Message.ClubId,
                    EventId = context.Message.EventId,
                    PublicPrice = context.Message.PublicPrice,
                    EventPrices = new List<EventPrice>()
                };
                foreach (var eventPricePartialEvent in context.Message.EventPrices)
                {
                    e.EventPrices.Add(new EventPrice(){
                        ClubSubscriptionId = eventPricePartialEvent.ClubSubscriptionId,
                        Price = eventPricePartialEvent.Price
                    });
                }

                await _paymentContext.Events.AddAsync(e);
                await _paymentContext.SaveChangesAsync();
            }
        }
    }
}