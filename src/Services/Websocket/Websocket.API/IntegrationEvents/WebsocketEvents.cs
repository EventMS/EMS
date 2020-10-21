using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class EventCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public Guid ClubId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float? PublicPrice { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
