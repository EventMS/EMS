using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class UserCreatedEvent : Event // All events should inherit from Integration event
    {
        public UserCreatedEvent(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class ClubCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid AdminId { get; set; }
    }
    public class ClubMemberCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid UserId { get; set; }
        public Guid ClubId { get; set; }
        public Guid ClubSubscriptionId { get; set; }
    }

    public class ClubMemberUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid UserId { get; set; }
        public Guid ClubId { get; set; }
        public Guid ClubSubscriptionId { get; set; }
    }
}
