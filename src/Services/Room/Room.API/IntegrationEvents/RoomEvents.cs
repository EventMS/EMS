using System;
using System.Collections.Generic;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class RoomCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
    }

    public class RoomUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
    }

    public class RoomDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }

        public string Name { get; set; }
    }

    public class ClubCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public List<String> Locations { get; set; }
    }

    public class VerifyAvailableTimeslotEvent : Event // All events should inherit from Integration event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid EventId { get; set; }
        public Guid RoomId { get; set; }
    }

    public class TimeslotReservationFailed : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid RoomId { get; set; }
        public string Reason { get; set; }
    }

    public class TimeslotReserved : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid RoomId { get; set; }
    }
}
