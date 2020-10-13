using System;
using System.Collections.Generic;
using EMS.EventParticipant_Services.API.Context.Model;
using Event = EMS.BuildingBlocks.EventLogEF.Event;

namespace EMS.Events
{
    public class CanUserSignUpToEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }

    public class SignUpEventSuccess : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }

    public class EventCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public Guid ClubId { get; set; }

        public EventType EventType { get; set; }

        public List<EventPrice> EventPrices { get; set; }
    }

    public class IsEventFreeForSubscriptionEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubSubscriptionId { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }


    public enum EventType
    {
        Public, Private
    }
}
