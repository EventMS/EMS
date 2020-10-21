using System;
using System.Collections.Generic;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
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

    public class InstructorAddedEvent : Event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
    }

    public class InstructorDeletedEvent: Event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
    }

    public class ClubCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public Guid AdminId { get; set; }
        public List<String> Locations { get; set; }
    }

    public class ClubUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
    }

    public class ClubDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
    }
}
