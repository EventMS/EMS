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
