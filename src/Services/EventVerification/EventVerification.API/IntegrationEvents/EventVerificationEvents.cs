using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class SignUpEventSuccess : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }
}