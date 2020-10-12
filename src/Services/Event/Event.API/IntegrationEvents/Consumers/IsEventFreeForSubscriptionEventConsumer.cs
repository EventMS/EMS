using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EMS.Event_Services.API.Events
{
    public class IsEventFreeForSubscriptionEventConsumer :
        IConsumer<IsEventFreeForSubscriptionEvent>
    {
        private readonly EventContext _context;
        private readonly IEventService _eventService;

        public IsEventFreeForSubscriptionEventConsumer(EventContext context, IEventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        public async Task Consume(ConsumeContext<IsEventFreeForSubscriptionEvent> context)
        {
            var e = await _context.Events.Include(e => e.EventPrices)
                .Where(e => context.Message.EventId == e.EventId).FirstOrDefaultAsync();
            if (e != null)
            {
                if (!e.EventPrices.Any(price =>
                    Math.Abs(price.Price) < 1 && price.ClubSubscriptionId == context.Message.ClubSubscriptionId))
                {
                    return;
                }

                var @event = new SignUpEventSuccess()
                {
                    UserId = context.Message.UserId,
                    EventId = context.Message.EventId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
        }
    }
}