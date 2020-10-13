using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EMS.EventParticipant_Services.API.Events
{
    public class IsEventFreeForSubscriptionEventConsumer :
        IConsumer<IsEventFreeForSubscriptionEvent>
    {
        private readonly EventParticipantContext _context;
        private readonly IEventService _eventService;

        public IsEventFreeForSubscriptionEventConsumer(EventParticipantContext context, IEventService eventService)
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
                    Math.Abs(price.Price.Value) < 1 && price.ClubSubscriptionId == context.Message.ClubSubscriptionId))
                {
                    return;
                }

                await _context.EventParticipants.AddAsync(new EventParticipant()
                {
                    EventId = context.Message.EventId,
                    UserId = context.Message.UserId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}