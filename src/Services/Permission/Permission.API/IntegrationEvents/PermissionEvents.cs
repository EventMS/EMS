using System;
using EMS.BuildingBlocks.IntegrationEventLogEF;

namespace EMS.Events
{
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
