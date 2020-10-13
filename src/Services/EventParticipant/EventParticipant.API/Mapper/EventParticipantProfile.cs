using System;
using System.Linq;
using AutoMapper;
using EMS.Events;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization;
using Microsoft.EntityFrameworkCore.Internal;

namespace EMS.EventParticipant_Services.API.Mapper
{
    public class EventParticipantProfile : Profile
    {
        public EventParticipantProfile()
        {
            CreateMap<EventParticipant, CanUserSignUpToEvent>();
            //Anything above price point of 1 will not be regarded as completly free. 
            double TOLERANCE = 1;
            CreateMap<EventCreatedEvent, Event>()
                .Transform(e => e.IsFree, e => !e.EventPrices.Any(price => Math.Abs(price.Price.Value) >= TOLERANCE));

            CreateMap<Event, CanUserSignUpToEvent>();
            CreateMap<SignUpEventSuccess, EventParticipant>();
        }
    }
}
