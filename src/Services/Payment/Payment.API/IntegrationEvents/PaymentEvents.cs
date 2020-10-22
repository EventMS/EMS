using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class UserCreatedEvent : Event // All events should inherit from Integration event
    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }

    public class ClubSubscriptionCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubSubscriptionId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Guid ClubId { get; set; }
    }
}
