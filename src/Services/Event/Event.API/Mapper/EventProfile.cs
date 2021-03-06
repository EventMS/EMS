using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using EMS.Events;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization;
using EventPrice = EMS.Event_Services.API.Context.Model.EventPrice;


namespace EMS.Event_Services.API.Mapper
{


    public class EventProfile : Profile
    {
        public EventProfile()
        {

            CreateMap<Guid, RoomEvent>().ConvertUsing(s => new RoomEvent()
            {
                RoomId = s
            });

            CreateMap<RoomEvent, Guid>().ConvertUsing(room => room.RoomId);

            CreateMap<Guid, InstructorForEvent>().ConvertUsing(s => new InstructorForEvent()
            {
                InstructorId = s
            });

            CreateMap<EventPrice, EMS.Events.EventPrice>().ConvertUsing(s => new EMS.Events.EventPrice()
            {
                ClubSubscriptionId = s.ClubSubscriptionId,
                Price = s.Price
            });

            CreateMap<ClubCreatedEvent, Club>();
            CreateMap<ClubSubscriptionCreatedEvent, ClubSubscription>();
            CreateMap<InstructorAddedEvent, Instructor>()
                .Transform(i => i.InstructorId, i => i.UserId);

            CreateMap<CreateEventRequest, Event>();
            CreateMap<UpdateEventRequest, Event>();
            CreateMap<Event, EventCreatedEvent>();
            CreateMap<Event, EventUpdatedEvent>();
            CreateMap<Event, EventDeletedEvent>();
            CreateMap<Event, EventCreationFailedEvent>();
            CreateMap<Event, VerifyAvailableTimeslotEvent>();
            CreateMap<Event, VerifyChangedTimeslotEvent>();
            CreateMap<EventPriceRequest, Context.Model.EventPrice>();
        }
    }
}
