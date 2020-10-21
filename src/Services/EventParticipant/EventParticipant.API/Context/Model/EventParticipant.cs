using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.EventParticipant_Services.API.Context.Model
{
    public class EventParticipant
    {
        public Guid EventParticipantId { get; set; }

        public Event Event { get; set; }

        public Guid EventId { get; set; }

        public Guid UserId { get; set; }

        public EventParticipant() { }
    }

}
