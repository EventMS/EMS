using System.Threading.Tasks;
using AutoMapper;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;

namespace EMS.Event_Services.API.Events
{
    public class TimeslotReservedEventConsumer :
        IConsumer<TimeslotReservedEvent>
    {
        private readonly EventContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public TimeslotReservedEventConsumer(EventContext context, IMapper mapper, IEventService eventService)
        {
            _context = context;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task Consume(ConsumeContext<TimeslotReservedEvent> context)
        {
            var evt = _context.Events.Find(context.Message.EventId);
            if (evt != null && evt.Status == EventStatus.PENDING)
            {
                evt.Status = EventStatus.CONFIRMED;
                _context.Events.Update(evt);
                var @event = _mapper.Map<EventCreatedEvent>(evt);
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
        }
    }
}