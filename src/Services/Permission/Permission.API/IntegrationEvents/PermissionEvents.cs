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
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public Guid AdminId { get; set; }
    }

    public class PermissionCreatedEvent : Event // All events should inherit from Integration event
    {
        public PermissionCreatedEvent(Guid template1Id, string name)
        {
            PermissionId = template1Id;
            Name = name;
        }
        public Guid PermissionId { get; set; }
        public string Name { get; set; }
    }

    public class PermissionUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid PermissionId { get; set; }
        public string Name { get; set; }

        public PermissionUpdatedEvent(Guid template1Id, string name)
        {
            PermissionId = template1Id;
            Name = name;
        }
    }

    public class PermissionDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid PermissionId { get; set; }
        public PermissionDeletedEvent(Guid template1Id)
        {
            PermissionId = template1Id;
        }
    }
}
