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
            CreateMap<EventCreatedEvent, Event>();
            CreateMap<SignUpEventSuccess, EventParticipant>();
        }
    }
}
