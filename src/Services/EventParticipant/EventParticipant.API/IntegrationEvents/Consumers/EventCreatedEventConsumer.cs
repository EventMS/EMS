using AutoMapper;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.Events;
using TemplateWebHost.Customization.BasicConsumers;

namespace EMS.EventParticipant_Services.API.Events
{
    public class EventCreatedEventConsumer :
        BasicDuplicateConsumer<EventParticipantContext, Event, EventCreatedEvent>
    {
        public EventCreatedEventConsumer(EventParticipantContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}