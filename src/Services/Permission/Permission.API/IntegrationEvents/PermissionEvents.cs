using System;
using EMS.BuildingBlocks.IntegrationEventLogEF;

namespace EMS.Events
{
    public class UserCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public UserCreatedIntegrationEvent(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class ClubCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
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

    public class PermissionCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public PermissionCreatedIntegrationEvent(Guid template1Id, string name)
        {
            PermissionId = template1Id;
            Name = name;
        }
        public Guid PermissionId { get; set; }
        public string Name { get; set; }
    }

    public class PermissionUpdatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid PermissionId { get; set; }
        public string Name { get; set; }

        public PermissionUpdatedIntegrationEvent(Guid template1Id, string name)
        {
            PermissionId = template1Id;
            Name = name;
        }
    }

    public class PermissionDeletedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid PermissionId { get; set; }
        public PermissionDeletedIntegrationEvent(Guid template1Id)
        {
            PermissionId = template1Id;
        }
    }
}
