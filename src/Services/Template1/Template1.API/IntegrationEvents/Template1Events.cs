using System;
using EMS.BuildingBlocks.IntegrationEventLogEF;

namespace EMS.Events
{
    public class Template1CreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid Template1Id { get; set; }
        public string Name { get; set; }
    }

    public class Template1UpdatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid Template1Id { get; set; }
        public string Name { get; set; }
    }

    public class Template1DeletedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid Template1Id { get; set; }
    }
}