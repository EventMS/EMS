using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class Template1CreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid Template1Id { get; set; }
        public string Name { get; set; }
    }

    public class Template1UpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid Template1Id { get; set; }
        public string Name { get; set; }
    }

    public class Template1DeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid Template1Id { get; set; }
    }
}