using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
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

    public class ClubMemberDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid UserId { get; set; }
        public Guid ClubId { get; set; }
        public Guid ClubSubscriptionId { get; set; }
    }

    public class ClubSubscriptionCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubSubscriptionId { get; set; }
        public Guid ClubId { get; set; }
    }

    public class IsUserClubMemberEvent : Event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
    }

    public class UserIsClubMemberEvent : Event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
    }
}
