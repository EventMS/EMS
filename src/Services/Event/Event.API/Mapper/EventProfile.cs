using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using EMS.Events;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Controllers.Request;



namespace EMS.Event_Services.API.Mapper
{
    public static class MyExtensions
    {
        public static IMappingExpression<TSource, TDestination> Transform<TSource, TDestination, TSourceMember>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector,
            Expression<Func<TSource, TSourceMember>> transform
        )
        {
            map.ForMember(selector, config =>
                config.MapFrom(transform));
            return map;
        }
    }

    public class RoomEventFormatter : IValueConverter<Guid, RoomEvent>
    {
        public RoomEvent Convert(Guid source, ResolutionContext context)
            => new RoomEvent()
            {
                RoomId = source
            };
    }

    public class EventProfile : Profile
    {
        public EventProfile()
        {

            CreateMap<Guid, RoomEvent>().ConvertUsing(s => new RoomEvent()
            {
                RoomId = s
            });

            CreateMap<Guid, InstructorForEvent>().ConvertUsing(s => new InstructorForEvent()
            {
                InstructorId = s
            });

            CreateMap<CreateEventRequest, Event>();
            CreateMap<UpdateEventRequest, Event>();
            CreateMap<Event, EventCreatedEvent>();
            CreateMap<Event, EventUpdatedEvent>();
            CreateMap<Event, EventDeletedEvent>();
            CreateMap<Event, EventCreationFailed>();
            CreateMap<Event, VerifyAvailableTimeslotEvent>();
            CreateMap<SubscriptionEventPriceRequest, ClubSubscriptionEventPrice>();
        }
    }
}
