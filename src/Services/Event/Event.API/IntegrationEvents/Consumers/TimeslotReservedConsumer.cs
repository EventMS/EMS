using System.Threading.Tasks;
using AutoMapper;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;

namespace EMS.Event_Services.API.Events
{
    public class TimeslotReservedConsumer :
        IConsumer<TimeslotReserved>
    {
        private readonly EventContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public TimeslotReservedConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<TimeslotReserved> context)
        {
            var evt = _context.Events.Find(context.Message.EventId);
            if (evt == null && evt.Status == EventStatus.Pending)
            {
                evt.Status = EventStatus.Confirmed;
                _context.Events.Update(evt);
                var @event = _mapper.Map<EventCreatedEvent>(evt);
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
        }
    }

    public class TimeslotReservationFailedConsumer :
        IConsumer<TimeslotReservationFailed>
    {
        private readonly EventContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public TimeslotReservationFailedConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<TimeslotReservationFailed> context)
        {
            var evt = _context.Events.Find(context.Message.EventId);
            if (evt == null && evt.Status == EventStatus.Pending)
            {
                evt.Status = EventStatus.Confirmed;
                _context.Events.Update(evt);
                var @event = _mapper.Map<EventCreatedEvent>(evt);
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
        }
    }
}