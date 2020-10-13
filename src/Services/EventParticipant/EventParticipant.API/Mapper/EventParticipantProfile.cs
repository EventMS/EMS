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
            CreateMap<EventCreatedEvent, Event>()
                .Transform(e => e.EventPrices, e => e.EventPrices);

            CreateMap<Event, CanUserSignUpToEvent>();
            CreateMap<SignUpEventSuccess, EventParticipant>();
        }
    }
}
