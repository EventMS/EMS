using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class EventCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public Guid ClubId { get; set; }

        public string Name { get; set; }
    }

    public class EventCreationFailedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public string Reason { get; set; }
        public Guid ClubId { get; set; }
    }

    public class SignUpSubscriptionSuccessEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubSubscriptionId { get; set; }
        public Guid UserId { get; set; }
    }
}
