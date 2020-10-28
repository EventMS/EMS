using System;
using System.Collections.Generic;
using EMS.EventParticipant_Services.API.Context.Model;
using Event = EMS.BuildingBlocks.EventLogEF.Event;

namespace EMS.Events
{
    public class SignUpEventSuccessEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }

    public class EventCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public float? PublicPrice { get; set; }

        public Guid ClubId { get; set; }

        public List<EventPrice> EventPrices { get; set; }
    }
}
