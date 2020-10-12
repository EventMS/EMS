using System;
using System.Collections.Generic;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class SignUpFreeEventSuccess : Event // All events should inherit from Integration event
    {
        public Guid EventParticipantId { get; set; }
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

    public class EventPrice
    {
        public float Price { get; set; }
    }

    public enum EventType
    {
        Public, Private
    }
}
