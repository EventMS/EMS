using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{ 
    public class ClubCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
    }
    public class ClubSubscriptionCreatedEvent : Event // All events should inherit from Integration event
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Guid ClubId { get; set; }
    }

    public class ClubSubscriptionUpdatedEvent : Event // All events should inherit from Integration event
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Guid ClubId { get; set; }
    }
}
