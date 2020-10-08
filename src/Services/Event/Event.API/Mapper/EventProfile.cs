using AutoMapper;
using EMS.Events;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Controllers.Request;

namespace EMS.Event_Services.API.Mapper
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<CreateEventRequest, Event>();
            CreateMap<UpdateEventRequest, Event>();
            CreateMap<Event, EventCreatedEvent>();
            CreateMap<Event, EventUpdatedEvent>();
            CreateMap<Event, EventDeletedEvent>();
            CreateMap<Event, EventCreationFailed>();
        }
    }
}
