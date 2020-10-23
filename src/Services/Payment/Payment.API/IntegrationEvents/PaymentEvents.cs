using System;
using System.Collections.Generic;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class UserCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }

    public class ClubSubscriptionCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubSubscriptionId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Guid ClubId { get; set; }
    }

    public class EventCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public Guid ClubId { get; set; }

        public List<EventPricePartialEvent> EventPrices { get; set; }

        public float? PublicPrice { get; set; }
    }

    public class EventPricePartialEvent
    {
        public float Price { get; set; }
        public Guid ClubSubscriptionId { get; set; }
    }
}
